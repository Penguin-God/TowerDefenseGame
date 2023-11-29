using System.Collections;
using System.Collections.Generic;

public readonly struct Slow
{
    public readonly float Intensity;
    public readonly float Duration;
    public readonly bool IsInfinity;

    Slow(float intensity, float duration, bool isInfinity)
    {
        Intensity = intensity;
        Duration = duration;
        IsInfinity = isInfinity;
    }

    public bool IsVaild => Intensity > 0;

    public static Slow CreateDurationSlow(float intensity, float duration) => new(intensity, duration, false);
    public static Slow CreateInfinitySlow(float intensity) => new(intensity, float.PositiveInfinity, true);
    public static Slow InVaildSlow() => new(-1, -1, false);
}
