using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpeedManager : MonoBehaviour
{
    protected SpeedManager SpeedManager { get; private set; }
    public Action OnRestoreSpeed;
    public void SetSpeed(float originSpeed) => SpeedManager = new SpeedManager(originSpeed);

    public float OnSlow(float slowRate, float slowTime)
    {
        SpeedManager.OnSlow(slowRate);
        ApplySlowTime(slowTime);
        return SpeedManager.CurrentSpeed;
    }

    public virtual float OnSlow(float slowRate, float slowTime, UnitFlags flag) => OnSlow(slowRate, slowTime);

    void ApplySlowTime(float slowTime)
    {
        StopCoroutine(nameof(Co_RestoreSpeed));
        StartCoroutine(nameof(Co_RestoreSpeed), slowTime);
    }

    IEnumerator Co_RestoreSpeed(float slowTime)
    {
        yield return new WaitForSeconds(slowTime);
        SpeedManager.RestoreSpeed();
        OnRestoreSpeed?.Invoke();
    }
}
