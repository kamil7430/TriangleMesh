using System.Collections.Generic;

namespace TriangleMesh.Models;

public class BezierPolygon
{
    public const int DIMENSION_SIZE = 4;

    public ControlPoint[,] ControlPoints { get; }
    public BezierPolygon? FirstDimensionDerivative { get; }
    public BezierPolygon? SecondDimensionDerivative { get; }

    public BezierPolygon(ControlPoint[,] controlPoints)
    {
        ControlPoints = controlPoints;
    }

    public BezierPolygon(string pointsFile)
    {
        var pointsLines = pointsFile.Split('\n');

        ControlPoints = new ControlPoint[DIMENSION_SIZE, DIMENSION_SIZE];

        // Control points
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

        // First dimension derivative
        var fdcp = new ControlPoint[DIMENSION_SIZE - 1, DIMENSION_SIZE];
        for (int i = 0; i < DIMENSION_SIZE - 1; i++)
        {
            for (int j = 0; j < DIMENSION_SIZE; j++)
            {
                var vector = ControlPoints[i + 1, j].P - ControlPoints[i, j].P;
                vector *= DIMENSION_SIZE - 1;
                fdcp[i, j] = new ControlPoint(vector);
            }
        }
        FirstDimensionDerivative = new BezierPolygon(fdcp);
        
        // Second dimension derivative
        var sdcp = new ControlPoint[DIMENSION_SIZE, DIMENSION_SIZE - 1];
        for (int i = 0; i < DIMENSION_SIZE; i++)
        {
            for (int j = 0; j < DIMENSION_SIZE - 1; j++)
            {
                var vector = ControlPoints[i, j + 1].P - ControlPoints[i, j].P;
                vector *= DIMENSION_SIZE - 1;
                sdcp[i, j] = new ControlPoint(vector);
            }
        }
        SecondDimensionDerivative = new BezierPolygon(sdcp);
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