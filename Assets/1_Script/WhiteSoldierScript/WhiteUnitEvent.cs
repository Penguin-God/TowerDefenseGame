using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteUnitEvent : MonoBehaviour
{
    public int unitNumber;
    public CreateDefenser createDefenser;
    
    private int Colornumber;
    public GameObject timerObject;

    public AudioClip unitTransformClip;

    private void Awake()
    {
        GameObject TimerCanavs = Instantiate(timerObject, timerObject.transform.position, timerObject.transform.rotation);
        TimerCanavs.GetComponent<WhiteUnitTimer>().targetUnit = transform;
        Colornumber = Random.Range(0, 6);
    }

    public void UnitTransform()
    {
        UnitManager.instance.unitAudioManagerSource.PlayOneShot(unitTransformClip, 0.9f);
        createDefenser.CreateSoldier(Colornumber, unitNumber, transform);
        Destroy(gameObject);
    }
}
