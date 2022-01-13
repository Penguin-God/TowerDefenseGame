using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum MyEventType
{
    None,
    Up_UnitDamage,
    Up_UnitBossDamage,
    Up_UnitSkillPercent,
    Reinforce_UnitPassive,
}

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

        buffActionList = new List<Action<int>>();
        SetBuff();

        // Dictonary 인스턴스
        eventTextDictionary = new Dictionary<Action<int>, string>();
        SetEventText_Dictionary();

        Debug.LogError("박준 메세지 : 이벤트 마무리 후 상점, 법사 상점 물품 선택 코드 수정하기");
    }

    // Test
    private void Start()
    {
        //Up_UnitDamage(2);
        //Up_UnitBossDamage(3);
        Reinforce_UnitPassive(1);
    }

    // 실제 유닛 이벤트 작동. GameManager의 GameStart에서 작동함
    [SerializeField] Text buffText;
    public void RandomBuffEvent() => ActionRandomEvent(buffText, buffActionList);


    List<Action<int>> buffActionList;
    Dictionary<Action<int>, string> eventTextDictionary;
    [SerializeField] UnitDataBase unitDataBase = null;
    void SetEventText_Dictionary()
    {
        // 버프
        eventTextDictionary.Add(Up_UnitDamage, "대미지 강화");
        eventTextDictionary.Add(Up_UnitBossDamage, "보스 대미지 강화");
        eventTextDictionary.Add(Up_UnitSkillPercent, "스킬 사용 빈도 증가");
        eventTextDictionary.Add(Reinforce_UnitPassive, "유닛스킬 강화");
    }

    [SerializeField]
    private bool[] unitColorIsEvent = new bool[] { false, false, false, false, false, false };  
    void ActionRandomEvent(Text eventText, List<Action<int>> eventActionList)
    {
        int unitColorNumber = UnityEngine.Random.Range(0, unitColorIsEvent.Length);
        //int unitColorNumber = Check_UnitIsEvnet(); // unit 설정
        //UnitManager.instance.ShowReinforceEffect(unitColorNumber);
        unitColorIsEvent[unitColorNumber] = true;

        // 이벤트 적용
        int eventNumber = UnityEngine.Random.Range(0, eventActionList.Count);
        eventActionList[eventNumber](unitColorNumber);
        eventText.text = ReturnUnitText(unitColorNumber) + eventTextDictionary[eventActionList[eventNumber]];
    }

    //int Return_RandomUnitNumver()
    //{
    //    int unitNumver = UnityEngine.Random.Range(0, 6);
    //    return unitNumver;
    //}
    //// 이벤트가 이미 적용된 유닛이면 다른 유닛넘버 리턴
    //int Check_UnitIsEvnet() 
    //{
    //    int unitNumber = Return_RandomUnitNumver();
    //    if (unitColorIsEvent[unitNumber])
    //    {
    //        unitNumber++;
    //        //if (unitNumber >= UnitManager.instance.unitArrays.Length) unitNumber = 0;
    //    }
    //    return unitNumber;
    //}



    void SetBuff()
    {
        buffActionList.Add(Up_UnitDamage);
        buffActionList.Add(Up_UnitBossDamage);
        buffActionList.Add(Up_UnitSkillPercent);
        buffActionList.Add(Reinforce_UnitPassive);
    }

    // 풀 안에 있는 애들은 OnEnalbe() 실행하면서 수치를 초기화하기 때문에 현재 활성화된 유닛만 수치를 적용하면 됨
    // 색깔넘버를 받음
    public void Up_UnitDamage(int _colorNum)
    {
        unitDataBase.ChangeUnitDataOfColor(_colorNum, (UnitData _data) => ChangeUnitDamage(_data, 2));

        TeamSoldier[] _units = UnitManager.instance.GetItems(_colorNum);
        for (int i = 0; i < _units.Length; i++)
        {
            string _unitTag = _units[i].gameObject.tag;
            UnitManager.instance.ApplyUnitData(_unitTag, _units[i]);
        }
    }

    public void Up_UnitBossDamage(int _colorNum)
    {
        unitDataBase.ChangeUnitDataOfColor(_colorNum, (UnitData _data) => ChangeUnitBossDamage(_data, 2));

        TeamSoldier[] _units = UnitManager.instance.GetItems(_colorNum);
        for (int i = 0; i < _units.Length; i++)
        {
            string _unitTag = _units[i].gameObject.tag;
            UnitManager.instance.ApplyUnitData(_unitTag, _units[i]);
        }
    }

    // 스킬강화같은 상태변수도 TeamSoldier에 추가해서 풀에 있던 애도 강화해야됨
    void Up_UnitSkillPercent(int _colorNum)
    {
        TeamSoldier[] _units = UnitManager.instance.GetItems(_colorNum);
        for (int i = 0; i < _units.Length; i++)
        {
            IEvent interfaceEvent = _units[i].GetComponent<IEvent>();
            if(interfaceEvent != null) interfaceEvent.SkillPercentUp();
        }
    }

    void Reinforce_UnitPassive(int _colorNum)
    {
        unitDataBase.ChangePassiveDataOfColor(_colorNum, (PassiveData _data) => { _data.isEnhance = true; return _data; });

        TeamSoldier[] _units = UnitManager.instance.GetItems(_colorNum);
        for (int i = 0; i < _units.Length; i++)
        {
            string _unitTag = _units[i].gameObject.tag;
            UnitManager.instance.ApplyPassiveData(_unitTag, _units[i]);
        }
    }


    public void ChangeUnitDamage(UnitData _data, float changeDamageWeigh) // 멀티에서 상대방 디버프도 고려
    {
        if (_data != null)
            _data.damage += Mathf.FloorToInt(_data.OriginDamage * (changeDamageWeigh - 1));
    }

    public void ChangeUnitBossDamage(UnitData _data, float changeDamageWeigh)
    {
        if (_data != null)
            _data.bossDamage += Mathf.FloorToInt(_data.OriginBossDamage * (changeDamageWeigh - 1));
    }

    public void ChangeUnitDamage(TeamSoldier _unit, float changeDamageWeigh) // 멀티에서 상대방 디버프도 고려
    {
        if (_unit != null)
            _unit.damage += Mathf.FloorToInt(_unit.originDamage * (changeDamageWeigh - 1));
    }

    public void ChangeUnitBossDamage(TeamSoldier _unit, float changeDamageWeigh)
    {
        if (_unit != null)
            _unit.bossDamage += Mathf.FloorToInt(_unit.originBossDamage * (changeDamageWeigh - 1));
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

    public void CurrnetUnitDamageUp()
    {
        foreach(GameObject unit in UnitManager.instance.CurrentUnitList)
        {
            TeamSoldier teamSoldier = unit.GetComponentInParent<TeamSoldier>();
            if (teamSoldier != null) teamSoldier.damage += teamSoldier.originDamage;
        }
    }

    string ReturnUnitText(int unitNumber)
    {
        string unitColotText = "";
        switch (unitNumber)
        {
            case 0: unitColotText = "빨간 유닛 : "; break;
            case 1: unitColotText = "파란 유닛 : "; break;
            case 2: unitColotText = "노란 유닛 : "; break;
            case 3: unitColotText = "초록 유닛 : "; break;
            case 4: unitColotText = "주황 유닛 : "; break;
            case 5: unitColotText = "보라 유닛 : "; break;
        }
        return unitColotText;
    }

    // 상점에 유닛 패시브 강화 판매를 추가하기 위한 빌드업 함수
    //public void Buy_Reinforce_UnitPassive(int colorNum, string unitColor)
    //{
    //    Reinforce_UnitPassive(colorNum);
    //    BeefUp_Passive(unitColor);
    //}

    //[ContextMenu("Reinforce")]
    //// 특정 색깔을 가진 유닛들 리턴
    //void BeefUp_Passive(string unitColor)
    //{
    //    string[] arr_UnitClass = new string[4] { "Swordman", "Archer", "Spearman", "Mage" };

    //    for(int i = 0; i < arr_UnitClass.Length; i++)
    //    {
    //        string tag = unitColor + arr_UnitClass[i];
    //        GameObject[] units = GameObject.FindGameObjectsWithTag(tag);
    //        Debug.Log(units.Length);
    //        foreach(GameObject unitObj in units)
    //        {
    //            UnitPassive passive = unitObj.GetComponentInChildren<UnitPassive>();
    //            if (passive != null) passive.Beefup_Passive();
    //        }
    //    }
    //}


    // 클래스 넘버를 인수로 받고 그 클래스의 유닛이 존재하면 유닛의 컬러 넘버를 List에 Add하고 반환
    //public List<int> Return_CurrentUnitColorList(int unitClassNumber) //  원하는 유닛 숫자를 받고 존재한 유닛들의 컬러 넘버가 담긴 리스트를 반환
    //{
    //    List<int> current_UnitColorNumberList = new List<int>();
    //    UnitArray[] unitArray = UnitManager.instance.unitArrays;
    //    for(int i = 0; i < unitArray.Length; i++)
    //    {
    //        string unitTag = unitArray[i].unitArray[unitClassNumber].transform.GetChild(0).gameObject.tag;
    //        GameObject unit = GameObject.FindGameObjectWithTag(unitTag);
    //        if (unit != null) current_UnitColorNumberList.Add(i);
    //    }

    //    return current_UnitColorNumberList;
    //}
}
