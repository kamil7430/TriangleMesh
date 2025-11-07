using Avalonia.Media.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using TriangleMesh.ViewModels;
using TriangleMesh.Views.Helpers;

namespace TriangleMesh.Views;

public class MainWindowRenderer
{
    private const uint BEZIER_POLYGON_COLOR = 0xFF00FF00;
    private const uint TRIANGLE_MESH_COLOR = 0xFFFF00FF;
    
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
        var lockedFramebuffer = _buffer.Lock();
        var ptr = (uint*)lockedFramebuffer.Address;

        ClearBitmap(ptr);

        if (_viewModel.IsTriangleMeshChecked)
            RenderTriangleMesh(ptr);
        
        if (_viewModel.IsBezierPolygonChecked)
            RenderBezierPolygon(ptr);

        lockedFramebuffer.Dispose();
    }

    private unsafe void ClearBitmap(uint* ptr)
    {
        fixed (byte* b = _blankImage)
            Unsafe.CopyBlock(ptr, b, _blankImageSize);
    }

    private unsafe void RenderTriangleMesh(uint* ptr)
    {
        var pixels = LineDrawer.GetPixelsToPaint(_viewModel.GetTriangleMeshEdges());
        
        foreach (var p in pixels)
            *(ptr + p.Y * _width + p.X) = TRIANGLE_MESH_COLOR;
    }
    
    private unsafe void RenderBezierPolygon(uint* ptr)
    {
        var pixels = LineDrawer.GetPixelsToPaint(_viewModel.GetBezierPolygonEdges());

        foreach (var p in pixels)
            *(ptr + p.Y * _width + p.X) = BEZIER_POLYGON_COLOR;
    }
}