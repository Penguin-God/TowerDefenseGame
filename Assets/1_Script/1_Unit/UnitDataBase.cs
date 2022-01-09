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
    public UnitData(string name, int damage, float speed, float range) { unitName = name; unitDamage = damage; unitSpeed = speed; attackRange = range; }

    public string unitName;
    public int unitDamage;
    public float unitSpeed;
    public float attackRange;
}

[Serializable]
public struct PassiveData
{
    public string name;
    public float p1;
    public float p2;
    public float p3;

    public PassiveData(string _name, float _p1, float _p2, float _p3)
    {
        name = _name;
        p1 = _p1;
        p2 = _p2;
        p3 = _p3;
    }
}


[System.Serializable]
public struct UnitStruct
{
    public string name;
    public TeamSoldier unit;

    public UnitStruct(string _name, TeamSoldier _unit)
    {
        name = _name;
        unit = _unit;
    }
}

[System.Serializable]
public struct UnitPassiveStruct
{
    public string name;
    public UnitPassive unitPassive;

    public UnitPassiveStruct(string _name, UnitPassive _unitPassive)
    {
        name = _name;
        unitPassive = _unitPassive;
    }
}

public class UnitDataList<T>
{
    public UnitDataList(List<T> p_List) => dataList = p_List;
    public List<T> dataList;
}

public class UnitDataBase : MonoBehaviour
{
    private void Awake()
    {
        //SetJson();
        LoadJson();
        SetUnitDataDictionary();

        //SetPassiveJson();
        LoadPassiveJson();
        SetPassiveDictionary();

        //SetUnitDictionary();

        //ApplyUnitData();

        //ApplyPassiveData();
    }

    [SerializeField] List<UnitStruct> unitStructs = new List<UnitStruct>();
    // ContextMenu 에서 설정한 변수들은 인스펙터 창에 노출되지 않으면 런타임 시작 시 초기화되서 사용 불가능
    Dictionary<string, TeamSoldier> unitDic = new Dictionary<string, TeamSoldier>();

    [ContextMenu("SetUnitStructs")]
    void SetUnitStructs()
    {
        unitStructs.Clear();

        string csvText = unitData_CSV.text.Substring(0, unitData_CSV.text.Length - 1);
        string[] datas = csvText.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음
        for (int i = 1; i < datas.Length; i++)
        {
            string[] cells = datas[i].Split(',');
            TeamSoldier unit = unitTeamList.Find(TeamSoldier => TeamSoldier.gameObject.tag == cells[0].Trim());
            if (unit != null)
            {
                string name = cells[0];
                UnitStruct unitStruct = new UnitStruct(cells[0], unit);
                unitStructs.Add(unitStruct);
            }
            else Debug.Log($"NONE : {cells[0]}");
        }
    }


    [SerializeField] List<UnitPassiveStruct> unitPassiveStructs = new List<UnitPassiveStruct>();
    [ContextMenu("SetUnitPassiveStructs")]
    void SetUnitPassiveStructs()
    {
        unitPassiveStructs.Clear();

        string csvText = unitData_CSV.text.Substring(0, unitData_CSV.text.Length - 1);
        string[] datas = csvText.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음
        for (int i = 1; i < datas.Length; i++)
        {
            string[] cells = datas[i].Split(',');
            UnitPassive unitPassive = null;
            if (unitTeamList.Find(TeamSoldier => TeamSoldier.gameObject.tag == cells[0].Trim()))
                unitPassive = unitTeamList.Find(TeamSoldier => TeamSoldier.gameObject.tag == cells[0].Trim()).gameObject.GetComponent<UnitPassive>();

            if (unitPassive != null)
            {
                string name = cells[0];
                UnitPassiveStruct unitPassiveSturct = new UnitPassiveStruct(cells[0], unitPassive);
                unitPassiveStructs.Add(unitPassiveSturct);
            }
            else Debug.Log($"NONE : {cells[0]}");
        }
    }

    [SerializeField] List<TeamSoldier> unitTeamList = new List<TeamSoldier>();
    [ContextMenu("SetUnitTeamList")]
    void SetUnitTeamList()
    {
        unitTeamList.Clear();
        for (int i = 0; i < unitList.Count; i++) unitTeamList.Add(unitList[i].GetComponentInChildren<TeamSoldier>());
    }


    [SerializeField] TextAsset unitData_CSV;

    void SetUnitDictionary()
    {
        for (int i = 0; i < unitStructs.Count; i++)
            unitDic.Add(unitStructs[i].name, unitStructs[i].unit);
    }

    void ApplyUnitData()
    {
        string csvText = unitData_CSV.text.Substring(0, unitData_CSV.text.Length - 1);
        string[] datas = csvText.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음

        for(int i = 1; i < datas.Length; i++)
        {
            string[] cells = datas[i].Split(',');
            unitDic.TryGetValue(cells[0].Trim(), out TeamSoldier unit);

            if (unit != null)
            {
                unit.originDamage = Int32.Parse(cells[1]);
                unit.damage = Int32.Parse(cells[1]);
                unit.originBossDamage = Int32.Parse(cells[1]);
                unit.bossDamage = Int32.Parse(cells[1]);

                int speed = Int32.Parse(cells[3]);
                int attackRange = Int32.Parse(cells[4]);
                unit.speed = speed;
                unit.attackRange = attackRange;

            }
            else Debug.Log($"NONE : {cells[0]}");
        }
    }


