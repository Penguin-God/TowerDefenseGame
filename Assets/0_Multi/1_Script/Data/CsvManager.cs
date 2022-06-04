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
    [SerializeField] List<Tests> testList;

    [ContextMenu("Read Test")]
    void Test()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/Test/Test");
        Tests tests = ToCsv<Tests>(textAsset.text);
        print(
            tests.number == 3 &&
            Mathf.Abs(tests.numberFloat - 4.5f) < 0.1f &&
            tests.text == "Hello Csv"
            );

        testList = FromCsvList<Tests>(textAsset.text);
        Debug.Assert(testList[0].number == 3, "값 오류");
        Debug.Assert(testList[0].text == "Hello Csv", "값 오류");
        Debug.Assert(testList[1].number == 2, "값 오류");
        Debug.Assert(testList[1].text == "Hello Line 2", "값 오류");
        print("성공!!");
    }

    void ToCsv(object obj)
    {
        
    }

    


    List<T> FromCsvList<T>(string csv)
    {
        csv = csv.Substring(0, csv.Length - 1);
        Dictionary<string, int> indexByKey = new Dictionary<string, int>();

        string[] colums = csv.Split('\n');
        string[] keys = colums[0].Split(',').Select(x => x.Trim()).ToArray();
        SetKeyIndex();

        List<T> result = new List<T>();
        for (int i = 1; i < colums.Length; i++)
        {
            object t = Activator.CreateInstance(typeof(T));
            SetFiledValue(t, colums[i].Split(',').Select(x => x.Trim()).ToArray());
            result.Add((T)t);
        }
        return result;

        void SetKeyIndex()
        {
            for (int i = 0; i < keys.Length; i++)
                indexByKey.Add(keys[i], i);
        }
        void SetFiledValue(object t, string[] values)
        {
            foreach (FieldInfo info in t.GetType().GetFields().Where(x => x.IsPublic && keys.Contains(x.Name)))
            {
                indexByKey.TryGetValue(info.Name, out int valueIndex);
                SetValue(t, info, values[valueIndex]);
            }
        }
    }

    // 파싱 실패 시 예외처리하기
    void SetValue(object t, FieldInfo info, string value)
    {
        switch (info.FieldType.ToString())
        {
            case "System.Int32":
                Int32.TryParse(value, out int valueInt);
                info.SetValue(t, valueInt);
                break;
            case "System.Single":
                float.TryParse(value, out float valueFloat);
                info.SetValue(t, valueFloat);
                break;
            case "System.String":
                info.SetValue(t, value);
                break;
            default: break;
        }
    }


    #region 레거시 코드
    // [ContextMenu("test tocsv")]
    void TestToCsv()
    {
        Tests a = ToCsv<Tests>(Resources.Load<TextAsset>("Data/Test/Test").text);
        print(a.number);
        print(a.numberFloat);
        print(a.text.Trim());
    }

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
