using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    //Func<GameObject[], string> event_DamageUp;
    //Func<GameObject[], string> event_DamageDown;

    //Func<GameObject[], string> event_SkillPercentUp;
    //Func<GameObject[], string> event_SkillPercentDown;

    //Func<GameObject[], string> event_BossDamageUp;
    //Func<GameObject[], string> event_BossDamageDown;

    //Func<GameObject[], string> event_SKillReinforce;
    //Func<GameObject[], string> event_SkillWeaken;

    //Func<GameObject[], string> event_PassiveReinforce;
    //Func<GameObject[], string> event_PassiveWeaken;

    List<Func<GameObject[], string>> eventFuncList;

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

        eventFuncList = new List<Func<GameObject[], string>>();
    }

    //void SetEventFunc()
    //{
    //    event_DamageUp = Up_UnitDamage;
    //    event_BossDamageUp = Up_UnitBossDamage;

    //    event_DamageDown = Down_UnitDamage;
    //    event_BossDamageDown = Down_UnitBossDamage;
    //}

    private void Start()
    {
    }

    public void SetEvent()
    {
        RandomBuffEvent();
        RandomDebuffEvent();
    }

    public Text buffText;
    public Text debuffText;
    void ActionRandomEvent(Text eventText)
    {
        int unitNumber = Return_RandomUnitNumver();
        int eventNumber = UnityEngine.Random.Range(0, eventFuncList.Count);
        //Func<GameObject[], string> eventFunc = eventFuncList[eventNumber];
        //eventFuncList[eventNumber](UnitManager.instance.unitArrays[unitNumber].unitArray);
        eventText.text = ReturnUnitText(unitNumber) + eventFuncList[eventNumber](UnitManager.instance.unitArrays[unitNumber].unitArray);
    }

    int Return_RandomUnitNumver()
    {
        int unitNumver = UnityEngine.Random.Range(0, UnitManager.instance.unitArrays.Length);
        return unitNumver;
    }

    public void RandomBuffEvent()
    {
        eventFuncList.Clear();
        eventFuncList.Add(Up_UnitDamage);
        eventFuncList.Add(Up_UnitBossDamage);
        ActionRandomEvent(buffText);
    }

    public void RandomDebuffEvent()
    {
        eventFuncList.Clear();
        eventFuncList.Add(Down_UnitDamage);
        eventFuncList.Add(Down_UnitBossDamage);
        ActionRandomEvent(debuffText);
    }

    string Up_UnitDamage(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            unitArray[i].GetComponentInChildren<TeamSoldier>().damage *= 2;
        }
        return "일반 몬스터 대미지 강화";
    }

    string Down_UnitDamage(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            unitArray[i].GetComponentInChildren<TeamSoldier>().damage = 
                Mathf.RoundToInt(unitArray[i].GetComponentInChildren<TeamSoldier>().damage / 2);
        }
        return "일반 몬스터 대미지 약화";
    }

    string Up_UnitBossDamage(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            unitArray[i].GetComponentInChildren<TeamSoldier>().bossDamage *= 2;
        }
        return "보스 대미지 강화";
    }

    string Down_UnitBossDamage(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            unitArray[i].GetComponentInChildren<TeamSoldier>().bossDamage = 
                Mathf.RoundToInt(unitArray[i].GetComponentInChildren<TeamSoldier>().bossDamage / 2);
        }
        return "보스 대미지 약화";
    }

    string ReturnUnitText(int unitNumber)
    {
        string unitColotText = "";
        switch (unitNumber)
        {
            case 0:
                unitColotText = "빨간 유닛 : ";
                break;
            case 1:
                unitColotText = "파랑 유닛 : ";
                break;
            case 2:
                unitColotText = "노랑 유닛 : ";
                break;
            case 3:
                unitColotText = "초록 유닛 : ";
                break;
            case 4:
                unitColotText = "주황 유닛 : ";
                break;
            case 5:
                unitColotText = "보라 유닛 : ";
                break;
            case 6:
                unitColotText = "검정 유닛 : ";
                break;
        }
        return unitColotText;
    }
}
