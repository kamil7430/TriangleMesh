namespace TriangleMesh.Views.Helpers;

public static class DoubleHelper
{
    public static double TruncateToZeroOne(this double d)
        => d switch
        {
            > 1 => 1,
            < 0 => 0,
            _ => d
        };
}