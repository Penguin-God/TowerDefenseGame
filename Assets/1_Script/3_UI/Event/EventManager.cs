using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    List<Action<int>> buffActionList;
    Dictionary<Action<int>, string> eventTextDictionary;

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
        buffActionList = new List<Action<int>>();
        SetBuff();

        // Dictonary 인스턴스
        eventTextDictionary = new Dictionary<Action<int>, string>();
        SetEventText_Dictionary();

        // 이벤트 배열 선언
        //Set_EventArray();
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
    private bool[] unitColorIsEvent = new bool[] { false, false, false, false, false, false };  
    void ActionRandomEvent(Text eventText, List<Action<int>> eventActionList)
    {
        int unitColorNumber = Check_UnitIsEvnet(); // unit 설정
        UnitManager.instance.ShowReinforceEffect(unitColorNumber);
        unitColorIsEvent[unitColorNumber] = true;

        // 이벤트 적용
        int eventNumber = UnityEngine.Random.Range(0, eventActionList.Count);
        eventActionList[eventNumber](unitColorNumber);
        eventText.text = ReturnUnitText(unitColorNumber) + eventTextDictionary[eventActionList[eventNumber]];
    }

    int Return_RandomUnitNumver()
    {
        int unitNumver = UnityEngine.Random.Range(0, 6);
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

    void SetBuff()
    {
        buffActionList.Add(Up_UnitDamage);
        buffActionList.Add(Up_UnitBossDamage);
        buffActionList.Add(Up_UnitSkillPercent);
        buffActionList.Add(Reinforce_UnitPassive);
    }




    // script 가져올 때 GetComponentInChildren 써야됨
    public void Up_UnitDamage(int colorNumber)
    {
        TeamSoldier[] teams = ReturnColorTeamSoldiers(colorNumber);
        for (int i = 0; i < teams.Length; i++)
        {
            ChangeUnitDamage(teams[i], 2);
        }
    }

    public void Up_UnitBossDamage(int colorNumber)
    {
        TeamSoldier[] teams = ReturnColorTeamSoldiers(colorNumber);
        for (int i = 0; i < teams.Length; i++)
        {
            //teamSoldier.bossDamage += teamSoldier.originBossDamage * 2;
            ChangeUnitBossDamage(teams[i], 2);
        }
    }

    void Up_UnitSkillPercent(int colorNumber) // Interface를 사용해 Up_Skill과 같은 함수를 만들것
    {
        GameObject[] unitArray = ReturnColorSoldierObjects(colorNumber);
        for (int i = 0; i < unitArray.Length; i++)
        {
            IEvent interfaceEvent = unitArray[i].GetComponentInChildren<IEvent>();
            interfaceEvent.SkillPercentUp();
        }
    }

    void Reinforce_UnitPassive(int colorNumber)
    {
        GameObject[] unitArray = ReturnColorSoldierObjects(colorNumber);
        for (int i = 0; i < unitArray.Length; i++)
        {
            UnitPassive passive = unitArray[i].GetComponentInChildren<UnitPassive>();
            passive.Beefup_Passive();
            unitArray[i].GetComponentInChildren<TeamSoldier>().passiveReinForce = true;
        }
    }

    //public Action[] eventArray;

    //void Set_EventArray()
    //{
    //    eventArray = new Action[] { CurrentEnemyDie , currnetUnitDamageUp };
    //}


    public void ChangeUnitDamage(TeamSoldier teamSoldier, float changeDamageWeigh) // 멀티에서 상대방 디버프도 고려
    {
        if (teamSoldier != null)
            teamSoldier.damage += Mathf.FloorToInt(teamSoldier.originDamage * (changeDamageWeigh - 1));
    }

    public void ChangeUnitBossDamage(TeamSoldier teamSoldier, float changeDamageWeigh)
    {
        if (teamSoldier != null)
            teamSoldier.bossDamage += Mathf.FloorToInt(teamSoldier.originBossDamage * (changeDamageWeigh - 1));
    }

    TeamSoldier[] ReturnColorTeamSoldiers(int colorNumber)
    {
        GameObject[] unitArray = UnitManager.instance.unitArrays[colorNumber].unitArray;
        List<TeamSoldier> teamList = new List<TeamSoldier>();
        for (int i = 0; i < unitArray.Length; i++)
        {
            teamList.Add(unitArray[i].GetComponentInChildren<TeamSoldier>());
        }
        return teamList.ToArray();
    }

    GameObject[] ReturnColorSoldierObjects(int colorNumber)
    {
        GameObject[] unitArray = UnitManager.instance.unitArrays[colorNumber].unitArray;

        return unitArray;
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

    // 상점에 유닛 패시브 강화 판매를 추가하기 위한 빌드업 함수
    public void Buy_Reinforce_UnitPassive(int colorNum, string unitColor)
    {
        Reinforce_UnitPassive(colorNum);
        BeefUp_Passive(unitColor);
    }

    [ContextMenu("Reinforce")]
    // 특정 색깔을 가진 유닛들 리턴
    void BeefUp_Passive(string unitColor)
    {
        string[] arr_UnitClass = new string[4] { "Swordman", "Archer", "Spearman", "Mage" };

        for(int i = 0; i < arr_UnitClass.Length; i++)
        {
            string tag = unitColor + arr_UnitClass[i];
            GameObject[] units = GameObject.FindGameObjectsWithTag(tag);
            Debug.Log(units.Length);
            foreach(GameObject unitObj in units)
            {
                UnitPassive passive = unitObj.GetComponentInChildren<UnitPassive>();
                if (passive != null) passive.Beefup_Passive();
            }
        }
    }

    // 클래스 넘버를 인수로 받고 그 클래스의 유닛이 존재하면 유닛의 컬러 넘버를 List에 Add하고 반환
    public List<int> Return_CurrentUnitColorList(int unitClassNumber) //  원하는 유닛 숫자를 받고 존재한 유닛들의 컬러 넘버가 담긴 리스트를 반환
    {
        List<int> current_UnitColorNumberList = new List<int>();
        UnitArray[] unitArray = UnitManager.instance.unitArrays;
        for(int i = 0; i < unitArray.Length; i++)
        {
            string unitTag = unitArray[i].unitArray[unitClassNumber].transform.GetChild(0).gameObject.tag;
            GameObject unit = GameObject.FindGameObjectWithTag(unitTag);
            if (unit != null) current_UnitColorNumberList.Add(i);
        }

        return current_UnitColorNumberList;
    }
}
