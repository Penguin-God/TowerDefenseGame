using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Linq;

[Serializable]
public class UnitData
{
    public UnitData(string _name, int _damage, int _specialAttackPercent, float _delayTime, float _speed, float _range) 
    {
        unitName = _name;
        OriginDamage = _damage;
        damage = _damage;
        bossDamage = _damage;
        OriginBossDamage = _damage;
        OriginAttackDelaytime = _specialAttackPercent;
        attackDelayTime = _delayTime;
        specialAttackPercent = _specialAttackPercent;
        speed = _speed; 
        attackRange = _range; 
    }

    [SerializeField] string unitName = "";
    public string UnitName => unitName;
    public int OriginDamage { get; private set; }
    public int OriginBossDamage { get; private set; }
    public float OriginAttackDelaytime { get; private set; }
    public int damage;
    public int bossDamage;
    public float attackDelayTime;
    public int specialAttackPercent;
    public float speed;
    public float attackRange;
}


[Serializable]
public struct PassiveData
{
    [SerializeField] string name;
    public string Name => name;

    public float p1;
    public float p2;
    public float p3;
    public float enhance_p1;
    public float enhance_p2;
    public float enhance_p3;

    public PassiveData(string _name, float _p1, float _p2, float _p3, float en_p1, float en_p2, float en_p3)
    {
        name = _name;
        p1 = _p1;
        p2 = _p2;
        p3 = _p3;
        enhance_p1 = en_p1;
        enhance_p2 = en_p2;
        enhance_p3 = en_p3;
    }
}

public class UnitDataList<T>
{
    public UnitDataList(List<T> p_List) => dataList = p_List;
    public List<T> dataList;
}

public class UnitDataBase : MonoBehaviour
{
    [SerializeField] public string[] unitTags;

    [ContextMenu("unit tag 세팅")]
    void SetUnitTags()
    {
        LoadUnitDataFromJson();
        unitTags = new string[loadDataList.Count];
        for (int i = 0; i < loadDataList.Count; i++) unitTags[i] = loadDataList[i].UnitName;
    }

    private void Awake()
    {
//#if UNITY_EDITOR
//        Debug.LogError("개같이 멸망");
//#else
//        Debug.Log("개같이 부활");
//#endif

        SaveUnitDataToJson();
        LoadUnitDataFromJson();
        SetUnitDataDictionary();

        SavePassiveDataToJson();
        LoadPassiveDataToJson();
        SetPassiveDictionary();

        SettingTagDictionary();
    }

    static Dictionary<KeyValuePair<UnitColor, UnitClass>, string> UnitTagDictionary;

    void SettingTagDictionary()
    {
        UnitTagDictionary = new Dictionary<KeyValuePair<UnitColor, UnitClass>, string>();
        int count = 0;
        foreach (UnitColor _color in Enum.GetValues(typeof(UnitColor)))
        {
            foreach (UnitClass _class in Enum.GetValues(typeof(UnitClass)))
            {
                KeyValuePair<UnitColor, UnitClass> _pair = new KeyValuePair<UnitColor, UnitClass>(_color, _class);
                UnitTagDictionary.Add(_pair, unitTags[count]);
                count++;
            }
        }
    }

    public static string GetUnitTag(UnitColor _color, UnitClass _class)
    {
        KeyValuePair<UnitColor, UnitClass> _pair = new KeyValuePair<UnitColor, UnitClass>(_color, _class);
        bool _isExist = UnitTagDictionary.TryGetValue(_pair, out string _tag);
        if (_isExist) return _tag;
        else return "";
    }

    private void OnValidate()
    {
        //SaveUnitDataToJson();
        //SavePassiveDataToJson();
        //Debug.Log("Save CSV Data To Json File");
    }


    [SerializeField] TextAsset unitData_CSV;
    [SerializeField] TextAsset Csv_UnitPassivedata = null;

    private string GetUnitDataPath()
    {
#if UNITY_EDITOR
        return UnitJsonPath;
#else
        return Application.dataPath + "/" + "unitData.txt";
#endif
    }

    private string GetUnitPassiveDataPath()
    {
#if UNITY_EDITOR
        return PassiveJsonPath;
#else
        return Application.dataPath + "/" + "UnitPassiveData.txt";
#endif
    }



