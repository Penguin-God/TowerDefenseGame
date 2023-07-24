using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SlowData
{
    public float SlowRate { get; private set; }
    public float SlowTime { get; private set; }

    public SlowData(float slowPercent, float slowTime)
    {
        SlowRate = slowPercent;
        SlowTime = slowTime;
    }
}

public class SpeedManager
{
    public float OriginSpeed { get; private set; }
    public float CurrentSpeed { get; private set; }
    
    public bool IsSlow => OriginSpeed > CurrentSpeed;
    public SpeedManager(float originSpeed) => ChangeOriginSpeed(originSpeed);

    public void ChangeOriginSpeed(float originSpeed)
    {
        OriginSpeed = originSpeed;
        CurrentSpeed = originSpeed;
    }
    public void RestoreSpeed() => CurrentSpeed = OriginSpeed;

    public virtual void OnSlow(float slowRate)
    {
        _applySlowRate = slowRate;
        CurrentSpeed = CalculateSlowSpeed(slowRate);
    }
    float _applySlowRate; // 적용된 슬로우
    public bool SlowCondition(float slowRate) => slowRate >= _applySlowRate - 0.1f; // float 오차 때문에 0.1 뺌
    float CalculateSlowSpeed(float slowRate) => OriginSpeed - (OriginSpeed * (slowRate / 100));
}
