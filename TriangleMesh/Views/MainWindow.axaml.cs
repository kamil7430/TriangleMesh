using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System.ComponentModel;
using TriangleMesh.Services;
using TriangleMesh.ViewModels;

namespace TriangleMesh.Views;

public partial class MainWindow : Window, IMessageBoxShower
{
    private readonly PixelSize DRAWING_AREA_SIZE = new PixelSize(800, 600);
    private readonly Vector DPI_VECTOR = new Vector(96, 96);

    private readonly MainWindowViewModel _viewModel;
    private readonly WriteableBitmap _drawingAreaBuffer;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _viewModel = new MainWindowViewModel(this);
        Loaded += MainWindow_OnLoaded;

        _drawingAreaBuffer = new WriteableBitmap(DRAWING_AREA_SIZE, DPI_VECTOR);
        DrawingArea.Source = _drawingAreaBuffer;
        _viewModel.PropertyChanged += ViewModel_OnPropertyChanged;
        RenderDrawingArea();
    }

    private void MainWindow_OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        => _viewModel.LoadBezierPolygon("BezierPoints.txt");

    private void ViewModel_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        => RenderDrawingArea();

    private unsafe void RenderDrawingArea()
    {
        using (var lockedFramebuffer = _drawingAreaBuffer.Lock())
        {
            var ptr = (uint*)lockedFramebuffer.Address;

            int width = _drawingAreaBuffer.PixelSize.Width;
            int height = _drawingAreaBuffer.PixelSize.Height;

            uint greenColor = 0xFF00FF00;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    *(ptr + y * width + x) = greenColor;
                }
            }
        }

        DrawingArea.InvalidateVisual();
    }
}