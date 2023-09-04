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
    public bool IsSlow => ApplySlowRate > 0;

    bool SlowCondition(float slowRate) => slowRate >= ApplySlowRate - 0.1f; // float 오차 때문에 0.1 뺌

    public void OnSlow(float slowRate)
    {
        if (SlowCondition(slowRate) == false) return;

        StopAllCoroutines();
        SpeedManager.OnSlow(slowRate);
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
