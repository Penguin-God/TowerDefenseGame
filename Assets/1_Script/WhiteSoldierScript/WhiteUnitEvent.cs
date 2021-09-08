using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteUnitEvent : MonoBehaviour
{
    public int unitNumber;
    public CreateDefenser createDefenser;
    
    private int Colornumber;
    public GameObject timerObject;

    private void Awake()
    {
        GameObject TimerCanavs = Instantiate(timerObject, timerObject.transform.position, timerObject.transform.rotation);
        TimerCanavs.GetComponent<WhiteUnitTimer>().targetUnit = transform;
        Colornumber = Random.Range(0, 6);
    }

    public void UnitTransform()
    {
        SoundManager.instance.PlayEffectSound_ByName("TransformWhiteUnit");
        createDefenser.CreateWhiteUnit(Colornumber, unitNumber, transform);
        Destroy(gameObject);
    }
}
