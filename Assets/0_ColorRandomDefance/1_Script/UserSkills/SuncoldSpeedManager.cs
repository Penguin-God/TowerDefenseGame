using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuncoldSpeedManager : SpeedManager
{
    Multi_NormalEnemy _normalMonster;
    readonly int DamagePerSlowRate;
    public SuncoldSpeedManager(float originSpeed, Multi_NormalEnemy normalMonster, int damagePerSlowRate) : base(originSpeed)
    {
        _normalMonster = normalMonster;
        DamagePerSlowRate = damagePerSlowRate;
    }

    public override void OnSlow(float slowRate)
    {
        if (IsSlow)
        {
            _normalMonster.OnDamage(CalculateColdDamage(slowRate), isSkill: true);
            // ÀÌÆÑÆ®
        }
        base.OnSlow(slowRate);
    }

    int CalculateColdDamage(float slowRate) => Mathf.RoundToInt(slowRate * DamagePerSlowRate);
}
