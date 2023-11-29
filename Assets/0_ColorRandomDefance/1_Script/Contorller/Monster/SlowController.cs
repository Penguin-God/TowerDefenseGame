using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowController : MonoBehaviour
{
    Slow _currentInfinitySlow = Slow.InVaildSlow();
    Slow _currentApplySlow = Slow.InVaildSlow();
    float _slowDuration;

    SpeedManager _speedManager;
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
            if (_slowDuration <= 0)
            {
                _slowDuration = 0;
                _speedManager.RestoreSpeed();
                if (_currentInfinitySlow.IsVaild) _speedManager.OnSlow(_currentInfinitySlow.Intensity);
                else ExitCurrentSlow();
            }
        }
    }

    public void ApplyNewSlow(Slow slow)
    {
        if (slow.IsInfinity)
        {
            if (NewSlowIsStrong(_currentInfinitySlow))
            {
                _currentInfinitySlow = slow;
                if(_currentInfinitySlow.Intensity > _currentApplySlow.Intensity)
                    UpdateApplySlow(_currentInfinitySlow);
            }
        }
        else if(_currentApplySlow.IsInfinity == false || _currentApplySlow.IsVaild == false)
        {
            if (NewSlowIsStrong(_currentApplySlow))
                UpdateApplySlow(slow);
        }

        bool NewSlowIsStrong(Slow currentSlow) => slow.Intensity > currentSlow.Intensity;
    }

    void UpdateApplySlow(Slow slow)
    {
        _speedManager.RestoreSpeed();
        _currentApplySlow = slow;
        _slowDuration = _currentApplySlow.Duration;
        _speedManager.OnSlow(_currentApplySlow.Intensity);
    }

    public void ExitCurrentSlow()
    {
        
    }
}
