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
        SuncoldDamages = damages;
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
}
