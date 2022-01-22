using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteUnitTimer : MonoBehaviour
{
    [SerializeField] float transformTime;
    private Slider timerSlider;
    public Vector3 offSet;
    public Transform targetUnit;

    private void Awake()
    {
        timerSlider = GetComponentInChildren<Slider>();
        timerSlider.maxValue = transformTime;
    }

    private void OnEnable()
    {
        transformTime = 30;
        StartCoroutine(Co_Timer());
    }

    IEnumerator Co_Timer()
    {
        while (true)
        {
            transformTime -= Time.deltaTime;
            timerSlider.value = transformTime;
            if (transformTime <= 0f)
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
