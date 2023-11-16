using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class MathUtil
{
    public static float CalculatePercentage(float total, float percent) => (total * percent) / 100f;
    public static int CalculatePercentage(int total, float percent) => Mathf.RoundToInt(CalculatePercentage((float)total, percent));

    public static IReadOnlyList<Vector2> CalculateDirections(int dirCount)
    {
        Vector2[] directions = new Vector2[dirCount];
        float angleStep = 360f / dirCount;

        for (int i = 0; i < dirCount; i++)
        {
            float angle = i * angleStep;
            directions[i] = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        }

        return directions;
    }

    public static IReadOnlyList<Vector2> CalculateCirclePositions(int positionCount, float radius)
    {
        Vector2[] startingPositions = new Vector2[positionCount];
        float angleStep = 360f / positionCount;

        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            startingPositions[i] = new Vector2(x, y);
        }

        return startingPositions;
    }

    public static bool GetRandomBoolByRate(int rate) => rate > Random.Range(0, 101);
}