    [SerializeField] TextAsset Csv_UnitPassivedata = null;
    void ApplyPassiveData()
    {
        Dictionary<string, UnitPassive> unitPassiveDic = new Dictionary<string, UnitPassive>();
        for (int i = 0; i < unitPassiveStructs.Count; i++) unitPassiveDic.Add(unitPassiveStructs[i].name, unitPassiveStructs[i].unitPassive);


        string csvText = Csv_UnitPassivedata.text.Substring(0, Csv_UnitPassivedata.text.Length - 1);
        string[] datas = csvText.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음

        for (int i = 1; i < datas.Length; i++)
        {
            string[] cells = datas[i].Split(',');
            unitPassiveDic.TryGetValue(cells[0], out UnitPassive unitPassive);

            if (unitPassive != null)
            {
                float[] passive_datas = new float[3];
                for (int data = 0; data < passive_datas.Length; data++)
                {
                    if (cells[data + 1].Trim() != "") passive_datas[data] = float.Parse(cells[data + 1]);
                    else passive_datas[data] = 0;
                }

                unitPassive.ApplyData(passive_datas[0], passive_datas[1], passive_datas[2]);
            }
            else Debug.Log($"NONE : {cells[0]}");
        }
    }

    [ContextMenu("WrietPassiveCSV")]
    void WritePassiveCSV()
    {
        List<string[]> LowList = new List<string[]>();
        SetUnitDictionary();

        string[] unitData = new string[4];
        unitData[0] = "Name";
        unitData[1] = "P1";
        unitData[2] = "P2";
        unitData[3] = "P3";
        LowList.Add(unitData);

        //string[] unitColors = new string[6] {"Red", "Blue", "Yellow", "Green", "Orange", "Violet"};
        //string[] unitClass = new string[4] { "Swordman", "Archer", "Spearman", "Mage" };
        //for (int i = 0; i < unitClass.Length; i++)
        //{
        //    for (int j = 0; j < unitColors.Length; j++)
        //    {
        //        string unitName = unitColors[j] + unitClass[i];
        //        string[] test_names = new string[1] { unitName };
        //        if (unitDic.ContainsKey(unitName)) LowList.Add(test_names);
        //    }
        //}

        for (int i = 0; i < unitDataList.Count; i++)
        {
            unitData = new string[1];
            string[] names = unitDataList[i].unitName.Split('_');
            if (names[1] == "SwordMan") names[1] = "Swordman";
            string name = names[0] + names[1];
            unitData[0] = name;
            LowList.Add(unitData);
        }

        string delimiter = ",";
        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < LowList.Count; index++)
        {
            sb.AppendLine(string.Join(delimiter, LowList[index]));
        }

        string filePath = "Assets/4_Data/UnitPassiveData.csv";

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    [ContextMenu("WrietCSV")]
    void WriteCSV()
    {
        List<string[]> LowList = new List<string[]>();

        string[] unitData = new string[5];
        unitData[0] = "Name";
        unitData[1] = "Damage";
        unitData[2] = "BossDamage";
        unitData[3] = "Speed";
        unitData[4] = "attackRange";
        LowList.Add(unitData);

        for (int i = 0; i < unitDataList.Count; i++)
        {
            unitData = new string[5];
            string[] names = unitDataList[i].unitName.Split('_');
            if (names[1] == "SwordMan") names[1] = "Swordman";
            string name = names[0] + names[1];
            unitData[0] = name;
            unitData[1] = unitDataList[i].unitDamage.ToString();
            unitData[2] = unitDataList[i].unitDamage.ToString();
            unitData[3] = unitDataList[i].unitSpeed.ToString();
            unitData[4] = unitDataList[i].attackRange.ToString();
            LowList.Add(unitData);
        }

        string delimiter = ",";
        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < LowList.Count; index++)
        {
            sb.AppendLine(string.Join(delimiter, LowList[index]));
        }

        string filePath = "Assets/4_Data/UnitData.csv";

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

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



    public List<GameObject> unitList = new List<GameObject>();


    [ContextMenu("SetList")]
    void SetUnitList()
    {
        unitList.Clear();
        UnitManager unitManager = GetComponent<UnitManager>();
        for (int i = 0; i < unitManager.unitArrays.Length; i++)
        {
            for (int k = 0; k < unitManager.unitArrays[i].unitArray.Length; k++)
            {
                unitList.Add(unitManager.unitArrays[i].unitArray[k]);
            }
        }
    }

