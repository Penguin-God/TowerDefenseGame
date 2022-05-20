using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum MyEventType
{
    Up_UnitDamage,
    Up_UnitBossDamage,
    Up_UnitSkillPercent,
    Reinforce_UnitPassive,
}

// 이벤트 상태를 바탕으로 Unit이 OnEnalbe() 에서 데이터를 세팅함
[Serializable]
public class UnitEventFlag
{
    //public Dictionary<MyEventType, bool> FlagDic { get; private set; }

    [SerializeField]
    private bool[] EventFlags;

    public UnitEventFlag()
    {
        EventFlags = new bool[Enum.GetValues(typeof(MyEventType)).Length];
    }

    public void UpFlag(MyEventType _type) => EventFlags[(int)_type] = true;
    public bool GetFlag(MyEventType _type) => EventFlags[(int)_type];
}

[Serializable]
public class MageEventFlag
{
    private Dictionary<UnitColor, bool> mageColorDic = new Dictionary<UnitColor, bool>();
    public MageEventFlag()
    {
        foreach (UnitColor _color in Enum.GetValues(typeof(UnitColor)))
        {
            if (_color == UnitColor.white) continue;
            mageColorDic.Add(_color, false);
        }
    }

    public void UpFlag(UnitColor _color) => mageColorDic[_color] = true;
    public bool GetFlag(UnitColor _color) => mageColorDic[_color];
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

