using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPositionCalculator
{
    public Vector3 CalculateRandomPosInRange(Vector3 pivot, float range) => CalculateRandomPosInRange(pivot, range, range);
    public Vector3 CalculateRandomPosInRange(Vector3 pivot, float xRange, float zRange)
    {
        float x = Random.Range(pivot.x - xRange, pivot.x + xRange);
        float z = Random.Range(pivot.z - zRange, pivot.z + zRange);
        Vector3 randomPos = new Vector3(x, pivot.y, z);
        return randomPos;
    }
}
