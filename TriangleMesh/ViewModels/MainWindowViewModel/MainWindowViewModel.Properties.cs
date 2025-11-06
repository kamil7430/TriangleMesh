using CommunityToolkit.Mvvm.ComponentModel;
using TriangleMesh.Models;
using TriangleMesh.Models.Helpers;

namespace TriangleMesh.ViewModels;

public partial class MainWindowViewModel
{
    // Model
    [ObservableProperty] private BezierPolygon? _bezierPolygon;

    // Check boxes
    [ObservableProperty] private bool _isBezierPolygonChecked;
    
    [ObservableProperty] private bool _isTriangleMeshChecked;
    
    [ObservableProperty] private bool _isFilledTrianglesChecked;

    // Sliders
    [ObservableProperty] private int _triangulationPrecision;
    
    private double _alphaAngle;
    public double AlphaAngle
    {
        get => _alphaAngle;
        set
        {
            RotationHelper.Alpha = value;
            SetProperty(ref _alphaAngle, value);
        }
    }
    
    private double _betaAngle;

    public double BetaAngle
    {
        get => _betaAngle;
        set
        {
            RotationHelper.Beta = value;
            SetProperty(ref _betaAngle, value);
        }
    }
}