    public List<UnitData> unitDataList;
    [ContextMenu("SaveJson")]
    void SetJson()
    {
        SetUnitDictionary();
        unitDataList = new List<UnitData>();

        string csvText = unitData_CSV.text.Substring(0, unitData_CSV.text.Length - 1);
        string[] datas = csvText.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음

        for (int i = 1; i < datas.Length; i++)
        {
            string[] cells = datas[i].Split(',');
            if (unitDic.ContainsKey(cells[0]))
            {
                string _name = cells[0];
                int _damage = Int32.Parse(cells[1]);
                int _speed = Int32.Parse(cells[3]);
                int _attackRange = Int32.Parse(cells[4]);
                unitDataList.Add(new UnitData(_name, _damage, _speed, _attackRange));
            }
            else Debug.Log($"NONE : {cells[0]}");
        }

        string jsonData = JsonUtility.ToJson(new UnitDataList<UnitData>(unitDataList), true);
        string path = Path.Combine(Application.dataPath, "4_Data", "unitData.txt");
        File.WriteAllText(path, jsonData);
    }

    [SerializeField] List<UnitData> loadDataList;
    [ContextMenu("Test")]
    void LoadJson()
    {
        loadDataList = new List<UnitData>();
        string path = Path.Combine(Application.dataPath, "4_Data", "unitData.txt");
        string jsonData = File.ReadAllText(path);
        loadDataList = JsonUtility.FromJson<UnitDataList<UnitData>>(jsonData).dataList;
    }

    private Dictionary<string, UnitData> UnitDataDictionary;
    void SetUnitDataDictionary()
    {
        UnitDataDictionary = new Dictionary<string, UnitData>();

        for (int i = 0; i < loadDataList.Count; i++)
        {
            UnitData _data = new UnitData(loadDataList[i].unitName, loadDataList[i].unitDamage, loadDataList[i].unitSpeed, loadDataList[i].attackRange);
            UnitDataDictionary.Add(loadDataList[i].unitName, _data);
        }
    }


    public void ApplyData(string _name, TeamSoldier _team)
    {
        _team.originDamage = UnitDataDictionary[_name].unitDamage;
        _team.damage = UnitDataDictionary[_name].unitDamage;
        _team.originBossDamage = UnitDataDictionary[_name].unitDamage;
        _team.bossDamage = UnitDataDictionary[_name].unitDamage;
        _team.speed = UnitDataDictionary[_name].unitSpeed;
        _team.attackRange = UnitDataDictionary[_name].attackRange;
    }

    public void ChangeLoadData(string _key, UnitData _data)
    {
        UnitDataDictionary[_key].unitDamage = _data.unitDamage;
        UnitDataDictionary[_key].unitSpeed = _data.unitSpeed;
        UnitDataDictionary[_key].attackRange = _data.attackRange;
    }






    
    public List<PassiveData> passiveDataList;
    public Dictionary<string, PassiveData> passiveDictionary;
    [ContextMenu("SavePassiveJson")]
    void SetPassiveJson()
    {
        SetUnitDictionary();
        passiveDataList = new List<PassiveData>();

        string csvText = Csv_UnitPassivedata.text.Substring(0, Csv_UnitPassivedata.text.Length - 1);
        string[] datas = csvText.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음

        for (int i = 1; i < datas.Length; i++)
        {
            string[] cells = datas[i].Split(',');
            if (unitDic.ContainsKey(cells[0]))
            {
                string _name = cells[0];
                // 패시브는 공백인 경우가 있어서 삼항연산자 사용함
                float _p1 = (cells[1].Trim() != "") ? float.Parse(cells[1]) : 0;
                float _p2 = (cells[2].Trim() != "") ? float.Parse(cells[2]) : 0;
                float _p3 = (cells[3].Trim() != "") ? float.Parse(cells[3]) : 0;

                passiveDataList.Add(new PassiveData(_name, _p1, _p2, _p3));
            }
            else Debug.Log($"NONE : {cells[0]}");
        }

        string jsonData = JsonUtility.ToJson(new UnitDataList<PassiveData>(passiveDataList), true);
        string path = Path.Combine(Application.dataPath, "4_Data", "UnitPassiveData.txt");
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("Test2")]
    void LoadPassiveJson()
    {
        passiveDataList = new List<PassiveData>();
        string path = Path.Combine(Application.dataPath, "4_Data", "UnitPassiveData.txt");
        string jsonData = File.ReadAllText(path);
        passiveDataList = JsonUtility.FromJson<UnitDataList<PassiveData>>(jsonData).dataList;
    }

    void SetPassiveDictionary()
    {
        passiveDictionary = new Dictionary<string, PassiveData>();

        for (int i = 0; i < passiveDataList.Count; i++)
        {
            PassiveData _data = new PassiveData(passiveDataList[i].name, passiveDataList[i].p1, passiveDataList[i].p2, passiveDataList[i].p3);
            passiveDictionary.Add(passiveDataList[i].name, _data);
        }
    }

    public float[] GetPassiveData(string _key)
    {
        float[] passive_datas = new float[3];
        passive_datas[0] = passiveDictionary[_key].p1;
        passive_datas[1] = passiveDictionary[_key].p2;
        passive_datas[2] = passiveDictionary[_key].p3;
        return passive_datas;
    }

    public void ChangeLoadPassiveData(string _key, PassiveData _data)
    {
        passiveDictionary[_key] = new PassiveData(passiveDictionary[_key].name, _data.p1, _data.p2, _data.p3);
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