using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteUnitTimer : MonoBehaviour
{
    private float transformTime;
    private Slider timerSlider;
    public Vector3 offSet;
    public Transform targetUnit;

    private void Awake()
    {
        timerSlider = GetComponentInChildren<Slider>();
        transformTime = 30;
        timerSlider.maxValue = transformTime;
    }

    // Update is called once per frame
    void Update()
    {
        transformTime -= Time.deltaTime;
        timerSlider.value = transformTime;
        if(transformTime <= 0f)
        {
            targetUnit.gameObject.GetComponent<WhiteUnitEvent>().UnitTransform();
            Destroy(gameObject);
        }

        transform.position = targetUnit.position + offSet;
    }
}
