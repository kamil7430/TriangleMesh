using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using TriangleMesh.Models;
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

    public IEnumerable<Triangle> GetTriangles()
        => Mesh.Triangles.Cast<Triangle>();

    public Rgb GetLightColor()
        => new Rgb(LightColor.R, LightColor.G, LightColor.B);

    public Vector3D GetLightVector()
        => new Vector3D(
            LightVectorVisualLength * Math.Sin(CurrentLightAngle),
            LightVectorVisualLength * Math.Cos(CurrentLightAngle),
            ZLightAnimationPosition
        );
}