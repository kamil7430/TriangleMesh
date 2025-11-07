using Avalonia;
using TriangleMesh.Models.Helpers;

namespace TriangleMesh.Models;

public class ControlPoint
{
    public Vector3D P { get; set; }

    public Vector3D PostRotationP
        => P.Rotate();

    public ControlPoint(double x, double y, double z)
    {
        P = new Vector3D(x, y, z);
    }

    public ControlPoint(Vector3D p)
    {
        P = p;
    }
}