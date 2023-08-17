using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager
{
    public float OriginSpeed { get; private set; }
    public float CurrentSpeed { get; private set; }
    public SpeedManager(float originSpeed) => ChangeOriginSpeed(originSpeed);

    public void ChangeOriginSpeed(float originSpeed)
    {
        OriginSpeed = originSpeed;
        CurrentSpeed = originSpeed;
    }
    public void RestoreSpeed() => CurrentSpeed = OriginSpeed;

    public void OnSlow(float slowRate) => CurrentSpeed = CalculateSlowSpeed(slowRate);
    float CalculateSlowSpeed(float slowRate) => OriginSpeed - (OriginSpeed * (slowRate / 100));
}
