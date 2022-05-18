using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct CombineData
{
    [SerializeField] int _colorNum;
    [SerializeField] int _classNum;
    [SerializeField] string id;
    [SerializeField] string _koearName;

    public CombineData(int colorNum, int classNum, string name, string koearName)
    {
        _colorNum = colorNum;
        _classNum = classNum;
        id = name;
        _koearName = koearName;
    }

    public int ColorNum => _colorNum;
    public int ClassNum => _classNum;
    public string ID => id;
    public string KoearName => _koearName;
}

[Serializable]
public class CombineDatas : ILoader<string, CombineData>
{
    public List<CombineData> combineDatas = new List<CombineData>();

    public void LoadCSV(TextAsset csv)
    {
        string csvText = csv.text.Substring(0, csv.text.Length - 1);
        string[] rows = csvText.Split('\n');

        for (int i = 1; i < rows.Length; i++)
        {
            string[] cells = rows[i].Split(',');
            combineDatas.Add( new CombineData(int.Parse(cells[0]), int.Parse(cells[1]), cells[2], cells[3]));
            //Debug.Log(cells[0]);
            //Debug.Log(cells[1]);
            //Debug.Log(cells[2]);
            //Debug.Log(cells[3]);
        }
    }

    public Dictionary<string, CombineData> MakeDict()
    {
        Dictionary<string, CombineData> dict = new Dictionary<string, CombineData>();

        foreach (CombineData data in combineDatas)
            dict.Add(data.ID, data);
        return dict;
    }
}