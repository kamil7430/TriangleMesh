using Avalonia;
using System;

namespace TriangleMesh.Models.Helpers;

public static class RotationHelper
{
    private static double _alpha;
    public static double Alpha
    {
        get => _alpha;
        set
        {
            _alpha = value;
            CalculateRotationMatrix();
        }
    }

    private static double _beta;
    public static double Beta
    {
        get => _beta;
        set
        {
            _beta = value;
            CalculateRotationMatrix();
        }
    }

    private static Matrix _rotationMatrix;

    public static Vector3D Rotate(this Vector3D v)
        => _rotationMatrix.MultiplicateBy(v);

    public static double ToRadians(this double degrees)
        => Math.PI / 180.0 * degrees;

    private static void CalculateRotationMatrix()
        => _rotationMatrix = GetXRotationMatrix(Beta.ToRadians()) * GetZRotationMatrix(Alpha.ToRadians());

    private static Matrix GetXRotationMatrix(double beta)
        => new Matrix(
            1, 0, 0,
            0, Math.Cos(beta), -Math.Sin(beta),
            0, Math.Sin(beta), Math.Cos(beta)
        );

    private static Matrix GetZRotationMatrix(double alpha)
        => new Matrix(
            Math.Cos(alpha), -Math.Sin(alpha), 0,
            Math.Sin(alpha), Math.Cos(alpha), 0,
            0, 0, 1
        );
}