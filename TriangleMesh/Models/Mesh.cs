namespace TriangleMesh.Models;

public class Mesh
{
    public Vertex[,] Vertices { get; private set; }
    public Triangle[,,] Triangles { get; private set; }

    public Mesh(int precision, BezierPolygon bezierPolygon)
    {
        Vertices = new Vertex[precision + 1, precision + 1];
        for (int u = 0; u <= precision; u++)
            for (int v = 0; v <= precision; v++)
                Vertices[u, v] = new Vertex(u, v, precision, bezierPolygon);
        
        Triangles = new Triangle[precision, precision, 2];
        for (int u = 0; u < precision; u++)
            for (int v = 0; v < precision; v++)
            {
                Triangles[u, v, 0] = new Triangle(Vertices[u, v], Vertices[u + 1, v], Vertices[u + 1, v + 1]);
                Triangles[u, v, 1] = new Triangle(Vertices[u, v], Vertices[u, v + 1], Vertices[u + 1, v + 1]);
            }
    }
}