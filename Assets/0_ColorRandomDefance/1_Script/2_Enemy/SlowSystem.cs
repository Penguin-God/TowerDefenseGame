using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSystem
{
    public float SlowPercent { get; private set; }
    public float SlowTime { get; private set; }

    public SlowSystem(float slowPercent, float slowTime)
    {
        SlowPercent = slowPercent;
        SlowTime = slowTime;
    }

    public float ApplySlowToSpeed(float originalSpeed) => originalSpeed - originalSpeed * (SlowPercent / 100);
}
