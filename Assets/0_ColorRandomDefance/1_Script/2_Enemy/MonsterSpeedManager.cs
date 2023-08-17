using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpeedManager : MonoBehaviour
{
    public SpeedManager SpeedManager { get; private set; }
    public Action OnRestoreSpeed;
    public void SetSpeed(float originSpeed) => SpeedManager = new SpeedManager(originSpeed);

    public void ApplySlow(float slowRate) => SpeedManager.OnSlow(slowRate);
    public void OnSlowWithTime(float slowRate, float slowTime)
    {
        ApplySlow(slowRate);
        ApplySlowTime(slowTime);
    }

    public virtual void OnSlowWithTime(float slowRate, float slowTime, UnitFlags flag) => OnSlowWithTime(slowRate, slowTime);
    void ApplySlowTime(float slowTime)
    {
        StopCoroutine(nameof(Co_RestoreSpeed));
        StartCoroutine(nameof(Co_RestoreSpeed), slowTime);
    }
    public void RestoreSpeed()
    {
        SpeedManager.RestoreSpeed();
        OnRestoreSpeed?.Invoke();
    }
    IEnumerator Co_RestoreSpeed(float slowTime)
    {
        yield return new WaitForSeconds(slowTime);
        RestoreSpeed();
    }

    void OnDisable() => StopAllCoroutines();
}
