using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using TriangleMesh.Models;
using TriangleMesh.Models.Helpers;

namespace TriangleMesh.Views.Helpers;

public class PolygonFiller
{
    private class AetEntry(double yStart, double yMax, double x, double delta)
    {
        public double YStart { get; } = yStart;
        public double YMax { get; } = yMax;
        public double X { get; set; } = x;
        public double Delta { get; } = delta;
    }

    private static List<AetEntry>[]? _et;
    private static List<AetEntry>[] ET
    {
        get
        {
            if (_et == null)
            {
                _et = new List<AetEntry>[CoordsTranslator.DRAWING_AREA_HEIGHT];
                for (int i = 0; i < _et.Length; i++)
                    _et[i] = new List<AetEntry>();
            }
            return _et;
        }
    }
        
    private readonly double _kD;
    private readonly double _kS;
    private readonly Rgb _iL;
    private readonly Rgb? _iOColor;
    private readonly ILockedFramebuffer? _iOSource;
    private readonly Vector3D _l;
    private readonly Vector3D _v;
    private readonly int _m;
    private readonly ILockedFramebuffer? _nVMSource;

    private Rgb GetObjectColorInPoint(double u, double v)
    {
        if (_iOColor != null)
            return _iOColor.Value;

        if (_iOSource != null)
            return GetPixelFromLockedFramebuffer(_iOSource, u, v);

        throw new InvalidOperationException("Both texture sources are null!");
    }
    
    private unsafe Rgb GetPixelFromLockedFramebuffer(ILockedFramebuffer lockedFramebuffer, double u, double v)
    {
        var ptr = (uint*)lockedFramebuffer.Address;
        var size = lockedFramebuffer.Size;
            
        var x = (int)Math.Round(size.Width * u);
        var y = (int)Math.Round(size.Height * v);

        return Rgb.FromUint(*(ptr + y * lockedFramebuffer.RowBytes / sizeof(uint) + x));
    }

    public PolygonFiller(double kD, double kS, Color iL, Color? iOColor, ILockedFramebuffer? iOSource, Vector3D l,
        int m, ILockedFramebuffer? nVMSource)
    {
        _kD = kD;
        _kS = kS;
        _iL = Rgb.FromColor(iL);
        _iOColor = iOColor == null ? null : Rgb.FromColor(iOColor.Value);
        _iOSource = iOSource;
        _l = l;
        _v = new Vector3D(0, 0, 1);
        _m = m;
        _nVMSource = nVMSource;
    }

    public IEnumerable<(PixelVector Vector, uint Color)> GetPixelsToPaint(Triangle t, double[,] zBuffer)
    {
        FillET(t.GetEdges());

        var v1p = t.V1.PostRotationP;
        var v2p = t.V2.PostRotationP;
        var v3p = t.V3.PostRotationP;

        var v1n = t.V1.PostRotationN;
        var v2n = t.V2.PostRotationN;
        var v3n = t.V3.PostRotationN;
        
        int y = 0;
        while (y < CoordsTranslator.DRAWING_AREA_HEIGHT && ET[y].Count <= 0)
            y++;

        var S_abc = CalculateS(v1p.ToVector(), v2p.ToVector(), v3p.ToVector());
        
        if (Math.Abs(S_abc) < 1e-6)
            yield break;
        
        var aet = new List<AetEntry>();

        while (y < CoordsTranslator.DRAWING_AREA_HEIGHT)
        {
            aet.AddRange(ET[y]);
            
            aet.RemoveAll(e => !(y < (int)Math.Round(e.YMax)));
            
            aet.Sort((e1, e2) => e1.X.CompareTo(e2.X));

            if (aet.Count % 2 != 0)
                throw new Exception($"Nie ma parzystej ilości krawędzi! (y={y}, count={aet.Count})");
            
            for (int i = 0; i < aet.Count; i += 2)
            {
                int startX = (int)Math.Round(aet[i].X);
                int endX = (int)Math.Round(aet[i + 1].X);

                for (int x = startX; x <= endX; x++)
                {
                    if (x < 0 || x >= CoordsTranslator.DRAWING_AREA_WIDTH)
                        continue;
                        
                    var p = new Vector(x, y).CanvasToModel();
                    var S_pbc = CalculateS(p, v2p.ToVector(), v3p.ToVector());
                    var S_apc = CalculateS(v1p.ToVector(), p, v3p.ToVector());
                    var alpha = S_pbc / S_abc;
                    var beta = S_apc / S_abc;
                    var gamma = 1 - alpha - beta;

                    var z = v1p.Z * alpha + v2p.Z * beta + v3p.Z * gamma;
                    
                    if (z < zBuffer[x, y])
                        continue;
                    zBuffer[x, y] = z;

                    var N = Vector3D.Normalize(v1n * alpha + v2n * beta + v3n * gamma);
                    
                    var u = (t.V1.UFraction * alpha + t.V2.UFraction * beta + t.V3.UFraction * gamma).TruncateToZeroOne();
                    var v = (t.V1.VFraction * alpha + t.V2.VFraction * beta + t.V3.VFraction * gamma).TruncateToZeroOne();
                    
                    if (_nVMSource != null)
                    {
                        var Pu = Vector3D.Normalize(t.V1.PostRotationPu * alpha + t.V2.PostRotationPu * beta +
                                                    t.V3.PostRotationPu * gamma);
                        var Pv = Vector3D.Normalize(t.V1.PostRotationPv * alpha + t.V2.PostRotationPv * beta +
                                                    t.V3.PostRotationPv * gamma);
                        N = CalculateModifiedNormalVector(Pu, Pv, N, u, v);
                    }
                    
                    var L = Vector3D.Normalize(_l - new Vector3D(p.X, p.Y, z));
                    var R = CalculateR(N, L);
                    
                    var iO = GetObjectColorInPoint(u, v);
                    
                    double r = 0, g = 0, b = 0;
                    var cosNL = MyCos(N, L, 1);
                    var cosVR = MyCos(_v, R, _m);
                    
                    r = (_kD * _iL.R * iO.R * cosNL + _kS * _iL.R * iO.R * cosVR).TruncateToZeroOne();
                    g = (_kD * _iL.G * iO.G * cosNL + _kS * _iL.G * iO.G * cosVR).TruncateToZeroOne();
                    b = (_kD * _iL.B * iO.B * cosNL + _kS * _iL.B * iO.B * cosVR).TruncateToZeroOne();
                    
                    yield return (new PixelVector(x, y), new Rgb(r, g, b).ToUint());
                }
            }
            
            y++;

            foreach (var e in aet)
                e.X += e.Delta;
        }
    }

