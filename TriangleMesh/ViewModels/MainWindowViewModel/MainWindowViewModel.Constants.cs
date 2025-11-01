namespace TriangleMesh.ViewModels;

public partial class MainWindowViewModel
{
    // Read-only (constant) fields
    public static int MinTriangulationPrecision => 3;
    public static int MaxTriangulationPrecision => 30;
    public static double MinAlphaAngle => -90;
    public static double MaxAlphaAngle => 90;
    public static double MinBetaAngle => -90;
    public static double MaxBetaAngle => 90;
}