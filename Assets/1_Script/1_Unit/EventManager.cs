using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("EventManager 2개");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        RandomBuffEvent(0);
    }

    int Return_RandomUnitNumver()
    {
        int unitNumver = Random.Range(0, UnitManager.instance.unitArrays.Length);
        return unitNumver;
    }

    public void RandomBuffEvent(int eventNumber)
    {
        int unitNumber = Return_RandomUnitNumver();
        switch (eventNumber)
        {
            // UnitManager.instance.unitArrays[unitNumber].unitArray 의 형식으로 접근
            case 0:
                Debug.Log(unitNumber);
                Up_UnitDamage(UnitManager.instance.unitArrays[unitNumber].unitArray);
                break;
        }
    }

    void Up_UnitDamage(GameObject[] unitArray)
    {
        for(int i = 0; i < unitArray.Length; i++)
        {
            unitArray[i].GetComponentInChildren<TeamSoldier>().damage *= 2;
        }
    }

    public void RandomDebuffEvent(int eventNumber)
    {
        switch (eventNumber)
        {
            case 0:
                break;
        }
    }
}
