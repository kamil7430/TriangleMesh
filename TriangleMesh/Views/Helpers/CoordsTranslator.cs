using Avalonia;

namespace TriangleMesh.Views.Helpers;

public static class CoordsTranslator
{
    public const int DRAWING_AREA_HEIGHT = 600;
    public const int DRAWING_AREA_WIDTH = 800;

    public static Vector ModelToCanvas(this Vector v)
        => new Vector(v.X + DRAWING_AREA_WIDTH / 2, v.Y + DRAWING_AREA_HEIGHT / 2);

    public static Vector CanvasToModel(this Vector v)
        => new Vector(v.X - DRAWING_AREA_WIDTH / 2, v.Y - DRAWING_AREA_HEIGHT / 2);
}
