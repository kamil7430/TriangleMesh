using System;

namespace TriangleMesh.ViewModels;

public partial class MainWindowViewModel
{
    // Read-only (constant) fields
    public static double LightRotationAngleSpeedInRadiansPerTick => 1 / (2 * Math.PI);
    public static double LightVectorVisualLength = 200;
    public static int MinTriangulationPrecision => 3;
    public static int MaxTriangulationPrecision => 30;
    public static double MinAlphaAngle => -90;
    public static double MaxAlphaAngle => 90;
    public static double MinBetaAngle => -90;
    public static double MaxBetaAngle => 90;
    public static double MinComponentValue => 0;
    public static double MaxComponentValue => 1;
    public static double MinZLightAnimationPosition => -200;
    public static double MaxZLightAnimationPosition => 200;
    public static int MinReflectionFactor => 1;
    public static int MaxReflectionFactor => 100;
}