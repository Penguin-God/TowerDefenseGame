using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

[Serializable]
class Tests
{
    public int number;
    public float numberFloat;
    public string text;
}

public class CsvManager : MonoBehaviour
{
    [ContextMenu("Read Test")]
    void Test()
    {
        Resources.Load<TextAsset>("Data/Test/Test.");
        Tests tests = ToCsv<Tests>("");
        print(
            tests.number == 3 &&
            Mathf.Abs(tests.numberFloat - 4.5f) < 0.1f &&
            tests.text == "Hello Csv"
            );
    }

    void FromCsv(object obj)
    {
        
    }

    [ContextMenu("test tocsv")]
    void TestToCsv()
    {
        Tests a = ToCsv<Tests>(Resources.Load<TextAsset>("Data/Test/Test").text);
        print(a.number);
        print(a.numberFloat);
        print(a.text);
    }

    T ToCsv<T>(string csv) where T : class
    {
        // TODO : 이 딕셔너리를 csv의 줄 수만큼 만들어서 List안에 넣기
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

        return t as T;
    }

    void SetValueByKey(Dictionary<string, string> vlaueByKey, string[] keys, string[] values)
    {
        for (int i = 0; i < keys.Length; i++)
            vlaueByKey.Add(keys[i], values[i]);
    }

    void SetFiledValue(Dictionary<string, string> vlaueByKey, string[] keys, object t, Type type)
    {
        foreach (FieldInfo info in type.GetFields().Where(x => x.IsPublic && keys.Contains(x.Name)).ToArray())
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
}
