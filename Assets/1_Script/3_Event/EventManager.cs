using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    List<Action<GameObject[]>> buffActionList;
    //List<Action<GameObject[]>> debuffActionList;
    Dictionary<Action<GameObject[]>, string> eventTextDictionary;

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
        buffActionList = new List<Action<GameObject[]>>();
        //debuffActionList = new List<Action<GameObject[]>>();
        SetEvent();

        // Dictonary 인스턴스
        eventTextDictionary = new Dictionary<Action<GameObject[]>, string>();
        SetEventText_Dictionary();
    }

    void SetEventText_Dictionary()
    {
        // 버프
        eventTextDictionary.Add(Up_UnitDamage, "일반 몬스터 대미지 강화");
        eventTextDictionary.Add(Up_UnitBossDamage, "보스 대미지 강화");
        eventTextDictionary.Add(Up_UnitSkillPercent, "스킬 사용 빈도 증가");
        eventTextDictionary.Add(Reinforce_UnitPassive, "패시브 강화");

        // 디버프
        //eventTextDictionary.Add(Down_UnitDamage, "일반 몬스터 대미지 약화");
        //eventTextDictionary.Add(Down_UnitBossDamage, "보스 대미지 약화");
        //eventTextDictionary.Add(Down_UnitSkillPercent, "스킬 사용 빈도 감소");
        //eventTextDictionary.Add(Weaken_UnitPassive, "패시브 삭제");
    }

    public void RandomUnitEvenet() // 실제 유닛 이벤트 작동
    {
        RandomBuffEvent();
        //RandomDebuffEvent();
    }

    void RandomBuffEvent()
    {
        ActionRandomEvent(buffText, buffActionList);
    }

    public void RandomDebuffEvent()
    {
        //ActionRandomEvent(debuffText, debuffActionList);
    }


    public Text buffText;
    public Text debuffText;
    [SerializeField]
    private bool[] unitColorIsEvent = new bool[] { false, false, false, false, false, false, false };  
    void ActionRandomEvent(Text eventText, List<Action<GameObject[]>> eventActionList)
    {
        int unitNumber = Check_UnitIsEvnet();
        unitColorIsEvent[unitNumber] = true;

        int eventNumber = UnityEngine.Random.Range(0, eventActionList.Count);
        eventActionList[eventNumber](UnitManager.instance.unitArrays[unitNumber].unitArray);
        eventText.text = ReturnUnitText(unitNumber) + eventTextDictionary[eventActionList[eventNumber]];
    }

    int Return_RandomUnitNumver()
    {
        int unitNumver = UnityEngine.Random.Range(0, UnitManager.instance.unitArrays.Length);
        return unitNumver;
    }

    // 이벤트가 이미 적용된 유닛이면 다른 유닛넘버 리턴
    int Check_UnitIsEvnet() 
    {
        int unitNumber = Return_RandomUnitNumver();
        if (unitColorIsEvent[unitNumber])
        {
            unitNumber++;
            if (unitNumber >= UnitManager.instance.unitArrays.Length) unitNumber = 0;
        }
        return unitNumber;
    }

    void SetEvent()
    {
        SetBuff();
        //SetDeBuff();
    }

    void SetBuff()
    {
        buffActionList.Add(Up_UnitDamage);
        buffActionList.Add(Up_UnitBossDamage);
        buffActionList.Add(Up_UnitSkillPercent);
        buffActionList.Add(Reinforce_UnitPassive);
    }

    void SetDeBuff()
    {
        //debuffActionList.Add(Down_UnitDamage);
        //debuffActionList.Add(Down_UnitBossDamage);
        //debuffActionList.Add(Down_UnitSkillPercent);
        //debuffActionList.Add(Weaken_UnitPassive);
    }


    // script 가져올 때 GetComponentInChildren 써야됨
    void Up_UnitDamage(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            TeamSoldier teamSoldier = unitArray[i].GetComponentInChildren<TeamSoldier>();
            teamSoldier.damage *= 2;
        }
    }

    void Down_UnitDamage(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            TeamSoldier teamSoldier = unitArray[i].GetComponentInChildren<TeamSoldier>();
            teamSoldier.damage = Mathf.RoundToInt(teamSoldier.damage / 2);
        }
    }

    void Up_UnitBossDamage(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            TeamSoldier teamSoldier = unitArray[i].GetComponentInChildren<TeamSoldier>();
            teamSoldier.bossDamage *= 2;
        }
    }

    void Down_UnitBossDamage(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            TeamSoldier teamSoldier = unitArray[i].GetComponentInChildren<TeamSoldier>();
            teamSoldier.bossDamage = Mathf.RoundToInt(teamSoldier.bossDamage / 2);
        }
    }

    void Up_UnitSkillPercent(GameObject[] unitArray) // Interface를 사용해 Up_Skill과 같은 함수를 만들것
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            IEvent interfaceEvent = unitArray[i].GetComponentInChildren<IEvent>();
            interfaceEvent.SkillPercentUp();
        }

    }

    void Down_UnitSkillPercent(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            IEvent interfaceEvent = unitArray[i].GetComponentInChildren<IEvent>();
            interfaceEvent.SkillPercentDown();
        }

    }

    void Reinforce_UnitPassive(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            IEvent interfaceEvent = unitArray[i].GetComponentInChildren<IEvent>();
            interfaceEvent.ReinforcePassive();
        }
    }

    void Weaken_UnitPassive(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            IEvent interfaceEvent = unitArray[i].GetComponentInChildren<IEvent>();
            interfaceEvent.WeakenPassive();
        }
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
        }
        return unitColotText;
    }
}
