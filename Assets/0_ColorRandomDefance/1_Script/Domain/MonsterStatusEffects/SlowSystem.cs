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
    readonly SlowData SlowData;
    public float SlowPercent => SlowData.SlowPercent;
    public float SlowTime => SlowData.SlowTime;

    public SlowSystem(float slowPercent, float slowTime) => SlowData = new SlowData(slowPercent, slowTime);

    public float ApplySlowToSpeed(float originalSpeed) => originalSpeed - originalSpeed * (SlowPercent / 100);
}
