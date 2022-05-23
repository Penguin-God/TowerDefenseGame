using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct UnitFlags : IEquatable<UnitFlags>
{
    [SerializeField] int _colorNumber;
    [SerializeField] int _classNumber;
    
    public UnitFlags(int colorNum, int classNum)
    {
        _colorNumber = colorNum;
        _classNumber = classNum;
    }

    public int ColorNumber => _colorNumber;
    public int ClassNumber => _classNumber;
    public UnitColor UnitColor => (UnitColor)_colorNumber;
    public UnitClass UnitClass => (UnitClass)_classNumber;

    public bool Equals(UnitFlags other) 
        => other.ColorNumber == _colorNumber && other.ClassNumber == _classNumber;

    public override int GetHashCode() => (_colorNumber, _classNumber).GetHashCode();
    public override bool Equals(object other) => base.Equals(other);

    public static bool operator ==(UnitFlags lhs, UnitFlags rhs) 
        => lhs.ColorNumber == rhs.ColorNumber && lhs.ClassNumber == rhs.ClassNumber;
    public static bool operator !=(UnitFlags lhs, UnitFlags rhs) => !(lhs == rhs);
}

[Serializable]
public struct CombineData
{
    [SerializeField] UnitFlags _UnitFlags;
    [SerializeField] string _name;
    [SerializeField] string _koearName;

    public CombineData(int colorNum, int classNum, string name, string koearName)
    {
        _UnitFlags = new UnitFlags(colorNum, classNum);
        _name = name;
        _koearName = koearName;
    }

    public UnitFlags UnitFlags => _UnitFlags;
    public string Name => _name;
    public string KoearName => _koearName;
}

[Serializable]
public class CombineDatas : ILoader<UnitFlags, CombineData>
{
    public List<CombineData> SerializtionDatas = new List<CombineData>();

    public void LoadCSV(TextAsset csv)
    {
        string csvText = csv.text.Substring(0, csv.text.Length - 1);
        string[] rows = csvText.Split('\n');

        for (int i = 1; i < rows.Length; i++)
        {
            string[] cells = rows[i].Split(',');
            SerializtionDatas.Add( new CombineData(int.Parse(cells[0]), int.Parse(cells[1]), cells[2], cells[3]));
        }
    }

    public Dictionary<UnitFlags, CombineData> MakeDict()
    {
        Dictionary<UnitFlags, CombineData> dict = new Dictionary<UnitFlags, CombineData>();

        foreach (CombineData data in SerializtionDatas)
            dict.Add(data.UnitFlags, data);
        return dict;
    }
}