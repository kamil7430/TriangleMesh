using System.Collections.Generic;
using System.Linq;
using Avalonia;
using TriangleMesh.Views.Helpers;

namespace TriangleMesh.ViewModels;

public partial class MainWindowViewModel
{
    public IEnumerable<(Vector, Vector)> GetBezierPolygonEdges()
        => BezierPolygon.GetEdges().Select(t => (t.Cp1.PostRotationP.ToVector().ModelToCanvas(),
            t.Cp2.PostRotationP.ToVector().ModelToCanvas()));

    public IEnumerable<(Vector, Vector)> GetTriangleMeshEdges()
        => Mesh.GetTriangleMeshEdges().Select(t => (t.V1.PostRotationP.ToVector().ModelToCanvas(),
            t.V2.PostRotationP.ToVector().ModelToCanvas()));
}