        // 자료구조 세팅
        SetEvenType_Dictionary();
        SetEventFlagDic();
    }

    // Test GameManager에 Event구독하기
    private void Start()
    {
        ActionRandomEvent();
    }

    [SerializeField]
    private bool[] MageFlags = new bool[Enum.GetValues(typeof(UnitColor)).Length];
    void DebugMageFlag()
    {
        foreach (UnitColor _color in Enum.GetValues(typeof(UnitColor)))
        {
            if (_color == UnitColor.white) continue;
            MageFlags[(int)_color] = mageEventFlag.GetFlag(_color);
        }
    }
    private void Update()
    {
        DebugMageFlag();
    }

    Dictionary<MyEventType, Action<UnitColor>> eventDictionary;
    void SetEvenType_Dictionary()
    {
        eventDictionary = new Dictionary<MyEventType, Action<UnitColor>>();
        eventDictionary.Add(MyEventType.Up_UnitDamage, Up_UnitDamage);
        eventDictionary.Add(MyEventType.Up_UnitBossDamage, Up_UnitBossDamage);
        eventDictionary.Add(MyEventType.Up_UnitSkillPercent, Up_UnitSkillPercent);
        eventDictionary.Add(MyEventType.Reinforce_UnitPassive, Reinforce_UnitPassive);
    }

    public Action GetEvent(MyEventType _eventType, UnitColor _color)
    {
        eventDictionary.TryGetValue(_eventType, out Action<UnitColor> _evnet);
        return () => _evnet(_color);
    }

    // 모든 이벤트는 Used이벤트를 통해 실행됨
    public void UsedEvent(MyEventType _type, UnitColor _color)
    {
        if (eventDictionary.ContainsKey(_type))
        {
            EventFlagUp(_type, (int)_color);
            eventDictionary[_type](_color);
        }
    }

    // 시작 시 랜덤 이벤트 GameManager의 GameStart에서 작동함
    [SerializeField] Text buffText;
    void ActionRandomEvent()
    {
        MyEventType _myEvent = (MyEventType)UnityEngine.Random.Range(0, eventDictionary.Count);
        int unitColorNumber = UnityEngine.Random.Range(0, unitEventFlags.Length);
        UsedEvent(_myEvent, (UnitColor)unitColorNumber);
        // Text 세팅
        buffText.text = ReturnUnitText(unitColorNumber) + GetEvnetText(_myEvent);
    }

    [SerializeField] int eventUnitCount = 0;
    [SerializeField] UnitEventFlag[] unitEventFlags = null;
    void SetEventFlagDic()
    {
        unitEventFlags = new UnitEventFlag[eventUnitCount];
        for (int i = 0; i < unitEventFlags.Length; i++) unitEventFlags[i] = new UnitEventFlag();
    }

    void EventFlagUp(MyEventType _type, int _colorNum) => unitEventFlags[_colorNum].UpFlag(_type);
    
    public bool GetEventFlag(MyEventType _type, int _colorNum)
    {
        // 검정, 하양 예외처리
        if (_colorNum >= unitEventFlags.Length) return false;

        return unitEventFlags[_colorNum].GetFlag(_type);
    }


    private MageEventFlag mageEventFlag = new MageEventFlag();
    public void UpMageUltimateFlag(UnitColor _color) => mageEventFlag.UpFlag(_color);
    public bool GetMageUltimateFlag(UnitColor _color) => mageEventFlag.GetFlag(_color);    


    [SerializeField] UnitDataBase unitDataBase = null;
    // 풀 안에 있는 애들은 OnEnalbe() 실행하면서 수치를 초기화하기 때문에 현재 활성화된 유닛만 수치를 적용하면 됨
    // int가 아니라 UnitColor받게 하기
    public void Up_UnitDamage(UnitColor _color)
    {
        unitDataBase.ChangeUnitDataOfColor(_color, (UnitData _data) => ChangeUnitDamage(_data, 2));

        TeamSoldier[] _units = UnitManager.instance.GetCurrnetUnits(_color);
        for (int i = 0; i < _units.Length; i++)
        {
            string _unitTag = _units[i].gameObject.tag;
            UnitManager.instance.ApplyUnitData(_unitTag, _units[i]);
        }
    }

    public void Up_UnitBossDamage(UnitColor _color)
    {
        unitDataBase.ChangeUnitDataOfColor(_color, (UnitData _data) => ChangeUnitBossDamage(_data, 2));

        TeamSoldier[] _units = UnitManager.instance.GetCurrnetUnits(_color);
        for (int i = 0; i < _units.Length; i++)
        {
            string _unitTag = _units[i].gameObject.tag;
            UnitManager.instance.ApplyUnitData(_unitTag, _units[i]);
        }
    }

    void Up_UnitSkillPercent(UnitColor _color)
    {
        TeamSoldier[] _units = UnitManager.instance.GetCurrnetUnits(_color);
        for (int i = 0; i < _units.Length; i++)
        {
            IEvent interfaceEvent = _units[i].GetComponent<IEvent>();
            if(interfaceEvent != null) interfaceEvent.SkillPercentUp();
        }
    }

    void Reinforce_UnitPassive(UnitColor _color)
    {
        TeamSoldier[] _units = UnitManager.instance.GetCurrnetUnits(_color);
        for (int i = 0; i < _units.Length; i++)
        {
            string _unitTag = _units[i].gameObject.tag;
            UnitManager.instance.ApplyPassiveData(_unitTag, _units[i].GetComponent<UnitPassive>(), _color);
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

    // 패시브 때문에 아직 필요
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

    public void CurrnetAllUnitDamageUp()
    {
        foreach(TeamSoldier _unit in UnitManager.instance.CurrentAllUnits)
        {
            if (_unit != null) _unit.damage += _unit.originDamage;
        }
    }

    // 편의 이벤트
    string ReturnUnitText(int UnitFlags)
    {
        string unitColotText = "";
        switch (UnitFlags)
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

    string GetEvnetText(MyEventType _myEventType)
    {
        string _eventText = "";
        switch (_myEventType)
        {
            case MyEventType.Up_UnitDamage: _eventText = "대미지 강화"; break;
            case MyEventType.Up_UnitBossDamage: _eventText = "보스 대미지 강화"; break;
            case MyEventType.Up_UnitSkillPercent: _eventText = "스킬 사용 빈도 증가"; break;
            case MyEventType.Reinforce_UnitPassive: _eventText = "유닛스킬 강화"; break;
        }
        return _eventText;
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
