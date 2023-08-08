using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Multi_WhiteUnitTimer : MonoBehaviour
{
    private Slider slider;
    public Slider Slider => slider;
    
    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    public void Setup(float aliveTime)
    {
        slider.maxValue = aliveTime;
        slider.value = aliveTime;
        StartCoroutine(Co_Timer());
    }

    public void Off()
    {
        slider.onValueChanged.RemoveAllListeners();
        StopAllCoroutines();
        Managers.Effect.CancleTracking(GetComponent<TargetTracker>());
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
