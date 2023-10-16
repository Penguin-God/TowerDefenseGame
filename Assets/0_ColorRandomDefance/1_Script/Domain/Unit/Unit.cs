using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public enum UnitState
//{
//    Idle,
//    Move,
//    Attack,
//    Die,
//}

[Serializable]
public class UnitStats
{
    public readonly UnitDamageInfo DamageInfo;
    public float AttackDelayTime;
    public float AttackSpeed;
    public float AttackRange;
    public float Speed;

    public UnitStats(UnitDamageInfo damageInfo, float attackDelayTime, float attackSpeed, float attackRange, float speed)
    {
        DamageInfo = damageInfo;
        AttackDelayTime = attackDelayTime;
        AttackSpeed = attackSpeed;
        AttackRange = attackRange;
        Speed = speed;
    }
}

[Serializable]
public class Unit
{
    [SerializeField] UnitFlags _unitFlags;
    public UnitFlags UnitFlags => _unitFlags;

    public readonly UnitStats Stats;
    [SerializeField] UnitDamageInfo _damageInfo;
    public UnitDamageInfo DamageInfo => _damageInfo;
    public void UpdateDamageInfo(UnitDamageInfo newInfo) => _damageInfo = newInfo;

    public Unit(UnitFlags flag, UnitStats unitStats)
    {
        _unitFlags = flag;
        Stats = unitStats;
        _damageInfo = unitStats.DamageInfo;
    }
}
