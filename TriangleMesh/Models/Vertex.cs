using Avalonia;

namespace TriangleMesh.Models;

public class Vertex
{
    public double U { get; set; }
    public double V { get; set; }

    public Vector3D P { get; set; }
    public Vector3D Pu { get; set; }
    public Vector3D Pv { get; set; }
    public Vector3D N { get; set; }

    public Vector3D PostRotationP { get; set; }
    public Vector3D PostRotationPu { get; set; }
    public Vector3D PostRotationPv { get; set; }
    public Vector3D PostRotationN { get; set; }
}
