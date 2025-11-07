using System.Collections.Generic;
using Avalonia;

namespace TriangleMesh.Models;

public class BezierPolygon
{
    public const int DIMENSION_SIZE = 4;

    public ControlPoint[,] ControlPoints { get; private set; }

    public BezierPolygon(ControlPoint[,] controlPoints)
    {
        ControlPoints = controlPoints;
    }

    public BezierPolygon(string pointsFile)
    {
        var pointsLines = pointsFile.Split('\n');

        ControlPoints = new ControlPoint[DIMENSION_SIZE, DIMENSION_SIZE];

        for (int i = 0; i < DIMENSION_SIZE; i++)
        {
            for (int j = 0; j < DIMENSION_SIZE; j++)
            {
                var point = pointsLines[i * DIMENSION_SIZE + j].Split(' ');
                var x = double.Parse(point[0]);
                var y = double.Parse(point[1]);
                var z = double.Parse(point[2]);
                ControlPoints[i, j] = new ControlPoint(x, y, z);
            }
        }
    }

    public IEnumerable<(ControlPoint Cp1, ControlPoint Cp2)> GetEdges()
    {
        for (int i = 0; i < DIMENSION_SIZE; i++)
        {
            for (int j = 0; j < DIMENSION_SIZE; j++)
            {
                if (i + 1 < DIMENSION_SIZE)
                    yield return (ControlPoints[i, j], ControlPoints[i + 1, j]);
                if (j + 1 < DIMENSION_SIZE)
                    yield return (ControlPoints[i, j], ControlPoints[i, j + 1]);
            }
        }
    }
}