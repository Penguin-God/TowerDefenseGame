using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;

[Serializable]
class Tests
{
    public int number;
    public float numberFloat;
    [SerializeField] string text;
    [SerializeField] bool isCheck;
    [SerializeField] UnitFlags flag;
    [SerializeField] int[] numbers;
    [SerializeField] string test;

    public string Text => text;
    public bool IsCheck => isCheck;
}

public class CsvManager : MonoBehaviour
{
    [ContextMenu("Type Test")]
    void TypeTest()
    {
        //GetEnumerableFromCsvTT(Resources.Load<TextAsset>("Data/Test/Test").text);

        //foreach (FieldInfo info in GetSerializedFields(new Tests()))
        //    print(CsvParsers.GetTypeName(info));
    }

    [SerializeField] List<Tests> testList;
    [ContextMenu("Read Test")]
    void Test()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/Test/Test");
        testList = GetEnumerableFromCsv<Tests>(textAsset.text).ToList();
    }


    [SerializeField, TextArea] string output;
    [ContextMenu("Write Test")]
    void TestToCsv()
    {
        output = EnumerableToCsv(testList);
        SaveCsv(EnumerableToCsv(testList), "Test");
    }

    string SubLastLine(string text) => text.Substring(0, text.Length - 1);

    string EnumerableToCsv<T>(IEnumerable<T> datas)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(string.Join(",", GetSerializedFields(datas.First()).Select(x => x.Name)));

        foreach (var data in datas)
        {
            IEnumerable<string> values = GetSerializedFields(data).Select(x => x.GetValue(data).ToString());
            stringBuilder.AppendLine(string.Join(",", values));
        }

        return SubLastLine(stringBuilder.ToString());
    }

    void SaveCsv(string text, string fileName)
    {
        Stream fileStream = new FileStream($"Assets/0_Multi/Resources/Data/Test/{fileName}.csv", FileMode.Create, FileAccess.Write);
        StreamWriter outStream = new StreamWriter(fileStream, Encoding.UTF8);
        outStream.Write(text);
        outStream.Close();
    }


    // Load Csv
    IEnumerable<T> GetEnumerableFromCsv<T>(string csv)
    {
        string[] columns = SubLastLine(csv).Split('\n');
        Dictionary<string, int> indexByFieldName = SetDict();

        return columns.Skip(1)
                      .Select(x => (T)SetFiledValue(Activator.CreateInstance<T>(), GetCells(x)));

        object SetFiledValue(object obj, string[] values)
        {
            foreach (FieldInfo info in GetSerializedFields())
                SetValue(obj, info, values[indexByFieldName[info.Name]]);
            return obj;

            // 중첩 함수
            IEnumerable<FieldInfo> GetSerializedFields() => this.GetSerializedFields(obj).Where(x => indexByFieldName.ContainsKey(x.Name)); 
        }

        string[] GetCells(string column) => column.Split(',').Select(x => x.Trim()).ToArray();
        Dictionary<string, int> SetDict()
        {
            int index = 0;
            int overlap = 0;
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (string key in GetCells(columns[0]))
            {
                if (result.ContainsKey(key))
                {
                    overlap++;
                    result.Add($"{key}{overlap}", index);
                    print($"{key}{overlap}");
                }
                else
                {
                    result.Add(key, index);
                    overlap = 0;
                    //print(key);
                }
                index++;
            }
            return result;
        }
    }

    IEnumerable<T> GetEnumerableFromCsvTT<T>(string csv)
    {
        string[] columns = SubLastLine(csv).Split('\n');
        Dictionary<string, int[]> indexsByFieldName = SetDict();

        return columns.Skip(1)
                      .Select(x => (T)SetFiledValue(Activator.CreateInstance<T>(), GetCells(x)));

        object SetFiledValue(object obj, string[] values)
        {
            foreach (FieldInfo info in GetSerializedFields())
                SetValue(obj, info, GetValues(info));
            return obj;

            // 중첩 함수
            IEnumerable<FieldInfo> GetSerializedFields() => this.GetSerializedFields(obj).Where(x => indexsByFieldName.ContainsKey(x.Name));
            string[] GetValues(FieldInfo info) => indexsByFieldName[info.Name].Select(x => GetCells(columns[2])[x]).ToArray();
        }

        string[] GetCells(string column) => column.Split(',').Select(x => x.Trim()).ToArray();
         
        Dictionary<string, int[]> SetDict()
        {
            return GetCells(columns[0]).Distinct().ToDictionary(x => x, x => GetIndexs(x));

            int[] GetIndexs(string key)
            {
                int[] result = GetCells(columns[0]).Where(cell => cell == key)
                                                   .Select(cell => Array.IndexOf(GetCells(columns[0]), cell))
                                                   .ToArray();
                List<int> upValueIndexs = new List<int>();
                for (int i = 1; i < result.Length; i++)
                {
                    if (result[i] == result[i - 1])
                        upValueIndexs.Add(i);
                }
                upValueIndexs.ForEach(x => result[x] = result[x-1] + 1);
                return result;
            }
        }
    }

    IEnumerable<FieldInfo> GetSerializedFields(object obj)
        => obj.GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Where(x => CsvSerializedCondition(x));

    bool CsvSerializedCondition(FieldInfo info) => info.IsPublic || info.GetCustomAttribute(typeof(SerializeField)) != null;

    void SetValue(object obj, FieldInfo info, string value) => CsvParsers.GetParser(info).SetValue(obj, info, value);
    void SetValue(object obj, FieldInfo info, string[] values) => CsvParsers.GetParser(info).SetValue(obj, info, values);

    #region 레거시 코드
    T ToCsv<T>(string csv)
    {
        Dictionary<string, string> vlaueByKey = new Dictionary<string, string>();

        string[] colums = csv.Split('\n');
        string[] keys = colums[0].Split(',');
        string[] values = colums[1].Split(',');
        keys = keys.Select(x => x.Trim()).ToArray();
        values = values.Select(x => x.Trim()).ToArray();

        SetValueByKey(vlaueByKey, keys, values);

        object t = Activator.CreateInstance(typeof(T));
        Type type = t.GetType();
        SetFiledValue(vlaueByKey, keys, t, type);

        return (T)t;
    }

    void SetValueByKey(Dictionary<string, string> vlaueByKey, string[] keys, string[] values)
    {
        for (int i = 0; i < keys.Length; i++)
            vlaueByKey.Add(keys[i], values[i]);
    }

    void SetFiledValue(Dictionary<string, string> vlaueByKey, string[] keys, object t, Type type)
    {
        foreach (FieldInfo info in type.GetFields().Where(x => x.IsPublic && keys.Contains(x.Name)))
        {
            vlaueByKey.TryGetValue(info.Name, out string previousValue);

            switch (info.FieldType.ToString())
            {
                case "System.Int32":
                    Int32.TryParse(previousValue, out int valueInt);
                    info.SetValue(t, valueInt);
                    break;
                case "System.Single":
                    float.TryParse(previousValue, out float valueFloat);
                    info.SetValue(t, valueFloat);
                    break;
                case "System.String":
                    info.SetValue(t, previousValue);
                    break;
                default: break;
            }
        }
    }
    #endregion 레거시 코드
}
