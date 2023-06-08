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

    public int ColorNumber => _colorNumber;
    public int ClassNumber => _classNumber;
    public UnitColor UnitColor => (UnitColor)_colorNumber;
    public UnitClass UnitClass => (UnitClass)_classNumber;

    public bool Equals(UnitFlags other) => other.ColorNumber == _colorNumber && other.ClassNumber == _classNumber;

    public override int GetHashCode() => (_colorNumber, _classNumber).GetHashCode();
    public override bool Equals(object other) => base.Equals(other);

    public static bool operator ==(UnitFlags lhs, UnitFlags rhs) 
        => lhs.ColorNumber == rhs.ColorNumber && lhs.ClassNumber == rhs.ClassNumber;
    public static bool operator !=(UnitFlags lhs, UnitFlags rhs) => !(lhs == rhs);
    public override string ToString() => $"{ColorNumber} : {ClassNumber}";

    public static UnitFlags RedSowrdman => new UnitFlags(0, 0);
    public static UnitFlags BlueSowrdman => new UnitFlags(1, 0);

    public static IEnumerable<UnitFlags> AllFlags
    {
        get
        {
            List<UnitFlags> result = new List<UnitFlags>();
            foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
            {
                foreach (UnitClass unitClass in Enum.GetValues(typeof(UnitClass)))
                    result.Add(new UnitFlags(color, unitClass));
            }
            return result;
        }
    }

    public static IEnumerable<UnitFlags> NormalFlags => AllFlags.Where(x => SpecialColors.Contains(x.UnitColor) == false);
    public static IEnumerable<UnitColor> SpecialColors => new UnitColor[] { UnitColor.White, UnitColor.Black };
}

[Serializable]
public class CombineCondition
{
    [SerializeField] UnitFlags _targetUnitFlag;
    [SerializeField] Dictionary<UnitFlags, int> _needCountByFlag = new Dictionary<UnitFlags, int>();
    public CombineCondition(UnitFlags targetUnitFlag, Dictionary<UnitFlags, int> needCountByFlag)
    {
        _targetUnitFlag = targetUnitFlag;
        _needCountByFlag = needCountByFlag;
    }
    public CombineCondition() { }

    public UnitFlags TargetUnitFlag => _targetUnitFlag;
    public IReadOnlyDictionary<UnitFlags, int> NeedCountByFlag => _needCountByFlag;
}

[Serializable]
public class CombineConditions : ICsvLoader<UnitFlags, CombineCondition>
{
    public Dictionary<UnitFlags, CombineCondition> MakeDict(string csv)
    {
        return CsvUtility.CsvToArray<CombineCondition>(csv).ToDictionary(x => x.TargetUnitFlag, x => x);
    }
}

[Serializable]
public struct UnitNameData
{
    [SerializeField] UnitFlags _UnitFlags;
    [SerializeField] string _prefabName;
    [SerializeField] string _koearName;
    
    public UnitFlags UnitFlags => _UnitFlags;
    public string Name => _prefabName;
    public string KoearName => _koearName;
}

[Serializable]
public class UnitNameDatas : ICsvLoader<string, UnitNameData>
{
    public Dictionary<string, UnitNameData> MakeDict(string csv)
    {
        return CsvUtility.CsvToArray<UnitNameData>(csv).ToDictionary(x => x.KoearName, x => x);
    }
}

[Serializable]
public class UnitStat
{
    [SerializeField] UnitFlags _flag;
    [SerializeField] int _damage;
    [SerializeField] int _bossDamage;
    [SerializeField] float _attackDelayTime;
    [SerializeField] float _speed;
    [SerializeField] float _attackRange;

    public UnitFlags Flag => _flag;
    public int Damage => _damage;
    public int BossDamage => _bossDamage;
    public float AttackDelayTime => _attackDelayTime;
    public float Speed => _speed;
    public float AttackRange => _attackRange;

    public void SetAttDelayTime(float attackDelayTime) => _attackDelayTime = attackDelayTime;
    public void SetSpeed(float speed) => _speed = speed;
    public void SetAttackRange(float attackRange) => _attackRange = attackRange;

    public UnitStat GetClone()
    {
        var result = new UnitStat();
        result._flag = _flag;
        result._damage = _damage;
        result._bossDamage = _bossDamage;
        result._attackDelayTime = _attackDelayTime;
        result._speed = _speed;
        result._attackRange = _attackRange;
        return result;
    }
}


[Serializable]
public class UnitStats : ICsvLoader<UnitFlags, UnitStat>
{
    public Dictionary<UnitFlags, UnitStat> MakeDict(string csv)
        => CsvUtility.CsvToArray<UnitStat>(csv).ToDictionary(x => x.Flag, x => x);
}


[Serializable]
public struct UnitPassiveStat
{
    [SerializeField] UnitFlags _flag;
    [SerializeField] float[] _stats;

    public UnitFlags Flag => _flag;
    public IReadOnlyList<float> Stats => _stats;
}

[Serializable]
public class UnitPassiveStats : ICsvLoader<UnitFlags, UnitPassiveStat>
{
    public Dictionary<UnitFlags, UnitPassiveStat> MakeDict(string csv)
        => CsvUtility.CsvToArray<UnitPassiveStat>(csv).ToDictionary(x => x.Flag, x => x);
}

[Serializable]
public struct MageUnitStat
{
    [SerializeField] UnitFlags _flag;
    [SerializeField] int _maxMana;
    [SerializeField] int _addMana;
    [SerializeField] float[] _skillStats;

    public UnitFlags Flag => _flag;
    public int MaxMana => _maxMana;
    public int AddMana => _addMana;
    public IReadOnlyList<float> SkillStats => _skillStats;
}

[Serializable]
public class MageUnitStats : ICsvLoader<UnitFlags, MageUnitStat>
{
    public Dictionary<UnitFlags, MageUnitStat> MakeDict(string csv)
        => CsvUtility.CsvToArray<MageUnitStat>(csv).ToDictionary(x => x.Flag, x => x);
}