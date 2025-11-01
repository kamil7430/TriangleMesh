namespace TriangleMesh.ViewModels;

public partial class MainWindowViewModel
{
    // Read-only (constant) fields
    public int MinTriangulationPrecision { get; } = 3;
    public int MaxTriangulationPrecision { get; } = 30;
    public double MinAlphaAngle { get; } = -90;
    public double MaxAlphaAngle { get; } = 90;
    public double MinBetaAngle { get; } = -90;
    public double MaxBetaAngle { get; } = 90;
}