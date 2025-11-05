using Avalonia;
using System;
using System.Collections.Generic;

namespace TriangleMesh.Views.Helpers;

public static class LineDrawer
{
    public static IEnumerable<Vector> GetPixelsToPaint(IEnumerable<(Vector, Vector)> linesToPaint)
    {
        foreach (var line in linesToPaint)
        {
            var (v1, v2) = (line.Item1, line.Item2);

            // Obsłużenie upierdliwego przypadku (tan zbiega do nieskończoności)
            if (Math.Abs(v1.X - v2.X) < 1)
            {
                if (v1.Y > v2.Y)
                    (v1, v2) = (v2, v1);
                for (int y = (int)v1.Y; y < v2.Y; y++)
                    yield return new Vector(v1.X, y);
            }

            // Zapewnienie sobie krawędzi "w prawo"
            if (v1.X > v2.X)
                (v1, v2) = (v2, v1);

            // Obliczenie nachylenia prostej i wybór odpowiedniej transformacji pod algorytm
            var tan = (v2.Y - v1.Y) / (v2.X - v1.X);
            if (tan > 1)
            {
                // Zamiana osi
                var pixels = BresenhamAlgorithm((int)v1.Y, (int)v1.X, (int)v2.Y, (int)v2.X);
                foreach (var p in pixels)
                    yield return new Vector(p.Y, p.X);
            }
            else if (tan > 0)
            {
                // Brak transformacji - przypadek bazowy
                var pixels = BresenhamAlgorithm((int)v1.X, (int)v1.Y, (int)v2.X, (int)v2.Y);
                foreach (var p in pixels)
                    yield return new Vector(p.X, p.Y);
            }
            else if (tan > -1)
            {
                // Odbicie względem osi OX
                var pixels = BresenhamAlgorithm((int)v1.X, (int)-v1.Y, (int)v2.X, (int)-v2.Y);
                foreach (var p in pixels)
                    yield return new Vector(p.X, -p.Y);
            }
            else
            {
                // Zamiana osi i odbicie względem OX
                var pixels = BresenhamAlgorithm((int)v2.Y, (int)-v2.X, (int)v1.Y, (int)-v1.X);
                foreach (var p in pixels)
                    yield return new Vector(-p.Y, p.X);
            }
        }
    }

    private static List<Vector> BresenhamAlgorithm(int x1, int y1, int x2, int y2)
    {
        int dx = x2 - x1;
        int dy = y2 - y1;
        int d = 2 * dy - dx;
        int incrE = 2 * dy;
        int incrNE = 2 * (dy - dx);
        int x = x1;
        int y = y1;
        List<Vector> pixels = [new Vector(x, y)];

        while (x < x2)
        {
            if (d < 0)
            {
                d += incrE;
                x++;
            }
            else
            {
                d += incrNE;
                x++;
                y++;
            }

            pixels.Add(new Vector(x, y));
        }

        return pixels;
    }
}