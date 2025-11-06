namespace TriangleMesh.Models;

public class Mesh
{
    private readonly int _precision;
    private readonly BezierPolygon _bezierPolygon;
    
    public Vertex[] Vertices { get; private set; }
    public Triangle[] Triangles { get; private set; }

    public Mesh(int precision, BezierPolygon bezierPolygon)
    {
        _precision = precision;
        _bezierPolygon = bezierPolygon;
        int triangleCount = 2 * _precision * _precision;

        Triangles = new Triangle[triangleCount];
    }
}