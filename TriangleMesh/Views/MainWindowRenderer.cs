using System.Linq;
using Avalonia;
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
        var lockedFramebuffer = _buffer.Lock();
        var ptr = (uint*)lockedFramebuffer.Address;

        if (_viewModel.IsBezierPolygonChecked)
            RenderBezierPolygon(ptr);
        
        lockedFramebuffer.Dispose();
    }

    private unsafe void RenderBezierPolygon(uint* ptr)
    {
        if (_viewModel.BezierPolygon == null)
            return;
        
        var pixels = LineDrawer.GetPixelsToPaint(_viewModel.BezierPolygon.GetEdges()
            .Select(t => (t.Cp1.PostRotationP.ToVector().ModelToCanvas(),
                t.Cp2.PostRotationP.ToVector().ModelToCanvas())));

        uint greenColor = 0xFF00FF00;

        foreach (var p in pixels)
            *(ptr + p.Y * _width + p.X) = greenColor;
    }
}