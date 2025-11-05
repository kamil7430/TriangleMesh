using CommunityToolkit.Mvvm.ComponentModel;
using TriangleMesh.Models;

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
    [ObservableProperty] private double _alphaAngle;
    [ObservableProperty] private double _betaAngle;
}