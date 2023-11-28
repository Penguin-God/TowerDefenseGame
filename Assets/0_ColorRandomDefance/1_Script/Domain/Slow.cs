using System.Collections;
using System.Collections.Generic;

public class Slow
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

    public static Slow CreateDurationSlow(float intensity, float duration) => new Slow(intensity, duration, false);
    public static Slow CreateInfinitySlow(float intensity) => new Slow(intensity, float.PositiveInfinity, true);
}
