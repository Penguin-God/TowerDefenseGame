using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteUnit : MonoBehaviour
{
    [SerializeField] int classNumber;
    [SerializeField] int maxColor = 0;
    [SerializeField] float aliveTime;
    Multi_WhiteUnitTimer timer = null;

    void OnEnable()
    {
        timer = Multi_SpawnManagers.Effect.ShwoForTime(Effects.WhiteUnit_Timer, transform.position, aliveTime).GetComponent<Multi_WhiteUnitTimer>();
        timer.Setup(transform, aliveTime);
        timer.Slider.onValueChanged.AddListener(ChangedColor);
    }

    public void ChangedColor(float value)
    {
        if(value <= 0)
        {
            Multi_SpawnManagers.NormalUnit.Spawn(Random.Range(0, maxColor), classNumber, transform.position, transform.rotation);
            Multi_Managers.Pool.Push(gameObject);
            timer.Off();
            timer = null;
        }
    }
}
