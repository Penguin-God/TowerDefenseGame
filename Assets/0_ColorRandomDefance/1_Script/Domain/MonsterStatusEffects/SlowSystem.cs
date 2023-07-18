using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SlowData
{
    public float SlowPercent { get; private set; }
    public float SlowTime { get; private set; }

    public SlowData(float slowPercent, float slowTime)
    {
        SlowPercent = slowPercent;
        SlowTime = slowTime;
    }
}

public class SpeedManager
{
    public readonly float OriginSpeed;
    public float CurrentSpeed { get; private set; }
    public bool IsSlow => OriginSpeed > CurrentSpeed;
    public SpeedManager(float originSpeed)
    {
        OriginSpeed = originSpeed;
        CurrentSpeed = originSpeed;
    }

    public void OnSlow(float slowRate) => CurrentSpeed = OriginSpeed - (OriginSpeed * (slowRate / 100));
    public void RestoreSpeed() => CurrentSpeed = OriginSpeed;
}
