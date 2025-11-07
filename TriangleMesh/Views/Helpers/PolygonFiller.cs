using System;
using System.Collections.Generic;
using Avalonia;
using TriangleMesh.Models;

namespace TriangleMesh.Views.Helpers;

public class PolygonFiller
{
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

    public IEnumerable<(PixelVector Vector, double Z)> GetPixelsToPaint(Vertex[] vertices)
    {
        throw new NotImplementedException();
    }
}