using System.Collections.Generic;
using Avalonia;

namespace TriangleMesh.Models.Algorithms;

public static class DeCasteljau
{
    public static Vector3D FindPointCoords(ControlPoint[,] controlPoints, double u, double v)
    {
        var newVectors = new List<Vector3D>(controlPoints.GetLength(0));
        
        for (int i = 0; i < controlPoints.GetLength(0); i++)
        {
            var vectors = new List<Vector3D>(controlPoints.GetLength(1));
            for (int j = 0; j < controlPoints.GetLength(1); j++)
                vectors[j] = controlPoints[i, j].P;
            
            newVectors.Add(Lerp(vectors, u));
        }
        
        return Lerp(newVectors, v);
    }

    public static Vector3D Lerp(List<Vector3D> vectors, double t)
    {
        for (int i = vectors.Count - 1; i > 0; i++)
            for (int j = 0; j < i; j++)
                vectors[j] = vectors[j] * (1 - t) + vectors[j + 1] * t;
        
        return vectors[0];
    }
}