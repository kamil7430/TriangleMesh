using System.Collections.Generic;

namespace TriangleMesh.Models;

public class Triangle
{
    public Vertex V1 { get; set; }
    public Vertex V2 { get; set; }
    public Vertex V3 { get; set; }

    public Triangle(Vertex v1, Vertex v2, Vertex v3)
    {
        V1 = v1;
        V2 = v2;
        V3 = v3;
    }

    public IEnumerable<(Vertex, Vertex)> GetEdges()
    {
        yield return (V1, V2);
        yield return (V1, V3);
        yield return (V2, V3);
    }
}
