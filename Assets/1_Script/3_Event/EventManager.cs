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

    List<Func<GameObject[], string>> buffFuncList;
    List<Func<GameObject[], string>> debuffFuncList;
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

        // 시작할 때 유닛 이벤트는 GameManager의 GameStart에서 작동함
        buffFuncList = new List<Func<GameObject[], string>>();
        debuffFuncList = new List<Func<GameObject[], string>>();
        SetEvent();
    }

    public Text buffText;
    public Text debuffText;
    [SerializeField]
    private bool[] unitColorIsEvent = new bool[] { false, false, false, false, false, false, false };  
    void ActionRandomEvent(Text eventText, List<Func<GameObject[], string>> eventFuncList)
    {
        int unitNumber = Return_RandomUnitNumver();
        if (unitColorIsEvent[unitNumber])
        {
            unitNumber++;
            if (unitNumber >= eventFuncList.Count) unitNumber = 0;
        }

        unitColorIsEvent[unitNumber] = true;
        int eventNumber = UnityEngine.Random.Range(0, eventFuncList.Count);
        eventText.text = ReturnUnitText(unitNumber) + eventFuncList[eventNumber](UnitManager.instance.unitArrays[unitNumber].unitArray);
    }

    int Return_RandomUnitNumver()
    {
        int unitNumver = UnityEngine.Random.Range(0, UnitManager.instance.unitArrays.Length);
        return unitNumver;
    }

    void SetEvent()
    {
        SetBuff();
        SetDeBuff();
    }

    void SetBuff()
    {
        buffFuncList.Add(Up_UnitDamage);
        buffFuncList.Add(Up_UnitBossDamage);
        buffFuncList.Add(Up_UnitSkillPercent);
        buffFuncList.Add(Reinforce_UnitPassive);
    }

    void SetDeBuff()
    {
        debuffFuncList.Add(Down_UnitDamage);
        debuffFuncList.Add(Down_UnitBossDamage);
        debuffFuncList.Add(Down_UnitSkillPercent);
        debuffFuncList.Add(Weaken_UnitPassive);
    }

    public void RandomUnitEvenet()
    {
        RandomBuffEvent();
        RandomDebuffEvent();
    }

    public void RandomBuffEvent()
    {
        ActionRandomEvent(buffText, buffFuncList);
    }

    public void RandomDebuffEvent()
    {
        ActionRandomEvent(debuffText, debuffFuncList);
    }


    // 가져올 때 GetComponentInChildren 써야됨
    string Up_UnitDamage(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            TeamSoldier teamSoldier = unitArray[i].GetComponentInChildren<TeamSoldier>();
            teamSoldier.damage *= 2;
        }
        return "일반 몬스터 대미지 강화";
    }

    string Down_UnitDamage(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            TeamSoldier teamSoldier = unitArray[i].GetComponentInChildren<TeamSoldier>();
            teamSoldier.damage = Mathf.RoundToInt(teamSoldier.damage / 2);
        }
        return "일반 몬스터 대미지 약화";
    }

    string Up_UnitBossDamage(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            TeamSoldier teamSoldier = unitArray[i].GetComponentInChildren<TeamSoldier>();
            teamSoldier.bossDamage *= 2;
        }
        return "보스 대미지 강화";
    }

    string Down_UnitBossDamage(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            TeamSoldier teamSoldier = unitArray[i].GetComponentInChildren<TeamSoldier>();
            teamSoldier.bossDamage = Mathf.RoundToInt(teamSoldier.bossDamage / 2);
        }
        return "보스 대미지 약화";
    }

    string Up_UnitSkillPercent(GameObject[] unitArray) // Interface를 사용해 Up_Skill과 같은 함수를 만들것
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            IEvent interfaceEvent = unitArray[i].GetComponentInChildren<IEvent>();
            interfaceEvent.SkillPercentUp();
        }

        return "스킬 사용 빈도 증가";
    }

    string Down_UnitSkillPercent(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            IEvent interfaceEvent = unitArray[i].GetComponentInChildren<IEvent>();
            interfaceEvent.SkillPercentDown();
        }

        return "스킬 사용 빈도 감소";
    }

    string Reinforce_UnitPassive(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            IEvent interfaceEvent = unitArray[i].GetComponentInChildren<IEvent>();
            interfaceEvent.ReinforcePassive();
        }
        return "패시브 강화"; 
    }

    string Weaken_UnitPassive(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            IEvent interfaceEvent = unitArray[i].GetComponentInChildren<IEvent>();
            interfaceEvent.WeakenPassive();
        }
        return "패시브 약화";
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
