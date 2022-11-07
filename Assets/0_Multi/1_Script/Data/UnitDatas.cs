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

    public string KoreaName => Multi_Managers.Data.UnitNameDataByFlag[this].KoearName;

    public bool Equals(UnitFlags other) 
        => other.ColorNumber == _colorNumber && other.ClassNumber == _classNumber;

    public override int GetHashCode() => (_colorNumber, _classNumber).GetHashCode();
    public override bool Equals(object other) => base.Equals(other);

    public static bool operator ==(UnitFlags lhs, UnitFlags rhs) 
        => lhs.ColorNumber == rhs.ColorNumber && lhs.ClassNumber == rhs.ClassNumber;
    public static bool operator !=(UnitFlags lhs, UnitFlags rhs) => !(lhs == rhs);
}

[Serializable]
public class CombineCondition
{
    [SerializeField] UnitFlags _targetUnitFlag;
    [SerializeField] Dictionary<UnitFlags, int> _needCountByFlag = new Dictionary<UnitFlags, int>();

    public UnitFlags TargetUnitFlags => _targetUnitFlag;
    public IReadOnlyDictionary<UnitFlags, int> NeedCountByFlag => _needCountByFlag;
}

[Serializable]
public class CombineConditions : ICsvLoader<UnitFlags, CombineCondition>
{
    public Dictionary<UnitFlags, CombineCondition> MakeDict(string csv)
    {
        return CsvUtility.CsvToArray<CombineCondition>(csv).ToDictionary(x => x.TargetUnitFlags, x => x);
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
    public Dictionary<UnitFlags, WeaponData> MakeDict(string csv) => CsvUtility.CsvToArray<WeaponData>(csv).ToDictionary(x => x.Flag, x => x);
}


[Serializable]
public class UnitStat
{
    [SerializeField] UnitFlags _flag;
    [SerializeField] int _damage;
    [SerializeField] int _bossDamage;
    //[SerializeField] int _useSkillPercent;
    [SerializeField] float _attackDelayTime;
    [SerializeField] float _speed;
    [SerializeField] float _attackRange;

    public UnitStat GetClone()
    {
        UnitStat result = new UnitStat();
        result._flag = Flag;
        result._damage = Damage;
        result._bossDamage = BossDamage;
        result._attackDelayTime = AttackDelayTime;
        result._speed = Speed;
        result._attackRange = AttackRange;
        return result;
    }

    public UnitFlags Flag => _flag;
    public int Damage => _damage;
    public int BossDamage => _bossDamage;
    public float AttackDelayTime => _attackDelayTime;
    public float Speed => _speed;
    public float AttackRange => _attackRange;

    public void SetDamage(int damage) => _damage = damage;
    public void SetBossDamage(int bossDamage) => _bossDamage = bossDamage;
    public void SetAttDelayTime(float attackDelayTime) => _attackDelayTime = attackDelayTime;
    public void SetSpeed(float speed) => _speed = speed;
    public void SetAttackRange(float attackRange) => _attackRange = attackRange;
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


// TODO : struct도 원본 수정 가능한지 확인하기
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