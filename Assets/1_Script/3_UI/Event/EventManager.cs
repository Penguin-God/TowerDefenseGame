using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    List<Action<GameObject[]>> buffActionList;
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
        SetBuff();

        // Dictonary 인스턴스
        eventTextDictionary = new Dictionary<Action<GameObject[]>, string>();
        SetEventText_Dictionary();

        // 이벤트 배열 선언
        Set_EventArray();
    }

    void SetEventText_Dictionary()
    {
        // 버프
        eventTextDictionary.Add(Up_UnitDamage, "대미지 강화");
        eventTextDictionary.Add(Up_UnitBossDamage, "보스 대미지 강화");
        eventTextDictionary.Add(Up_UnitSkillPercent, "스킬 사용 빈도 증가");
        eventTextDictionary.Add(Reinforce_UnitPassive, "유닛스킬 강화");
    }

    public void RandomUnitEvenet() // 실제 유닛 이벤트 작동
    {
        RandomBuffEvent();
    }

    void RandomBuffEvent()
    {
        ActionRandomEvent(buffText, buffActionList);
    }


    public Text buffText;
    [SerializeField]
    private bool[] unitColorIsEvent = new bool[] { false, false, false, false, false, false, false };  
    void ActionRandomEvent(Text eventText, List<Action<GameObject[]>> eventActionList)
    {
        int unitNumber = Check_UnitIsEvnet(); // unit 설정
        UnitManager.instance.ShowReinforceEffect(unitNumber);
        unitColorIsEvent[unitNumber] = true;

        // 이벤트 적용
        int eventNumber = UnityEngine.Random.Range(0, eventActionList.Count);
        eventActionList[eventNumber](UnitManager.instance.unitArrays[unitNumber].unitArray);
        eventText.text = ReturnUnitText(unitNumber) + eventTextDictionary[eventActionList[eventNumber]];
    }

    public void Action_SelectReinForceEvent(int eventNumber, int unitNumber)
    {
        buffActionList[eventNumber](UnitManager.instance.unitArrays[unitNumber].unitArray);
    }

    int Return_RandomUnitNumver()
    {
        int unitNumver = UnityEngine.Random.Range(0, UnitManager.instance.unitArrays.Length - 1); // 검은유닛 빼려고 -1
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

    void SetBuff()
    {
        buffActionList.Add(Up_UnitDamage);
        buffActionList.Add(Up_UnitBossDamage);
        buffActionList.Add(Up_UnitSkillPercent);
        buffActionList.Add(Reinforce_UnitPassive);
    }

    // script 가져올 때 GetComponentInChildren 써야됨
    void Up_UnitDamage(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            TeamSoldier teamSoldier = unitArray[i].GetComponentInChildren<TeamSoldier>();
            teamSoldier.damage += teamSoldier.originDamage * 1;
        }
    }

    void Up_UnitBossDamage(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            TeamSoldier teamSoldier = unitArray[i].GetComponentInChildren<TeamSoldier>();
            teamSoldier.bossDamage += teamSoldier.originBossDamage * 2;
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

    void Reinforce_UnitPassive(GameObject[] unitArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            IEvent interfaceEvent = unitArray[i].GetComponentInChildren<IEvent>();
            interfaceEvent.ReinforcePassive();
            unitArray[i].GetComponentInChildren<TeamSoldier>().passiveReinForce = true;
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
                unitColotText = "파란 유닛 : ";
                break;
            case 2:
                unitColotText = "노란 유닛 : ";
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

    public Action[] eventArray;

    void Set_EventArray()
    {
        eventArray = new Action[] { CurrentEnemyDie , currnetUnitDamageUp };
    }

    // 상점에서 파는 이벤트
    public EnemySpawn enemySpawn;
    public void CurrentEnemyDie()
    {
        int dieEnemyCount = 10;
        for (int i = 0; i < dieEnemyCount; i++)
        {
            if (enemySpawn.currentEnemyList.Count == 0) break;

            int dieEnemyNumber = UnityEngine.Random.Range(0, enemySpawn.currentEnemyList.Count);
            NomalEnemy enemy = enemySpawn.currentEnemyList[dieEnemyNumber].GetComponent<NomalEnemy>();
            if (enemy != null) enemy.Dead();
        }
    }

    public void currnetUnitDamageUp()
    {
        foreach(GameObject unit in UnitManager.instance.currentUnitList)
        {
            TeamSoldier teamSoldier = unit.GetComponentInParent<TeamSoldier>();
            if (teamSoldier != null) teamSoldier.damage += teamSoldier.originDamage;
        }
    }

    public List<int> Return_CurrentUnitColorList(int unitNumber) // 원하는 유닛 숫자를 받고 존재한 유닛들의 컬러 넘버가 담긴 리스트를 반환
    {
        List<int> current_UnitColorNumberList = new List<int>();
        UnitArray[] unitArray = UnitManager.instance.unitArrays;
        for(int i = 0; i < unitArray.Length; i++)
        {
            string unitTag = unitArray[i].unitArray[unitNumber].transform.GetChild(0).gameObject.tag;
            GameObject unit = GameObject.FindGameObjectWithTag(unitTag);
            if (unit != null) current_UnitColorNumberList.Add(i);
        }

        return current_UnitColorNumberList;
    }
}
