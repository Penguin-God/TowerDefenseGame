using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteUnitTimer : MonoBehaviour
{
    [SerializeField] float aliveTime;
    [SerializeField] Vector3 offSet;

    private Slider timerSlider;
    public Transform targetUnit;

    private void Awake()
    {
        timerSlider = GetComponentInChildren<Slider>();
        timerSlider.maxValue = aliveTime;
    }

    private void OnEnable()
    {
        timerSlider.value = aliveTime;
        StartCoroutine(Co_Timer());
    }

    IEnumerator Co_Timer()
    {
        while (true)
        {
            aliveTime -= Time.deltaTime;
            timerSlider.value = aliveTime;
            if (aliveTime <= 0f)
            {
                targetUnit.gameObject.GetComponent<WhiteUnitEvent>().UnitTransform();
                gameObject.SetActive(false);
                yield break;
            }

            if(targetUnit != null) transform.position = targetUnit.position + offSet;
            yield return null;
        }
    }
}