    private string UnitJsonPath => Path.Combine(Application.dataPath, "4_Data", "UnitData", "JSON", "unitData.txt");
    private string PassiveJsonPath => Path.Combine(Application.dataPath, "4_Data", "UnitData", "JSON", "UnitPassiveData.txt");

    public List<UnitData> unitDataList;
    [ContextMenu("Save Unit Data To Json")]
    void SaveUnitDataToJson()
    {
        unitDataList = new List<UnitData>();

        string csvText = unitData_CSV.text.Substring(0, unitData_CSV.text.Length - 1);
        string[] datas = csvText.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음

        for (int i = 1; i < datas.Length; i++)
        {
            string[] cells = datas[i].Split(',');
            if (unitTags.Contains(cells[0]))
            {
                string _name = cells[0];
                int _damage = Int32.Parse(cells[1]);
                //int _bossDamage = Int32.Parse(cells[2]);
                int skillPercent = Int32.Parse(cells[3]);
                float _attackDelayTime = float.Parse(cells[4]);
                int _speed = Int32.Parse(cells[5]);
                int _attackRange = Int32.Parse(cells[6]);

                unitDataList.Add(new UnitData(_name, _damage, skillPercent, _attackDelayTime, _speed, _attackRange));
            }
            else if (cells[0] != "") Debug.Log($"NONE : {cells[0]}");
        }

        string jsonData = JsonUtility.ToJson(new UnitDataList<UnitData>(unitDataList), true);
        File.WriteAllText(GetUnitDataPath(), jsonData);
    }

    [SerializeField] List<UnitData> loadDataList;
    [ContextMenu("Load Unit Data From Json")]
    void LoadUnitDataFromJson()
    {
        loadDataList = new List<UnitData>();
        string jsonData = File.ReadAllText(GetUnitDataPath());
        loadDataList = JsonUtility.FromJson<UnitDataList<UnitData>>(jsonData).dataList;
    }

    private Dictionary<string, UnitData> UnitDataDictionary;
    void SetUnitDataDictionary()
    {
        UnitDataDictionary = new Dictionary<string, UnitData>();

        for (int i = 0; i < loadDataList.Count; i++)
        {
            UnitData _data = new UnitData(loadDataList[i].UnitName, loadDataList[i].damage, loadDataList[i].specialAttackPercent,
                            loadDataList[i].attackDelayTime, loadDataList[i].speed, loadDataList[i].attackRange);
            UnitDataDictionary.Add(loadDataList[i].UnitName, _data);
        }
    }


    public void ApplyUnitBaseData(string _name, TeamSoldier _team)
    {
        _team.originDamage = UnitDataDictionary[_name].OriginDamage;
        _team.damage = UnitDataDictionary[_name].damage;
        _team.originBossDamage = UnitDataDictionary[_name].OriginBossDamage;
        _team.bossDamage = UnitDataDictionary[_name].bossDamage;
        _team.originAttackDelayTime = UnitDataDictionary[_name].OriginAttackDelaytime;
        _team.attackDelayTime = UnitDataDictionary[_name].attackDelayTime;
        _team.speed = UnitDataDictionary[_name].speed;
        _team.attackRange = UnitDataDictionary[_name].attackRange;

        if(EventManager.instance.GetEventFlag(MyEventType.Up_UnitSkillPercent, (int)_team.unitColor))
        {
            IEvent @event = _team.GetComponent<IEvent>();
            if (@event != null) @event.SkillPercentUp();
            Debug.Log("HIHI");
        }

        if(_team.GetComponent<Unit_Mage>() != null)
        {
            Unit_Mage _mage = _team.GetComponent<Unit_Mage>();
            _mage.isUltimate = EventManager.instance.GetMageUltimateFlag(_team.unitColor);
            Debug.Log("Mage God");
        }
    }

    public void ChangeUnitDataOfColor(UnitColor _color, Action<UnitData> OnChaneData)
    {
        string[] _keys = GetDataKeys(_color);
        for (int i = 0; i < _keys.Length; i++) ChangeUnitData(_keys[i], OnChaneData);
    }

    public void ChangeUnitData(string _key, Action<UnitData> ChangeData) => ChangeData(UnitDataDictionary[_key]);



