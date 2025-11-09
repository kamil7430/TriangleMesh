using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using TriangleMesh.Models;
using TriangleMesh.Models.Helpers;

namespace TriangleMesh.ViewModels;

public partial class MainWindowViewModel
{
    // Model
    [ObservableProperty] 
    private BezierPolygon _bezierPolygon;
    
    [ObservableProperty] 
    private Mesh _mesh;

    // Check boxes
    [ObservableProperty] 
    private bool _isBezierPolygonChecked;
    
    [ObservableProperty] 
    private bool _isTriangleMeshChecked;
    
    [ObservableProperty] 
    private bool _isFilledTrianglesChecked;

    // Sliders
    private int _triangulationPrecision = MinTriangulationPrecision;
    public int TriangulationPrecision
    {
        get => _triangulationPrecision;
        set
        {
            Mesh = new Mesh(value, BezierPolygon);
            SetProperty(ref _triangulationPrecision, value);
        }
    }

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

    [ObservableProperty] 
    private double _distributedComponent;
    
    [ObservableProperty]
    private double _specularComponent;

    [ObservableProperty] 
    private Color _lightColor = Colors.White;

    [ObservableProperty] 
    private ObjectTextureType _objectTextureType = ObjectTextureType.OneColor;
    
    [ObservableProperty]
    private Color _objectColor = Colors.White;

    [ObservableProperty] 
    private int _reflectionFactor = MinReflectionFactor;
}