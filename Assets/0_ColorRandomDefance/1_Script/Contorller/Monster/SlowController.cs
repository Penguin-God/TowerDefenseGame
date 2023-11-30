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
        // 현재 슬로우 나간다고 끝이 아님. 예를 들어 시간 슬로우가 끝나도 장판 안에 있으면 범위 슬로우가 들어가야 함.
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
            Debug.LogError("왜 유효하지 않은 슬로우를 준거야");
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
