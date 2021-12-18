using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

[System.Serializable]
public class UnitData
{
    public UnitData(string name, int damage, float speed, float range) { unitName = name; unitDamage = damage; unitSpeed = speed; attackRange = range; }

    public string unitName;
    public int unitDamage;
    public float unitSpeed;
    public float attackRange;
}

public class UnitDataList<T>
{
    public UnitDataList(List<T> p_List) => dataList = p_List;
    public List<T> dataList;
}

public class UnitDataBase : MonoBehaviour
{
    [SerializeField] TextAsset unitData_CSV;

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
            unitData[0] = unitDataList[i].unitName;
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