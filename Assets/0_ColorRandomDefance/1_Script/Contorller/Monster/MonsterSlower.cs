using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSlowController
{
    readonly SlowController _slowController;
    public MonsterSlowController(SlowController slowController) => _slowController = slowController;

    public void Slow(Slow slow) => OnSlow(slow);
    public virtual void Slow(Slow slow, UnitFlags flag) => OnSlow(slow);

    protected void OnSlow(Slow slow) => _slowController.ApplyNewSlow(slow);
}

public class SunColdMonsterSlowController : MonsterSlowController
{
    readonly Multi_NormalEnemy _normalMonster;
    readonly SpeedManager _speedManager;
    readonly int[] _suncoldDamages;
    readonly MultiEffectManager _effect;
    readonly WorldAudioPlayer _audioPlayer;

    public SunColdMonsterSlowController(SlowController slowController, SpeedManager speedManager, Multi_NormalEnemy normalMonster, int[] damages, 
        MultiEffectManager effect, WorldAudioPlayer audioPlayer) : base(slowController)
    {
        _normalMonster = normalMonster;
        _speedManager = speedManager;
        _suncoldDamages = damages;
        _effect = effect;
        _audioPlayer = audioPlayer;
    }

    public override void Slow(Slow slow, UnitFlags flag)
    {
        if (_speedManager.IsSlow)
        {
            _effect.PlayOneShotEffect("BlueLightning", _normalMonster.transform.position + Vector3.up * 3);
            _audioPlayer.PlayObjectEffectSound(_normalMonster.MonsterSpot, EffectSoundType.LightningClip);
            if (PhotonNetwork.IsMasterClient)
                _normalMonster.OnDamage(_suncoldDamages[flag.ClassNumber], isSkill: true);
        }
        if (_normalMonster.IsDead == false)
            base.OnSlow(slow);
    }
}
