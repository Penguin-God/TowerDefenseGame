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
            Managers.Effect.PlayOneShotEffect("BlueLightning", _normalMonster.transform.position + Vector3.up * 3);
            Managers.Sound.PlayEffect(EffectSoundType.LightningClip);
            _normalMonster.OnDamage(CalculateColdDamage(slowRate), isSkill: true);
        }
        base.OnSlow(slowRate);
    }

    int CalculateColdDamage(float slowRate) => Mathf.RoundToInt(slowRate * DamagePerSlowRate);
}
