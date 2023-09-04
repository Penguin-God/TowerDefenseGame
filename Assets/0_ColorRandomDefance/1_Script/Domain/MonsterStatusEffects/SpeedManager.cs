using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager
{
    public float OriginSpeed { get; private set; }
    public float CurrentSpeed { get; private set; }
    public float ApplySlowRate { get; private set; } // 적용된 슬로우
    public bool IsSlow => ApplySlowRate > 0;

    public SpeedManager(float originSpeed) => ChangeOriginSpeed(originSpeed);

    public void ChangeOriginSpeed(float originSpeed)
    {
        OriginSpeed = originSpeed;
        CurrentSpeed = originSpeed;
    }
    public void RestoreSpeed()
    {
        CurrentSpeed = OriginSpeed;
        ApplySlowRate = 0;
    }

    public void OnSlow(float slowRate)
    {
        if (SlowCondition(slowRate))
        {
            CurrentSpeed = CalculateSlowSpeed(slowRate);
            ApplySlowRate = slowRate;
        }
    }
    bool SlowCondition(float slowRate) => slowRate >= ApplySlowRate - 0.1f; // float 오차 때문에 0.1 뺌
    float CalculateSlowSpeed(float slowRate) => OriginSpeed - (OriginSpeed * (slowRate / 100));
}
