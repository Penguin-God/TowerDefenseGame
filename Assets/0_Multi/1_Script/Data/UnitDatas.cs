using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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

    public UnitFlags(UnitColor unitColor, UnitClass unitClass)
    {
        _colorNumber = (int)unitColor;
        _classNumber = (int)unitClass;
    }

    public UnitFlags(string unitColor, string unitClass)
    {
        Int32.TryParse(unitColor, out _colorNumber);
        Int32.TryParse(unitClass, out _classNumber);
        ClempFlags();
    }

    void ClempFlags()
    {
        Debug.Assert(_colorNumber >= 0 && _colorNumber <= GetEnumCount(typeof(UnitColor)), $"color number 입력 잘못됨 : {_colorNumber}");
        Debug.Assert(_classNumber >= 0 && _classNumber <= GetEnumCount(typeof(UnitClass)), $"class number 입력 잘못됨 : {_classNumber}");

        _colorNumber = Mathf.Clamp(_colorNumber, 0, GetEnumCount(typeof(UnitColor)));
        _classNumber = Mathf.Clamp(_classNumber, 0, GetEnumCount(typeof(UnitClass)));
    }
    int GetEnumCount(Type t) => Enum.GetValues(t).Length - 1;
    public bool IsRange() => _colorNumber >= 0 && _colorNumber <= GetEnumCount(typeof(UnitColor))
                            && _classNumber >= 0 && _classNumber <= GetEnumCount(typeof(UnitClass));

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
public struct CombineCondition
{
    [SerializeField] UnitFlags _targetUnitFlag;
    [SerializeField] UnitFlags[] _unitFlags; // 조합에 필요한 유닛의 플래그
    [SerializeField] int[] _counts;
    List<KeyValuePair<UnitFlags, int>> _unitFlagsCountPair;

    public UnitFlags TargetUnitFlags => _targetUnitFlag;
    public IReadOnlyList<KeyValuePair<UnitFlags, int>> UnitFlagsCountPair
    {
        get
        {
            _unitFlagsCountPair = new List<KeyValuePair<UnitFlags, int>>();
            for (int i = 0; i < _unitFlags.Length; i++)
            {
                if (_unitFlags[i].IsRange() && _counts[i] > 0)
                    _unitFlagsCountPair.Add(new KeyValuePair<UnitFlags, int>(_unitFlags[i], _counts[i]));
            }
            return _unitFlagsCountPair;
        }
    }
}

[Serializable]
public class CombineConditions : ICsvLoader<UnitFlags, CombineCondition>
{
    public Dictionary<UnitFlags, CombineCondition> MakeDict(string csv)
    {
        return CsvUtility.GetEnumerableFromCsv<CombineCondition>(csv).ToDictionary(x => x.TargetUnitFlags, x => x);
    }
}

[Serializable]
public struct CombineData
{
    [SerializeField] UnitFlags _unitFlags; // 조합하려는 유닛의 플래그
    [SerializeField] string _koearName;
    
    public CombineData(UnitFlags unitFlags, string koreaName)
    {
        _unitFlags = unitFlags;
        _koearName = koreaName;
    }

    public UnitFlags UnitFlags => _unitFlags;
    public string KoearName => _koearName;
    public CombineCondition Condition => Multi_Managers.Data.CombineConditionByUnitFalg[_unitFlags];
}

[Serializable]
public class CombineDatas : ICsvLoader<UnitFlags, CombineData>
{
    public Dictionary<UnitFlags, CombineData> MakeDict(string csv)
    {
        return CsvUtility.GetEnumerableFromCsv<CombineData>(csv).ToDictionary(x => x.UnitFlags, x => x);
    }
}


[Serializable]
public struct UnitNameData
{
    [SerializeField] UnitFlags _UnitFlags;
    [SerializeField] string _prefabName;
    [SerializeField] string _koearName;
    
    public UnitNameData(int colorNum, int classNum, string prefabName, string koearName)
    {
        _UnitFlags = new UnitFlags(colorNum, classNum);
        _prefabName = prefabName;
        _koearName = koearName;
    }

    public UnitFlags UnitFlags => _UnitFlags;
    public string Name => _prefabName;
    public string KoearName => _koearName;
}

[Serializable]
public class UnitNameDatas : ICsvLoader<string, UnitNameData>
{
    public Dictionary<string, UnitNameData> MakeDict(string csv)
    {
        return CsvUtility.GetEnumerableFromCsv<UnitNameData>(csv).ToDictionary(x => x.KoearName, x => x);
    }
}


[Serializable]
public struct WeaponData
{
    [SerializeField] UnitFlags flag;
    [SerializeField] string[] paths;

    public IReadOnlyList<string> Paths => paths;
    public UnitFlags Flag => flag;
}

[Serializable]
public class WeaponDatas : ICsvLoader<UnitFlags, WeaponData>
{
    public Dictionary<UnitFlags, WeaponData> MakeDict(string csv) => CsvUtility.GetEnumerableFromCsv<WeaponData>(csv).ToDictionary(x => x.Flag, x => x);
}