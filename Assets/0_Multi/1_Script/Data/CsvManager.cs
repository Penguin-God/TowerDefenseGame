using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;

[Serializable]
public class Tests
{
    public KeyValuePair<int, string> pair;
    public Dictionary<UnitFlags, int> dict;
    public float[] list;
    public void printData()
    {
        Debug.Log($"pair : {pair.Key} :: {pair.Value}");
        Debug.Log("========================================");

        foreach (var item in dict)
        {
            Debug.Log($"{item.Key.ColorNumber} {item.Key.ClassNumber} :: {item.Value}");
        }
    }
}



public class CsvManager : MonoBehaviour
{
    [SerializeField] UI_UnitWindowData[] datas;
    [ContextMenu("Test")]
    void UnitTest()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/UI_UnitWindowData");
        datas = CsvUtility.GetEnumerableFromCsv<UI_UnitWindowData>(textAsset.text).ToArray();
    }

    [SerializeField] List<Tests> tests = new List<Tests>();
    void Test()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/Test/Test");
        tests = CsvUtility.GetEnumerableFromCsv<Tests>(textAsset.text).ToList();


        Dictionary<UnitFlags, int> dict = new Dictionary<UnitFlags, int>();
        //print(dict.GetType().ToString().GetMiddleString("[", "]"));
        //new CsvListParser().SetValue(tests[0], tests[0].GetType().GetField("list"), new string[] {"11","22","33" }, "Int32");

        //object a = 22;
        //object b = "안녕세상";
        //Type aaa = new KeyValuePair<int, string>(1, "22").GetType();
        //ConstructorInfo[] infos = aaa.GetConstructors();
        ////print($"{pair.Key}, {pair.Value}");

        //tests.GetType().GetField("pair").SetValue(tests, infos[0].Invoke(new object[] { a, b }));
        //infos.ToList().ForEach(x => print(x.Name));
        //print($"{tests.pair.Key} : {tests.pair.Value}");
    }
}
