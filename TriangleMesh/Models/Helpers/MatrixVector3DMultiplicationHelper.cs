using Avalonia;

namespace TriangleMesh.Models.Helpers;

public static class MatrixVector3DMultiplicationHelper
{
    public static Vector3D MultiplicateBy(this Matrix m, Vector3D v)
        => new Vector3D(
            m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z,
            m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z,
            m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z
        );
}