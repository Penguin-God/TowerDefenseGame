using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowController : MonoBehaviour
{
    Slow _currentInfinitySlow;
    Slow _currentDurationSlow;
    float _slowDuration;

    void Update()
    {
        if (_currentInfinitySlow != null) return;

        UpdateDurationSlow();
    }

    void UpdateDurationSlow()
    {
        if (_currentDurationSlow != null && _slowDuration > 0)
        {
            _slowDuration -= Time.deltaTime;
            if (_slowDuration <= 0)
            {
                _currentDurationSlow = null;
                _slowDuration = 0;
                // slow exit
                if (_currentInfinitySlow != null)
                {
                    // slow
                }
            }
        }
    }

    void ApplySlow()
    {

    }

    public void ApplyNewSlow(Slow slow)
    {
        if (slow.IsInfinity)
        {
            if (NewSlowIsStrong(_currentInfinitySlow))
                _currentInfinitySlow = Slow.CreateInfinitySlow(slow.Intensity);
        }
        else
        {
            if (NewSlowIsStrong(_currentDurationSlow))
                _currentDurationSlow = Slow.CreateDurationSlow(slow.Intensity, slow.Duration);
        }

        bool NewSlowIsStrong(Slow currentSlow) => currentSlow == null || slow.Intensity > currentSlow.Intensity;
    }
}
