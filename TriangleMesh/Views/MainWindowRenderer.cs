using Avalonia.Media.Imaging;
using TriangleMesh.ViewModels;
using TriangleMesh.Views.Helpers;

namespace TriangleMesh.Views;

public class MainWindowRenderer
{
    private readonly WriteableBitmap _buffer;
    private readonly MainWindowViewModel _viewModel;

    private readonly int _width;
    private readonly int _height;

    public MainWindowRenderer(WriteableBitmap buffer, MainWindowViewModel viewModel)
    {
        _buffer = buffer;
        _viewModel = viewModel;
        _width = buffer.PixelSize.Width;
        _height = buffer.PixelSize.Height;
    }

    public unsafe void Render()
    {
        using var lockedFramebuffer = _buffer.Lock();
        var ptr = (uint*)lockedFramebuffer.Address;

        if (_viewModel.IsBezierPolygonChecked)
            RenderBezierPolygon(ptr);
    }

    private unsafe void RenderBezierPolygon(uint* ptr)
    {
        if (_viewModel.BezierPolygon == null)
            return;

        _viewModel.BezierPolygon.ControlPoints
        var pixels = LineDrawer.GetPixelsToPaint()

        uint greenColor = 0xFF00FF00;

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                *(ptr + y * _width + x) = greenColor;
            }
        }
    }
}