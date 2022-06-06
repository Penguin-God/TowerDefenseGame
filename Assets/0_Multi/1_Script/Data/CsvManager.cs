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

    public string Text => text;
    public bool IsCheck => isCheck;
}

public class CsvManager : MonoBehaviour
{
    [ContextMenu("Test")]
    void Tests()
    {
        bool daa = true;
        output = daa.ToString();
        print(daa.GetType().ToString());
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
        return columns.Skip(1)
                     .Select(x => (T)SetFiledValue(Activator.CreateInstance<T>(), GetCells(x)));

        object SetFiledValue(object obj, string[] values)
        {
            string[] keys = GetCells(columns[0]);
            Dictionary<string, int> indexByKey = keys.ToDictionary(x => x, x => Array.IndexOf(keys, x));

            foreach (FieldInfo info in GetSerializedFields())
                SetValue(obj, info, values[indexByKey[info.Name]]);
            return obj;

            IEnumerable<FieldInfo> GetSerializedFields() => this.GetSerializedFields(obj).Where(x => keys.Contains(x.Name)); 
        }
        string[] GetCells(string column) => column.Split(',').Select(x => x.Trim()).ToArray();
    }

    IEnumerable<FieldInfo> GetSerializedFields(object obj) 
        => obj.GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Where(x => CsvSerializedCondition(x));
    bool CsvSerializedCondition(FieldInfo info) => info.IsPublic || info.GetCustomAttribute(typeof(SerializeField)) != null;

    void SetValue(object obj, FieldInfo info, string value)
    {
        switch (info.FieldType.ToString())
        {
            case "System.Int32":
                Int32.TryParse(value, out int valueInt);
                info.SetValue(obj, valueInt);
                break;
            case "System.Single":
                float.TryParse(value, out float valueFloat);
                info.SetValue(obj, valueFloat);
                break;
            case "System.Boolean":
                info.SetValue(obj, value == "True" || value == "TRUE");
                break;
            case "System.String":
                info.SetValue(obj, value);
                break;
            default: break;
        }
    }

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
