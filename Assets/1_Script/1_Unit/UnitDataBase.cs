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


//[System.Serializable]
//public struct UnitStruct
//{
//    public string name;
//    public TeamSoldier unit;

//    public UnitStruct(string _name, TeamSoldier _unit)
//    {
//        name = _name;
//        unit = _unit;
//    }
//}

//[System.Serializable]
//public struct UnitPassiveStruct
//{
//    public string name;
//    public UnitPassive unitPassive;

//    public UnitPassiveStruct(string _name, UnitPassive _unitPassive)
//    {
//        name = _name;
//        unitPassive = _unitPassive;
//    }
//}

public class UnitDataList<T>
{
    public UnitDataList(List<T> p_List) => dataList = p_List;
    public List<T> dataList;
}

public class UnitDataBase : MonoBehaviour
{
    public string[] unitTags;

    [ContextMenu("unit tag 세팅")]
    void SetUnitTags()
    {
        LoadUnitDataFromJson();
        unitTags = new string[loadDataList.Count];
        for (int i = 0; i < loadDataList.Count; i++)
        {
            unitTags[i] = loadDataList[i].UnitName;
        }
    }

    private void Awake()
    {
        LoadUnitDataFromJson();
        SetUnitDataDictionary();

        LoadPassiveDataToJson();
        SetPassiveDictionary();
    }

    private void OnValidate()
    {
        SaveUnitDataToJson();
        SavePassiveDataToJson();
        Debug.Log("Save CSV Data To Json File");
    }


    [SerializeField] TextAsset unitData_CSV;
    [SerializeField] TextAsset Csv_UnitPassivedata = null;


    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/“+”/Student Data.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Student Data.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Student Data.csv";
#else
        return Application.dataPath +"/"+"Student Data.csv";
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
            else Debug.Log($"NONE : {cells[0]}");
        }

        string jsonData = JsonUtility.ToJson(new UnitDataList<UnitData>(unitDataList), true);
        File.WriteAllText(UnitJsonPath, jsonData);
    }

    [SerializeField] List<UnitData> loadDataList;
    [ContextMenu("Load Unit Data From Json")]
    void LoadUnitDataFromJson()
    {
        loadDataList = new List<UnitData>();
        string jsonData = File.ReadAllText(UnitJsonPath);
        loadDataList = JsonUtility.FromJson<UnitDataList<UnitData>>(jsonData).dataList;
    }

    private Dictionary<string, UnitData> UnitDataDictionary;
    void SetUnitDataDictionary()
    {
        UnitDataDictionary = new Dictionary<string, UnitData>();

        for (int i = 0; i < loadDataList.Count; i++)
        {
            UnitData _data = 
                new UnitData(loadDataList[i].UnitName, loadDataList[i].damage, loadDataList[i].specialAttackPercent,
                                loadDataList[i].attackDelayTime, loadDataList[i].speed, loadDataList[i].attackRange);
            UnitDataDictionary.Add(loadDataList[i].UnitName, _data);
        }
    }


    public void ApplyData(string _name, TeamSoldier _team)
    {
        _team.originDamage = UnitDataDictionary[_name].OriginDamage;
        _team.damage = UnitDataDictionary[_name].damage;
        _team.originBossDamage = UnitDataDictionary[_name].OriginBossDamage;
        _team.bossDamage = UnitDataDictionary[_name].bossDamage;
        _team.originAttackDelayTime = UnitDataDictionary[_name].OriginAttackDelaytime;
        _team.attackDelayTime = UnitDataDictionary[_name].attackDelayTime;
        _team.speed = UnitDataDictionary[_name].speed;
        _team.attackRange = UnitDataDictionary[_name].attackRange;
    }

    public void ChangeUnitData(string _key, Action<UnitData> ChangeData)
    {
        ChangeData(UnitDataDictionary[_key]);
    }



    public List<PassiveData> passiveDataList;
    public Dictionary<string, PassiveData> passiveDictionary;
    [ContextMenu("Save Passive To Json")]
    void SavePassiveDataToJson()
    {
        passiveDataList = new List<PassiveData>();

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
                passiveDataList.Add(new PassiveData(_name, _p1, _p2, _p3, _p4, _p5, _p6));
            }
            else Debug.Log($"NONE : {cells[0]}");
        }

        string jsonData = JsonUtility.ToJson(new UnitDataList<PassiveData>(passiveDataList), true);
        File.WriteAllText(PassiveJsonPath, jsonData);
    }

    [ContextMenu("Load Passive Data To Json")]
    void LoadPassiveDataToJson()
    {
        passiveDataList = new List<PassiveData>();
        string jsonData = File.ReadAllText(PassiveJsonPath);
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

    public float[] GetPassiveData(string _key)
    {
        float[] passive_datas = new float[6];
        passive_datas[0] = passiveDictionary[_key].p1;
        passive_datas[1] = passiveDictionary[_key].p2;
        passive_datas[2] = passiveDictionary[_key].p3;
        passive_datas[3] = passiveDictionary[_key].enhance_p1;
        passive_datas[4] = passiveDictionary[_key].enhance_p2;
        passive_datas[5] = passiveDictionary[_key].enhance_p3;
        return passive_datas;
    }

    public void ChangePassiveData(string _key, Action<PassiveData> ChaneData)
    {
        ChaneData(passiveDictionary[_key]);
    }


    //[ContextMenu("SetUnitData")]
    //void SetUnitData()
    //{
    //    int count = Mathf.Min(loadDataList.Count, unitList.Count);
    //    for(int i = 0; i < count; i++) // TeamSoldier에 데이터 세팅 함수 만들기
    //    {
    //        TeamSoldier unit = unitList[i].GetComponentInChildren<TeamSoldier>();
    //        unit.originDamage = loadDataList[i].unitDamage;
    //        unit.damage = loadDataList[i].unitDamage;
    //        unit.originBossDamage = loadDataList[i].unitDamage;
    //        unit.bossDamage = loadDataList[i].unitDamage;
    //        unit.speed = loadDataList[i].unitSpeed;
    //        unit.attackRange = loadDataList[i].attackRange;
    //    }
    //}

}