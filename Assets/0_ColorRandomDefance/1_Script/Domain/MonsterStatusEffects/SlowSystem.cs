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
    float _applySlowRate; // ����� ���ο�
    public bool SlowCondition(float slowRate) => slowRate >= _applySlowRate - 0.1f; // float ���� ������ 0.1 ��
    float CalculateSlowSpeed(float slowRate) => OriginSpeed - (OriginSpeed * (slowRate / 100));
}
