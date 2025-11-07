using Avalonia;

namespace TriangleMesh.Models.Helpers;

public static class Vector3DHelper
{
    public static Vector3D Cross(this Vector3D a, Vector3D b)
    {
        double newX = a.Y * b.Z - a.Z * b.Y;
        double newY = a.Z * b.X - a.X * b.Z;
        double newZ = a.X * b.Y - a.Y * b.X;
        
        return new Vector3D(newX, newY, newZ);
    }
}