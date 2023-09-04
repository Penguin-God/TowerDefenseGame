using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpeedSystem : MonoBehaviour
{
    public SpeedManager SpeedManager { get; private set; }
    public Action OnRestoreSpeed;
    public void SetSpeed(float originSpeed) => SpeedManager = new SpeedManager(originSpeed);
    public void ReceiveInject(SpeedManager speedManager) => SpeedManager = speedManager;

    public float ApplySlowRate => SpeedManager.ApplySlowRate;
    public bool IsSlow => SpeedManager.IsSlow;

    public void OnSlow(float slowRate)
    {
        if (SpeedManager.OnSlow(slowRate))
            StopAllCoroutines();
    }

    public virtual void OnSlowWithTime(float slowRate, float slowTime, UnitFlags flag)
    {
        SpeedManager.OnSlow(slowRate, flag);
        if(gameObject.activeSelf)
            StartCoroutine(nameof(Co_RestoreSpeed), slowTime);
    }

    IEnumerator Co_RestoreSpeed(float slowTime)
    {
        yield return new WaitForSeconds(slowTime);
        RestoreSpeed();
    }

    public void RestoreSpeed()
    {
        SpeedManager.RestoreSpeed();
        OnRestoreSpeed?.Invoke();
    }

    void OnDisable() => StopAllCoroutines();
}
