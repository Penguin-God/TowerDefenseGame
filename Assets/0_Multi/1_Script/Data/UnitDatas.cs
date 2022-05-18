using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct UnitNumber : IEquatable<UnitNumber>
{
    [SerializeField] int _colorNumber;
    [SerializeField] int _classNumber;
    
    public UnitNumber(int colorNum, int classNum)
    {
        _colorNumber = colorNum;
        _classNumber = classNum;
    }

    public int ColorNumber => _colorNumber;
    public int ClassNumber => _classNumber;
    public UnitColor UnitColor => (UnitColor)_colorNumber;
    public UnitClass UnitClass => (UnitClass)_classNumber;

    public bool Equals(UnitNumber other) 
        => other.ColorNumber == _colorNumber && other.ClassNumber == _classNumber;

    public override int GetHashCode() => (_colorNumber, _classNumber).GetHashCode();
    public override bool Equals(object other) => base.Equals(other);
}

[Serializable]
public class UnitNumbers
{
    public List<UnitNumber> unitNumbers = new List<UnitNumber>();
}

[Serializable]
public struct CombineData
{
    [SerializeField] UnitNumber _unitNumber;
    [SerializeField] string _name;
    [SerializeField] string _koearName;

    public CombineData(int colorNum, int classNum, string name, string koearName)
    {
        _unitNumber = new UnitNumber(colorNum, classNum);
        _name = name;
        _koearName = koearName;
    }

    public UnitNumber UnitNumber => _unitNumber;
    public string Name => _name;
    public string KoearName => _koearName;
}

[Serializable]
public class CombineDatas : ILoader<UnitNumber, CombineData>
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
        }
    }

    public Dictionary<UnitNumber, CombineData> MakeDict()
    {
        Dictionary<UnitNumber, CombineData> dict = new Dictionary<UnitNumber, CombineData>();

        foreach (CombineData data in combineDatas)
            dict.Add(data.UnitNumber, data);
        return dict;
    }
}