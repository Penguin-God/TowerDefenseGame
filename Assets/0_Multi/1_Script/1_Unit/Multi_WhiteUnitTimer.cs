using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Multi_WhiteUnitTimer : MonoBehaviour
{
    [SerializeField] Vector3 offSet;

    private Slider slider;
    public Slider Slider => slider;
    
    Transform target;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();   
    }

    public void Setup(Transform unit, float aliveTime)
    {
        target = unit;
        slider.maxValue = aliveTime;
        slider.value = aliveTime;
        StartCoroutine(Co_Timer());
    }

    public void Off()
    {
        StopAllCoroutines();
        slider.onValueChanged.RemoveAllListeners();
    }

    IEnumerator Co_Timer()
    {
        while (true)
        {
            if (target != null) transform.position = target.position + offSet;
            slider.value -= Time.deltaTime;
            yield return null;
        }
    }
}
