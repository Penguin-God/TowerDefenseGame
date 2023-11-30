using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowController : MonoBehaviour
{
    Slow _currentInfinitySlow = Slow.InVaildSlow();
    Slow _currentDurationSlow = Slow.InVaildSlow();
    Slow _currentApplySlow = Slow.InVaildSlow();
    float _slowDuration;

    SpeedManager _speedManager;
    public bool IsSlow => _currentApplySlow.IsVaild;
    public event Action OnExitSlow = null;
    public void DependencyInject(SpeedManager speedManager) => _speedManager = speedManager;

    void Update()
    {
        if (_currentApplySlow.IsVaild == false || _currentApplySlow.IsInfinity) return;

        UpdateSlowDuration();
    }

    void UpdateSlowDuration()
    {
        if (_slowDuration > 0)
        {
            _slowDuration -= Time.deltaTime;
            if (_slowDuration <= 0) ExitDurationSlow();
        }
    }

    void ExitCurrentSlow(ref Slow currentSlow, Slow otherSlow)
    {
        currentSlow = Slow.InVaildSlow();
        _speedManager.RestoreSpeed();
        // ���� ���ο� �����ٰ� ���� �ƴ�. ���� ��� �ð� ���ο찡 ������ ���� �ȿ� ������ ���� ���ο찡 ���� ��.
        if (otherSlow.IsVaild) UpdateApplySlow(otherSlow);
        else ExitApplySlow();
    }

    void ExitDurationSlow()
    {
        _slowDuration = 0;
        ExitCurrentSlow(ref _currentDurationSlow, _currentInfinitySlow);
    }

    public void ExitInfinitySlow() => ExitCurrentSlow(ref _currentInfinitySlow, _currentDurationSlow);


    public void ApplyNewSlow(Slow slow)
    {
        if (slow.IsInfinity) 
            UpdateCurrentSlow(ref _currentInfinitySlow, slow);
        else if(UpdateCurrentSlow(ref _currentDurationSlow, slow))
            _slowDuration = slow.Duration;
    }

    bool UpdateCurrentSlow(ref Slow currentSlow, Slow newSlow)
    {
        if (newSlow.Intensity >= currentSlow.Intensity)
        {
            currentSlow = newSlow;
            if (currentSlow.Intensity > _currentApplySlow.Intensity)
                UpdateApplySlow(currentSlow);
            return true;
        }
        else return false;
    }

    void UpdateApplySlow(Slow slow)
    {
        if (slow.IsVaild == false)
        {
            Debug.LogError("�� ��ȿ���� ���� ���ο츦 �ذž�");
            return;
        }

        _speedManager.RestoreSpeed();
        _currentApplySlow = slow;
        _speedManager.OnSlow(_currentApplySlow.Intensity);
    }

    void ExitApplySlow()
    {
        _currentApplySlow = Slow.InVaildSlow();
        OnExitSlow?.Invoke();
    }

    void OnEnable()
    {
        _currentInfinitySlow = Slow.InVaildSlow();
        _currentDurationSlow = Slow.InVaildSlow();
        _currentApplySlow = Slow.InVaildSlow();
        _slowDuration = 0;
    }
}
