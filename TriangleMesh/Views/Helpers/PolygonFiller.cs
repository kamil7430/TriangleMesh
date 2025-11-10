using System.Collections.Generic;
using System.Linq;
using Avalonia;
using TriangleMesh.Models;
using TriangleMesh.Models.DataStructures;

namespace TriangleMesh.Views.Helpers;

public class PolygonFiller
{
    private class AetEntry(double yMax, double x, double delta)
    {
        public double YMax { get; } = yMax;
        public double X { get; set; } = x;
        public double Delta { get; } = delta;
    }

    private static readonly MyLinkedList<AetEntry>[] _et
        = new MyLinkedList<AetEntry>[CoordsTranslator.DRAWING_AREA_HEIGHT];
        
    private double _kD;
    private double _kS;
    private Rgb _iL;
    private Rgb _iO;
    private Vector3D _l;
    private Vector3D _n;
    private Vector3D _v;
    private Vector3D _r;
    private int _m;

    public PolygonFiller(double kD, double kS, Rgb iL, Rgb iO, Vector3D l, Vector3D n, int m)
    {
        _kD = kD;
        _kS = kS;
        _iL = iL;
        _iO = iO;
        _l = Vector3D.Normalize(l);
        _n = Vector3D.Normalize(n);
        _v = new Vector3D(0, 0, 1);
        _r = Vector3D.Normalize(_n * Vector3D.Dot(_n, _l) * 2 - _l);
        _m = m;
    }

    public IEnumerable<(PixelVector Vector, double Z, uint Color)> GetPixelsToPaint(IEnumerable<(Vertex, Vertex)> edges)
    {
        FillET(edges);
        
        int y = 0;
        while (y < CoordsTranslator.DRAWING_AREA_HEIGHT && _et[y].Count <= 0)
            y++;
        
        var aet = new MyLinkedList<AetEntry>();

        while (aet.Count > 0 || y < CoordsTranslator.DRAWING_AREA_HEIGHT)
        {
            if (y < CoordsTranslator.DRAWING_AREA_HEIGHT)
                aet.PushBack(_et[y]);
            
            var sortedAet = aet.OrderBy(e => e.X);

            // TODO: wypełnianie

            aet = new MyLinkedList<AetEntry>(sortedAet.Where(e => (int)e.YMax < y));
            
            y++;

            foreach (var e in aet)
                e.X += e.Delta;
        }

        return [];
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
            
            _et[(int)aCanvas.Y].PushBack(new AetEntry(
                bCanvas.Y,
                aCanvas.X,
                (bCanvas.X - aCanvas.X) / (bCanvas.Y - aCanvas.Y)
            ));
        }
    }

    private double MyCos(Vector3D v1, Vector3D v2)
    {
        var result = v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        return result <= 0 ? 0 : result;
    }
}