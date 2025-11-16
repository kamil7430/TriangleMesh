using Avalonia.Media;

namespace TriangleMesh.Models;

public record struct Rgb(double R, double G, double B)
{
    public static Rgb FromColor(Color color)
        => new Rgb(
            color.R / 255.0,
            color.G / 255.0,
            color.B / 255.0
        );

    public static Rgb FromUint(uint color)
        => new Rgb(
            ((color >> 16) & 0xFF) / 255.0,
            ((color >> 8) & 0xFF) / 255.0,
            (color & 0xFF) / 255.0
        );
    
    public uint ToUint()
    {
        var r = R * 255.0;
        var g = G * 255.0;
        var b = B * 255.0;

        uint R_int = (uint)(r > 255.0 ? 255.0 : r);
        uint G_int = (uint)(g > 255.0 ? 255.0 : g);
        uint B_int = (uint)(b > 255.0 ? 255.0 : b);
        
        uint A = 255;
    
        uint packedColor = (A << 24) | (R_int << 16) | (G_int << 8) | B_int;
    
        return packedColor;
    }
}