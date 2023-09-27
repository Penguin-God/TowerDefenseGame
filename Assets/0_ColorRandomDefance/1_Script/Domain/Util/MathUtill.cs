using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class MathUtil
{
    public static float CalculatePercentage(float total, float percent) => (total * percent) / 100f;

    public static int CalculatePercentage(int total, float percent) => Mathf.RoundToInt(CalculatePercentage((float)total, percent));
}
