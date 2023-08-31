using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpeedManager : MonoBehaviour
{
    public SpeedManager SpeedManager { get; private set; }
    public Action OnRestoreSpeed;
    public void SetSpeed(float originSpeed) => SpeedManager = new SpeedManager(originSpeed);

    public float ApplySlowRate { get; private set; } // ����� ���ο�
    public bool IsSlow => ApplySlowRate > 0;

    bool SlowCondition(float slowRate) => slowRate >= ApplySlowRate - 0.1f; // float ���� ������ 0.1 ��

    public void OnSlow(float slowRate)
    {
        if (SlowCondition(slowRate) == false || gameObject == null) return;
        StopAllCoroutines();
        SpeedManager.OnSlow(slowRate);
        ApplySlowRate = slowRate;
    }

    public virtual void OnSlowWithTime(float slowRate, float slowTime, UnitFlags flag)
    {
        OnSlow(slowRate);
        StartCoroutine(nameof(Co_RestoreSpeed), slowTime);
    }

    IEnumerator Co_RestoreSpeed(float slowTime)
    {
        yield return new WaitForSeconds(slowTime);
        RestoreSpeed();
    }

    public void RestoreSpeed()
    {
        ApplySlowRate = 0;
        SpeedManager.RestoreSpeed();
        OnRestoreSpeed?.Invoke();
    }

    void OnDisable() => StopAllCoroutines();
}
