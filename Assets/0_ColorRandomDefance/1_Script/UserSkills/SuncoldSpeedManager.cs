using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    public override void OnSlowWithTime(float slowRate, float slowTime, UnitFlags flag)
    {
        if (base.IsSlow)
        {
            _effect.PlayOneShotEffect("BlueLightning", _normalMonster.transform.position + Vector3.up * 3);
            Managers.Sound.PlayEffect(EffectSoundType.LightningClip);
            if (PhotonNetwork.IsMasterClient)
                _normalMonster.OnDamage(SuncoldDamages[flag.ClassNumber], isSkill: true);
        }
        if(_normalMonster.IsDead == false)
            base.OnSlowWithTime(slowRate, slowTime, flag);
    }

    int CalculateColdDamage(float slowRate) => Mathf.RoundToInt(Mathf.Pow(slowRate, 3) / 20); // 임시 계산 로직
}
