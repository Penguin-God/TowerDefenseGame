using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SuncoldSpeedManager : SpeedManager
{
    readonly Multi_NormalEnemy _normalMonster;
    readonly int[] _suncoldDamages;
    readonly MultiEffectManager _effect;
    readonly WorldAudioPlayer _audioPlayer;

    public SuncoldSpeedManager(float originSpeed, Multi_NormalEnemy normalMonster, int[] damages, MultiEffectManager effect, WorldAudioPlayer audioPlayer) : base(originSpeed)
    {
        _normalMonster = normalMonster;
        _suncoldDamages = damages;
        _effect = effect;
        _audioPlayer = audioPlayer;
    }

    public override void OnSlow(float slowRate, UnitFlags flag)
    {
        if (base.IsSlow)
        {
            _effect.PlayOneShotEffect("BlueLightning", _normalMonster.transform.position + Vector3.up * 3);
            _audioPlayer.PlayObjectEffectSound(_normalMonster.MonsterSpot, EffectSoundType.LightningClip);
            if (PhotonNetwork.IsMasterClient)
                _normalMonster.OnDamage(_suncoldDamages[flag.ClassNumber], isSkill: true);
        }
        if (_normalMonster.IsDead == false)
            base.OnSlow(slowRate);
    }
}