    public List<PassiveData> passiveDataList;
    public Dictionary<string, PassiveData> passiveDictionary;
    [ContextMenu("Save Passive To Json")]
    void SavePassiveDataToJson()
    {
        List<PassiveData> _passiveDataList = new List<PassiveData>();
        string csvText = Csv_UnitPassivedata.text.Substring(0, Csv_UnitPassivedata.text.Length - 1);
        string[] datas = csvText.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음

        for (int i = 1; i < datas.Length; i++)
        {
            string[] cells = datas[i].Split(',');
            if (unitTags.Contains(cells[0]))
            {
                string _name = cells[0];
                // 패시브는 셀 값이 공백인 경우가 있어서 삼항연산자 사용함
                float _p1 = (cells[1].Trim() != "") ? float.Parse(cells[1]) : 0;
                float _p2 = (cells[2].Trim() != "") ? float.Parse(cells[2]) : 0;
                float _p3 = (cells[3].Trim() != "") ? float.Parse(cells[3]) : 0;

                float _p4 = (cells[5].Trim() != "") ? float.Parse(cells[5]) : 0;
                float _p5 = (cells[6].Trim() != "") ? float.Parse(cells[6]) : 0;
                float _p6 = (cells[7].Trim() != "") ? float.Parse(cells[7]) : 0;
                _passiveDataList.Add(new PassiveData(_name, _p1, _p2, _p3, _p4, _p5, _p6));
            }
            else if(cells[0] != "") Debug.Log($"NONE : {cells[0]}");
        }

        string jsonData = JsonUtility.ToJson(new UnitDataList<PassiveData>(_passiveDataList), true);
        File.WriteAllText(GetUnitPassiveDataPath(), jsonData);
    }

    [ContextMenu("Load Passive Data To Json")]
    void LoadPassiveDataToJson()
    {
        passiveDataList = new List<PassiveData>();
        string jsonData = File.ReadAllText(GetUnitPassiveDataPath());
        passiveDataList = JsonUtility.FromJson<UnitDataList<PassiveData>>(jsonData).dataList;
    }

    void SetPassiveDictionary()
    {
        passiveDictionary = new Dictionary<string, PassiveData>();

        for (int i = 0; i < passiveDataList.Count; i++)
        {
            PassiveData _data = passiveDataList[i];
            passiveDictionary.Add(passiveDataList[i].Name, _data);
        }
    }

    public void ApplyPassiveData(string _key, UnitPassive _passive, UnitColor _color)
    {
        float[] passive_datas = new float[3];
        bool _isEnhance = EventManager.instance.GetEventFlag(MyEventType.Reinforce_UnitPassive, (int)_color);
        // 패시브 강화 여부에 따라 다른 값 적용
        passive_datas[0] = (_isEnhance) ? passiveDictionary[_key].enhance_p1 : passiveDictionary[_key].p1;
        passive_datas[1] = (_isEnhance) ? passiveDictionary[_key].enhance_p2 : passiveDictionary[_key].p2;
        passive_datas[2] = (_isEnhance) ? passiveDictionary[_key].enhance_p3 : passiveDictionary[_key].p3;

        _passive.ApplyData(passive_datas[0], passive_datas[1], passive_datas[2]);
    }

    public void ChangePassiveDataOfColor(UnitColor _color, Func<PassiveData, PassiveData> OnChaneData)
    {
        string[] _keys = GetDataKeys(_color);
        for (int i = 0; i < _keys.Length; i++) ChangePassiveData(_keys[i], OnChaneData);
    }

    public void ChangePassiveData(string _key, Func<PassiveData, PassiveData> ChaneData) => passiveDictionary[_key] = ChaneData(passiveDictionary[_key]);

    string[] GetDataKeys(UnitColor _color)
    {
        string[] _tags = new string[Enum.GetValues(typeof(UnitClass)).Length];
        int _index = 0;
        foreach (UnitClass _class in Enum.GetValues(typeof(UnitClass)))
        {
            _tags[_index] = GetUnitTag(_color, _class);
            _index++;
        }
        return _tags;
    }

    public string GetUnitKey(UnitColor _color, UnitClass _class)
    {
        string[] _colors = GetDataKeys(_color);
        return _colors[(int)_class];
    }
}