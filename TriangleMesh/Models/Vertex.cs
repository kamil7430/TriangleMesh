using System.Numerics;
using Avalonia;
using TriangleMesh.Models.Algorithms;
using TriangleMesh.Models.Helpers;

namespace TriangleMesh.Models;

public class Vertex
{
    public int U { get; }
    public int V { get; }
    public int Precision { get; }
    
    public double UFraction 
        => (double)U / Precision;

    public double VFraction
        => (double)V / Precision;

    public Vector3D P { get; }
    public Vector3D Pu { get; }
    public Vector3D Pv { get; }
    public Vector3D N
        => Pu.Cross(Pv);

    public Vector3D PostRotationP
        => P.Rotate();

    public Vector3D PostRotationPu
        => Pu.Rotate();

    public Vector3D PostRotationPv
        => Pv.Rotate();

    public Vector3D PostRotationN
        => N.Rotate();

    public Vertex(int u, int v, int precision, BezierPolygon bezierPolygon)
    {
        U = u;
        V = v;
        Precision = precision;
        P = DeCasteljau.FindPointCoords(bezierPolygon.ControlPoints, UFraction, VFraction);
    }
}
