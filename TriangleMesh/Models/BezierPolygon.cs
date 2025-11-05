using Avalonia;

namespace TriangleMesh.Models;

public class BezierPolygon
{
    private const int DIMENSION_SIZE = 4;

    public Vector3D[,] ControlPoints { get; private set; }

    public BezierPolygon(Vector3D[,] controlPoints)
    {
        ControlPoints = controlPoints;
    }

    public BezierPolygon(string pointsFile)
    {
        var pointsLines = pointsFile.Split('\n');

        ControlPoints = new Vector3D[DIMENSION_SIZE, DIMENSION_SIZE];

        for (int i = 0; i < DIMENSION_SIZE; i++)
        {
            for (int j = 0; j < DIMENSION_SIZE; j++)
            {
                var point = pointsLines[i * DIMENSION_SIZE + j].Split(' ');
                var x = double.Parse(point[0]);
                var y = double.Parse(point[1]);
                var z = double.Parse(point[2]);
                ControlPoints[i, j] = new Vector3D(x, y, z);
            }
        }
    }
}