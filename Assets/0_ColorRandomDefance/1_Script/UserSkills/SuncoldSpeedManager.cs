using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuncoldSpeedManager : MonsterSpeedManager
{
    Multi_NormalEnemy _normalMonster;
    int[] SuncoldDamages;
    MultiEffectManager _effect;
    public void Init(float originSpeed, Multi_NormalEnemy normalMonster, int[] damages, MultiEffectManager effect)
    {
        base.SetSpeed(originSpeed);
        _normalMonster = normalMonster;
        SuncoldDamages = new int[] { 150, 1000, 10000, 100000 };
        _effect = effect;
    }

    public override void OnSlow(float slowRate, float slowTime, UnitFlags flag)
    {
        print(base.SpeedManager.IsSlow);
        if (base.SpeedManager.IsSlow)
        {
            _effect.PlayOneShotEffect("BlueLightning", _normalMonster.transform.position + Vector3.up * 3);
            Managers.Sound.PlayEffect(EffectSoundType.LightningClip);
            _normalMonster.OnDamage(SuncoldDamages[flag.ClassNumber], isSkill: true);
        }
        base.OnSlow(slowRate, slowTime, flag);
    }

    int CalculateColdDamage(float slowRate) => Mathf.RoundToInt(Mathf.Pow(slowRate, 3) / 20); // 임시 계산 로직
}
