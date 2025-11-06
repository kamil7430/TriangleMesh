using Avalonia;
using TriangleMesh.Models.Helpers;

namespace TriangleMesh.Models;

public class Vertex
{
    public double U { get; set; }
    public double V { get; set; }

    public Vector3D P { get; set; }
    public Vector3D Pu { get; set; }
    public Vector3D Pv { get; set; }
    public Vector3D N { get; set; }

    public Vector3D PostRotationP
        => P.Rotate();

    public Vector3D PostRotationPu
        => Pu.Rotate();

    public Vector3D PostRotationPv
        => Pv.Rotate();

    public Vector3D PostRotationN
        => N.Rotate();
}
