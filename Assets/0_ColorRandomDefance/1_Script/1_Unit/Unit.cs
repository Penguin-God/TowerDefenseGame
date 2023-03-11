using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit
{
    private readonly UnitFlags _unitFlags;
    public UnitFlags UnitFlags => _unitFlags;

    public int Damage { get; private set; }
    public int BossDamage { get; private set; }
    public float Speed { get; private set; }
    public float AttackDelayTime { get; private set; }
    public float AttackRange { get; private set; }

    public Action<Multi_Enemy> OnHit;
    public Action<Multi_Enemy> OnPassiveHit;
    public event Action<Unit> OnDead;

    public Unit(UnitStat stat)
    {
        _unitFlags = stat.Flag;
        LoadStat(stat);
        OnHit += AttackEnemy;
    }

    public Unit(UnitFlags flag)
    {
        _unitFlags = flag;
    }

    void LoadStat(UnitStat stat)
    {
        Damage = stat.Damage;
        BossDamage = stat.BossDamage;
        Speed = stat.Speed;
        AttackDelayTime = stat.AttackDelayTime;
        AttackRange = stat.AttackRange;
    }

    public void Dead() => OnDead?.Invoke(this);

    void AttackEnemy(Multi_Enemy enemy) // Boss랑 쫄병 구분해서 대미지 적용
    {
        if (enemy.enemyType == EnemyType.Normal) AttackEnemy(enemy, Damage);
        else AttackEnemy(enemy, BossDamage);
    }

    void AttackEnemy(Multi_Enemy enemy, int damage) => enemy.OnDamage(damage);

    protected void SkillAttackToEnemy(Multi_Enemy enemy, int damage)
    {
        enemy.OnDamage(damage, isSkill: true);
        OnPassiveHit?.Invoke(enemy);
    }
}
