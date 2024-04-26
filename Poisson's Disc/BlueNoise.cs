using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueNoise
{

    public static List<Vector3> GeneratePoints(float radius, float width, float height, int maxItr, Vector3 src)
    {
        float cellSize = radius/Mathf.Sqrt(radius);
        int[,] grid = new int[Mathf.CeilToInt(width / cellSize), Mathf.CeilToInt(height / cellSize)];
        List<Vector3> activeSamples = new List<Vector3>();
        List<Vector3> points = new List<Vector3>();
        Vector3 gridCenter = src + new Vector3(width / 2, 0, height / 2);
        activeSamples.Add(gridCenter);
        while (activeSamples.Count != 0)
        {
            int stagePointIndex = Random.Range(0, activeSamples.Count);
            Vector3 stagedPoint = activeSamples[stagePointIndex];
            bool isValid = false;

            for (int i = 0; i < maxItr; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                float randRad = Random.Range(radius, 2 * radius);
                Vector3 dir = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Vector3 newPoint = stagedPoint + dir;

                if (IsValid(newPoint, grid ,points, src , width, height, radius , cellSize))
                {
                    activeSamples.Add(newPoint);
                    points.Add(newPoint);
                    grid[(int)((newPoint.x - src.x) / cellSize), (int)((newPoint.z - src.z) / cellSize)] = points.Count;
                    isValid = true;
                    break;
                }
            }

            if (!isValid)
            {
                activeSamples.RemoveAt(stagePointIndex);
            }
        }


        return points;
        
    }


    private static bool IsValid(Vector3 newPos, int[,] grid ,List<Vector3> points, Vector3 src ,float width, float height, float rad, float cellSize)
    {
        if(src.x < newPos.x && src.z < newPos.z && (src.x + width) > newPos.x && (src.z + height) > newPos.z)
        {
            /*
             * AN INEFFICIENT APPROACH
             * foreach (var i in points)
            { 
                float dst = (i - newPos).magnitude;
                if (dst <= rad)
                {
                    return false;
                }
            }
             */

            int cellX = (int)((newPos.x - src.x)/ cellSize);
            int cellY = (int)((newPos.z - src.z)/ cellSize);
            //Debug.Log(cellX + " " + cellY);
            int startSearchX = Mathf.Max(0, cellX - 1);
            int startSearchY = Mathf.Max(0, cellY - 1);

            int endSearchX = Mathf.Min(cellX + 1 , grid.GetLength(0) - 1);
            int endSearchY = Mathf.Min(cellY + 1 , grid.GetLength(1) - 1);


            for (int i = startSearchX; i <= endSearchX; i++)
            {
                for (int j = startSearchY; j <= endSearchY; j++)
                { 
                    int val = grid[i, j] -1;
                    if (val != -1)
                    {
                        float dst = (points[val] - newPos).magnitude;
                        if (dst < rad)
                        {
                            return false;
                        }
                    }
                }
            }


            return true;
        }
        return false;
    }
}
