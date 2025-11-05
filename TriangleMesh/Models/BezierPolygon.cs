using Avalonia;

namespace TriangleMesh.Models;

public class BezierPolygon
{
    public Vector3D[,] ControlPoints { get; private set; }
}