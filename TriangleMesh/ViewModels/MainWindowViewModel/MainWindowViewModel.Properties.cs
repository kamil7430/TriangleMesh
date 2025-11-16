using Avalonia.Media;
using Avalonia.Media.Imaging;
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

    [ObservableProperty] 
    private double _currentLightAngle;

    // Check boxes
    [ObservableProperty] 
    private bool _isBezierPolygonChecked;
    
    [ObservableProperty] 
    private bool _isTriangleMeshChecked;
    
    [ObservableProperty] 
    private bool _isFilledTrianglesChecked;
    
    [ObservableProperty]
    private bool _isLightDirectionChecked;

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
    
    private WriteableBitmap? _objectTexture;
    public WriteableBitmap ObjectTexture
    {
        get
        {
            if (_objectTexture == null)
                RestoreDefaultTexture();
            return _objectTexture!;
        }
        set => SetProperty(ref _objectTexture, value);
    }
    
    [ObservableProperty]
    private double _zLightAnimationPosition;
    
    [ObservableProperty]
    private bool _isLightAnimationStopped = true;

    [ObservableProperty] 
    private int _reflectionFactor = MinReflectionFactor;

    [ObservableProperty] 
    private bool _isNormalVectorsMapChecked = false;
    
    private WriteableBitmap? _normalVectorsMap;
    public WriteableBitmap NormalVectorsMap
    {
        get
        {
            if (_normalVectorsMap == null)
                RestoreDefaultNormalVectorsMap();
            return _normalVectorsMap!;
        }
        set => SetProperty(ref _normalVectorsMap, value);
    }
}