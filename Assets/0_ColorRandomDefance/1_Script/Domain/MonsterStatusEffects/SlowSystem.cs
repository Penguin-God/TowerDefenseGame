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

public class SlowSystem
{
    public readonly SlowData SlowData;
    public float SlowPercent => SlowData.SlowPercent;
    public float SlowTime => SlowData.SlowTime;

    public SlowSystem(float slowPercent, float slowTime) => SlowData = new SlowData(slowPercent, slowTime);

    public float ApplySlowToSpeed(float originalSpeed) => originalSpeed - originalSpeed * (SlowPercent / 100);
}

public class SpeedManager
{
    public readonly float OriginSpeed;
    public float CurrentSpeed { get; private set; }
    public SpeedManager(float originSpeed)
    {
        OriginSpeed = originSpeed;
        CurrentSpeed = originSpeed;
    }

    public void OnSlow(float slowRate) => CurrentSpeed = OriginSpeed - (OriginSpeed * (slowRate / 100));
    public void RestoreSpeed() => CurrentSpeed = OriginSpeed;
}
