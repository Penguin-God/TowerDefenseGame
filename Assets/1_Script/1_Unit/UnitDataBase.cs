using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Linq;

[System.Serializable]
public class UnitData
{
    public UnitData(string name, int damage, float speed, float range) { unitName = name; unitDamage = damage; unitSpeed = speed; attackRange = range; }

    public string unitName;
    public int unitDamage;
    public float unitSpeed;
    public float attackRange;
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


public class UnitDataList<T>
{
    public UnitDataList(List<T> p_List) => dataList = p_List;
    public List<T> dataList;
}

public class UnitDataBase : MonoBehaviour
{
    //private void Awake()
    //{
    //    SetUnitDictionary();
    //    ReadCSV();
    //}

    [SerializeField] List<UnitStruct> unitStructs = new List<UnitStruct>();
    // ContextMenu 에서 설정한 변수들은 인스펙터 창에 노출되지 않으면 런타임 시작 시 초기화되서 사용 불가능
    Dictionary<string, TeamSoldier> unitDic = new Dictionary<string, TeamSoldier>();

    [ContextMenu("SetUnitDictionary")]
    void SetUnitDictionary()
    {
        unitStructs.Clear();
        unitDic.Clear();

        string csvText = unitData_CSV.text.Substring(0, unitData_CSV.text.Length - 1);
        string[] datas = csvText.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음
        for (int i = 1; i < datas.Length; i++)
        {
            string[] cells = datas[i].Split(',');
            // TeamSoldier 타입 가져오기
            TeamSoldier unit = unitTeamList.Find(TeamSoldier => TeamSoldier.gameObject.tag == cells[0].Trim());
            if (unit != null)
            {
                string name = cells[0];
                UnitStruct unitStruct = new UnitStruct(cells[0], unit);
                unitStructs.Add(unitStruct);
                unitDic.Add(cells[0].Trim(), unit);
            }
            else
            {
                Debug.Log("NONE");
            }
        }
    }


    [SerializeField] List<TeamSoldier> unitTeamList = new List<TeamSoldier>();
    [ContextMenu("GetUnit")]
    void GetUnit()
    {
        unitTeamList.AddRange(Resources.FindObjectsOfTypeAll<TeamSoldier>());
    }

    [SerializeField] TextAsset unitData_CSV;
    
    void ReadCSV()
    {
        string csvText = unitData_CSV.text.Substring(0, unitData_CSV.text.Length - 1);
        string[] datas = csvText.Split(new char[] { '\n' }); // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음

        for(int i = 1; i < datas.Length; i++)
        {
            string[] cells = datas[i].Split(',');
            // TeamSoldier 타입 가져오기
            Debug.Log(cells[0].Trim());
            Debug.Log(unitDic.Count);
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

                Debug.Log($"Name : {cells[0]}, Damage : {22} \n ");
            }
            else
            {
                Debug.Log("NONE");
            }
        }
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

    public List<UnitData> unitDataList;
    [ContextMenu("SetList")]
    void SetUnitList()
    {
        for (int i = 0; i < UnitManager.instance.unitArrays.Length; i++)
        {
            for (int k = 0; k < UnitManager.instance.unitArrays[i].unitArray.Length; k++)
            {
                unitList.Add(UnitManager.instance.unitArrays[i].unitArray[k]);
            }
        }
    }

    [ContextMenu("SaveJson")]
    void SetJson()
    {
        unitDataList = new List<UnitData>();
        for (int i = 0; i < unitList.Count; i++)
        {
            TeamSoldier team = unitList[i].GetComponentInChildren<TeamSoldier>();

            unitDataList.Add(new UnitData(unitList[i].name, team.originDamage, team.speed, team.attackRange));
        }

        string jsonData = JsonUtility.ToJson(new UnitDataList<UnitData>(unitDataList), true);
        string path = Path.Combine(Application.dataPath, "4_Data", "unitData.txt");
        File.WriteAllText(path, jsonData);
    }

    public List<UnitData> loadDataList;
    [ContextMenu("LoadJson")]
    void LoadJson()
    {
        loadDataList = new List<UnitData>();
        string path = Path.Combine(Application.dataPath, "4_Data", "unitData.txt");
        string jsonData = File.ReadAllText(path);
        loadDataList = JsonUtility.FromJson<UnitDataList<UnitData>>(jsonData).dataList;
    }

    [ContextMenu("SetUnitData")]
    void SetUnitData()
    {
        int count = Mathf.Min(loadDataList.Count, unitList.Count);
        for(int i = 0; i < count; i++) // TeamSoldier에 데이터 세팅 함수 만들기
        {
            TeamSoldier unit = unitList[i].GetComponentInChildren<TeamSoldier>();
            unit.originDamage = loadDataList[i].unitDamage;
            unit.damage = loadDataList[i].unitDamage;
            unit.originBossDamage = loadDataList[i].unitDamage;
            unit.bossDamage = loadDataList[i].unitDamage;
            unit.speed = loadDataList[i].unitSpeed;
            unit.attackRange = loadDataList[i].attackRange;
        }
    }
}