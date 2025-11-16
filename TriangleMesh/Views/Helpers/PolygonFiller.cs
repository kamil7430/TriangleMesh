using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using TriangleMesh.Models;
using TriangleMesh.Models.DataStructures;

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

    private static MyLinkedList<AetEntry>[]? _et;
    private static MyLinkedList<AetEntry>[] ET
    {
        get
        {
            if (_et == null)
            {
                _et = new MyLinkedList<AetEntry>[CoordsTranslator.DRAWING_AREA_HEIGHT];
                for (int i = 0; i < _et.Length; i++)
                    _et[i] = new MyLinkedList<AetEntry>();
            }
            return _et;
        }
    }
        
    private double _kD;
    private double _kS;
    private Rgb _iL;
    private Rgb? _iOColor;
    private Bitmap? _iOSource;
    private Vector3D _l;
    private Vector3D _v;
    private int _m;

    private Rgb GetObjectColorInPoint(double u, double v)
    {
        if (_iOColor != null)
            return _iOColor.Value;
        else
        {
            throw new NotImplementedException();
        }
    }

    public PolygonFiller(double kD, double kS, Color iL, Color? iOColor, Bitmap? iOSource, Vector3D l, int m)
    {
        _kD = kD;
        _kS = kS;
        _iL = Rgb.FromColor(iL);
        _iOColor = iOColor == null ? null : Rgb.FromColor(iOColor.Value);
        _iOSource = iOSource;
        _l = Vector3D.Normalize(l);
        _v = new Vector3D(0, 0, 1);
        _m = m;
    }

    public IEnumerable<(PixelVector Vector, uint Color)> GetPixelsToPaint(IEnumerable<(Vertex, Vertex)> edges,
        Triangle t, double[,] zBuffer)
    {
        FillET(edges);
        
        int y = 0;
        while (y < CoordsTranslator.DRAWING_AREA_HEIGHT && ET[y].Count <= 0)
            y++;

        var S_abc = CalculateS(t.V1.PostRotationP.ToVector(), t.V2.PostRotationP.ToVector(),
            t.V3.PostRotationP.ToVector());
        
        var aet = new MyLinkedList<AetEntry>();

        while (aet.Count > 0 || y < CoordsTranslator.DRAWING_AREA_HEIGHT)
        {
            if (y < CoordsTranslator.DRAWING_AREA_HEIGHT)
                aet.PushBack(ET[y]);
            
            var sortedAet = aet.OrderBy(e => e.X).ToList();

            if (sortedAet.Count % 2 != 0)
                throw new Exception("Nie ma parzystej ilości krawędzi!");
            
            for (int i = 0; i < sortedAet.Count; i += 2)
            {
                for (int x = (int)sortedAet[i].X; x <= sortedAet[i + 1].X; x++)
                {
                    var p = new Vector(x, y).CanvasToModel();
                    var S_pbc = CalculateS(p, t.V2.PostRotationP.ToVector(), t.V3.PostRotationP.ToVector());
                    var S_apc = CalculateS(t.V1.PostRotationP.ToVector(), p, t.V3.PostRotationP.ToVector());
                    var alpha = S_pbc / S_abc;
                    var beta = S_apc / S_abc;
                    var gamma = 1 - alpha - beta;

                    var z = t.V1.PostRotationP.Z * alpha + t.V2.PostRotationP.Z * beta + t.V3.PostRotationP.Z * gamma;
                    if (zBuffer[x, y] < z) // TODO: zweryfikować!
                        continue;
                    zBuffer[x, y] = z;

                    var N = Vector3D.Normalize(t.V1.PostRotationN * alpha + t.V2.PostRotationN * beta +
                                               t.V3.PostRotationN * gamma);
                    var R = CalculateR(N);
                    
                    var u = t.V1.UFraction * alpha + t.V2.UFraction * beta + t.V3.UFraction * gamma;
                    var v = t.V1.VFraction * alpha + t.V2.VFraction * beta + t.V3.VFraction * gamma;
                    var iO = GetObjectColorInPoint(u, v);
                    
                    double r = 0, g = 0, b = 0;
                    
                    r = _kD * _iL.R * iO.R * MyCos(N, _l, 1) + _kS * _iL.R * iO.R * MyCos(_v, R, _m);
                    g = _kD * _iL.G * iO.G * MyCos(N, _l, 1) + _kS * _iL.G * iO.G * MyCos(_v, R, _m);
                    b = _kD * _iL.B * iO.B * MyCos(N, _l, 1) + _kS * _iL.B * iO.B * MyCos(_v, R, _m);
                    
                    yield return (new PixelVector(x, y), new Rgb(r, g, b).ToUint());
                }
            }
            
            aet = new MyLinkedList<AetEntry>(sortedAet.Where(e => (int)e.YMax > y + 1));
            
            y++;

            foreach (var e in aet)
                e.X += e.Delta;
        }
    }

    private void FillET(IEnumerable<(Vertex, Vertex)> edges)
    {
        foreach (var (v1, v2) in edges)
        {
            (Vertex a, Vertex b) = (v1, v2);
            if (a.PostRotationP.Y > b.PostRotationP.Y)
                (a, b) = (b, a);
            
            // y_min jest w a
            var aCanvas = a.PostRotationP.ToVector().ModelToCanvas();
            var bCanvas = b.PostRotationP.ToVector().ModelToCanvas();
            
            // pominięcie krótkiej krawędzi
            if (bCanvas.Y - aCanvas.Y < 1)
                continue;
            
            ET[(int)aCanvas.Y].PushBack(new AetEntry(
                aCanvas.Y,
                bCanvas.Y,
                aCanvas.X,
                (bCanvas.X - aCanvas.X) / (bCanvas.Y - aCanvas.Y)
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

    private Vector3D CalculateR(Vector3D n)
        => Vector3D.Normalize(n * Vector3D.Dot(n, _l) * 2 - _l);

    private double CalculateS(Vector v1, Vector v2, Vector v3)
        => (v2.X - v1.X) * (v3.Y - v1.Y) - (v3.X - v1.X) * (v2.Y - v1.Y);
}