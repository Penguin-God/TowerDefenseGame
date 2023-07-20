using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuncoldSpeedManager : SpeedManager
{
    Multi_NormalEnemy _normalMonster;
    readonly int DamagePerSlowRate;
    MultiEffectManager _effect;
    public SuncoldSpeedManager(float originSpeed, Multi_NormalEnemy normalMonster, int damagePerSlowRate, MultiEffectManager effect) : base(originSpeed)
    {
        _normalMonster = normalMonster;
        DamagePerSlowRate = damagePerSlowRate;
        _effect = effect;
    }

    public override void OnSlow(float slowRate)
    {
        if (IsSlow)
        {
            _effect.PlayOneShotEffect("BlueLightning", _normalMonster.transform.position + Vector3.up * 3);
            Managers.Sound.PlayEffect(EffectSoundType.LightningClip);
            _normalMonster.OnDamage(CalculateColdDamage(slowRate), isSkill: true);
        }
        base.OnSlow(slowRate);
    }

    int CalculateColdDamage(float slowRate) => Mathf.RoundToInt(slowRate * DamagePerSlowRate);
}