    private void FillET(IEnumerable<(Vertex, Vertex)> edges)
    {
        for (int i = 0; i < ET.Length; i++)
            ET[i].Clear();
            
        foreach (var (v1, v2) in edges)
        {
            (Vertex a, Vertex b) = (v1, v2);
            if (a.PostRotationP.Y > b.PostRotationP.Y)
                (a, b) = (b, a);
            
            var aCanvas = a.PostRotationP.ToVector().ModelToCanvas();
            var bCanvas = b.PostRotationP.ToVector().ModelToCanvas();
            
            int startY = (int)Math.Round(aCanvas.Y);
            int endY = (int)Math.Round(bCanvas.Y);
            
            if (startY == endY)
                continue;
            
            if (startY < 0) 
                startY = 0;
            if (startY >= CoordsTranslator.DRAWING_AREA_HEIGHT) 
                continue;

            double delta = 100;
            if (bCanvas.Y - aCanvas.Y > 1)
                delta = (bCanvas.X - aCanvas.X) / (bCanvas.Y - aCanvas.Y);
            
            ET[startY].Add(new AetEntry(
                aCanvas.Y,
                bCanvas.Y,
                aCanvas.X,
                delta
            ));
        }
    }

    private double MyCos(Vector3D v1, Vector3D v2, int power)
    {
        var baseRes = v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        
        if (baseRes <= 0)
            return 0;
        
        var result = baseRes;
        while (power > 1)
        {
            result *= baseRes;
            power--;
        }

        return result;
    }

    private Vector3D CalculateR(Vector3D n, Vector3D l)
        => Vector3D.Normalize(n * Vector3D.Dot(n, l) * 2 - l);

    private double CalculateS(Vector v1, Vector v2, Vector v3)
        => (v2.X - v1.X) * (v3.Y - v1.Y) - (v3.X - v1.X) * (v2.Y - v1.Y);

    private Vector3D CalculateModifiedNormalVector(Vector3D Pu, Vector3D Pv, Vector3D N, double u, double v)
    {
        var color = GetPixelFromLockedFramebuffer(_nVMSource!, u, v);
        var N_texture = new Vector3D(
            (color.R - 0.5) * 2.0,
            (color.G - 0.5) * 2.0,
            (color.B - 0.5) * 2.0
        );
        
        var M = new Matrix(
            Pu.X, Pv.X, N.X,
            Pu.Y, Pv.Y, N.Y,
            Pu.Z, Pv.Z, N.Z
        );

        return Vector3D.Normalize(M.MultiplicateBy(N_texture));
    }
}