using Avalonia.Media.Imaging;
using System.Runtime.CompilerServices;
using TriangleMesh.Models;
using TriangleMesh.ViewModels;
using TriangleMesh.Views.Helpers;

namespace TriangleMesh.Views;

public class MainWindowRenderer
{
    private const uint BEZIER_POLYGON_COLOR = 0xFF00FF00;
    private const uint TRIANGLE_MESH_COLOR = 0xFFFF00FF;
    private const uint LIGHT_DIRECTION_COLOR = 0xFFFFFF00;
    
    private readonly WriteableBitmap _buffer;
    private readonly MainWindowViewModel _viewModel;

    private readonly uint _width;
    private readonly uint _height;

    private readonly uint _blankImageSize;
    private readonly byte[] _blankImage;

    private readonly double[,] _zBuffer;

    public MainWindowRenderer(WriteableBitmap buffer, MainWindowViewModel viewModel)
    {
        _buffer = buffer;
        _viewModel = viewModel;
        _width = (uint)buffer.PixelSize.Width;
        _height = (uint)buffer.PixelSize.Height;
        _blankImageSize = _width * _height * sizeof(uint);
        _blankImage = new byte[_blankImageSize];
        _zBuffer = new double[_width, _height];
    }

    public unsafe void Render()
    {
        var lockedFramebuffer = _buffer.Lock();
        var ptr = (uint*)lockedFramebuffer.Address;

        ClearBitmap(ptr);

        if (_viewModel.IsFilledTrianglesChecked)
            RenderFilledTriangles(ptr);
        
        if (_viewModel.IsTriangleMeshChecked)
            RenderTriangleMesh(ptr);
        
        if (_viewModel.IsBezierPolygonChecked)
            RenderBezierPolygon(ptr);

        if (_viewModel.IsLightDirectionChecked)
            RenderLightDirection(ptr);
        
        lockedFramebuffer.Dispose();
    }

    private unsafe void ClearBitmap(uint* ptr)
    {
        fixed (byte* b = _blankImage)
            Unsafe.CopyBlock(ptr, b, _blankImageSize);
    }

    private unsafe void RenderFilledTriangles(uint* ptr)
    {
        for (int i = 0; i < _zBuffer.GetLength(0); i++)
            for (int j = 0; j < _zBuffer.GetLength(1); j++)
                _zBuffer[i, j] = double.MinValue;

        var filler = new PolygonFiller(
            _viewModel.DistributedComponent,
            _viewModel.SpecularComponent,
            _viewModel.LightColor,
            _viewModel.ObjectTextureType == ObjectTextureType.OneColor ? _viewModel.ObjectColor : null,
            _viewModel.ObjectTextureType == ObjectTextureType.ExternalTexture ? _viewModel.ObjectTexture : null,
            _viewModel.GetLightVector(),
            _viewModel.ReflectionFactor
        );
        
        foreach (var triangle in _viewModel.GetTriangles())
        {
            foreach (var (p, color) in filler.GetPixelsToPaint(triangle, _zBuffer))
                *(ptr + p.Y * _width + p.X) = color;
        }
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

    private unsafe void RenderLightDirection(uint* ptr)
    {
        var vector = _viewModel.GetLightVector().ToVector().ModelToCanvas();
        var pixels = LineDrawer.GetPixelsToPaint([(CoordsTranslator.MiddleOfScreen, vector)]);
        
        foreach (var p in pixels)
            *(ptr + p.Y * _width + p.X) = LIGHT_DIRECTION_COLOR;
    }
}