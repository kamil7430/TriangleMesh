using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System.ComponentModel;
using TriangleMesh.Services;
using TriangleMesh.ViewModels;
using TriangleMesh.Views.Helpers;

namespace TriangleMesh.Views;

public partial class MainWindow : Window, IMessageBoxShower
{
    private readonly PixelSize DRAWING_AREA_SIZE
        = new PixelSize(CoordsTranslator.DRAWING_AREA_WIDTH, CoordsTranslator.DRAWING_AREA_HEIGHT);
    private readonly Vector DPI_VECTOR = new Vector(96, 96);

    private readonly MainWindowViewModel _viewModel;
    private readonly WriteableBitmap _drawingAreaBuffer;
    private readonly MainWindowRenderer _renderer;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _viewModel = new MainWindowViewModel(this);
        Loaded += MainWindow_OnLoaded;

        _drawingAreaBuffer = new WriteableBitmap(DRAWING_AREA_SIZE, DPI_VECTOR);
        DrawingArea.Source = _drawingAreaBuffer;
        _viewModel.PropertyChanged += ViewModel_OnPropertyChanged;

        _renderer = new MainWindowRenderer(_drawingAreaBuffer, _viewModel);
        RenderDrawingArea();
    }

    private void MainWindow_OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        => _viewModel.OnMainWindowLoaded();

    private void ViewModel_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        => RenderDrawingArea();

    private void RenderDrawingArea()
    {
        _renderer.Render();
        DrawingArea.InvalidateVisual();
    }
}