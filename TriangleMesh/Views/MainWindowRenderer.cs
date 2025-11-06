using Avalonia.Media.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using TriangleMesh.ViewModels;
using TriangleMesh.Views.Helpers;

namespace TriangleMesh.Views;

public class MainWindowRenderer
{
    private readonly WriteableBitmap _buffer;
    private readonly MainWindowViewModel _viewModel;

    private readonly uint _width;
    private readonly uint _height;

    private readonly uint _blankImageSize;
    private readonly byte[] _blankImage;

    public MainWindowRenderer(WriteableBitmap buffer, MainWindowViewModel viewModel)
    {
        _buffer = buffer;
        _viewModel = viewModel;
        _width = (uint)buffer.PixelSize.Width;
        _height = (uint)buffer.PixelSize.Height;
        _blankImageSize = _width * _height * sizeof(uint);
        _blankImage = new byte[_blankImageSize];
    }

    public unsafe void Render()
    {
        if (_viewModel.BezierPolygon == null)
            return;

        var lockedFramebuffer = _buffer.Lock();
        var ptr = (uint*)lockedFramebuffer.Address;

        ClearBitmap(ptr);

        if (_viewModel.IsBezierPolygonChecked)
            RenderBezierPolygon(ptr);

        lockedFramebuffer.Dispose();
    }

    private unsafe void ClearBitmap(uint* ptr)
    {
        fixed (byte* b = _blankImage)
            Unsafe.CopyBlock(ptr, b, _blankImageSize);
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