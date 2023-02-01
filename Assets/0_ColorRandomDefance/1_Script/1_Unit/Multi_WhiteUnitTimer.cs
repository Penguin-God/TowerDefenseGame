using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Multi_WhiteUnitTimer : MonoBehaviour
{
    private Slider slider;
    public Slider Slider => slider;
    
    Transform _target;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    public void Setup(Transform target, float aliveTime)
    {
        _target = target;
        slider.maxValue = aliveTime;
        slider.value = aliveTime;
        StartCoroutine(Co_Timer());
    }

    public void Off()
    {
        slider.onValueChanged.RemoveAllListeners();
        StopAllCoroutines();
        Managers.Effect.StopTargetTracking(_target);
        _target = null;
    }

    IEnumerator Co_Timer()
    {
        while (true)
        {
            slider.value -= Time.deltaTime;
            yield return null;
        }
    }
}
