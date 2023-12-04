using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MonsterSlowController
{
    readonly protected SlowController _slowController;
    public MonsterSlowController(SlowController slowController) => _slowController = slowController;

    public void Slow(Slow slow) => OnSlow(slow);
    public virtual void Slow(Slow slow, UnitFlags flag) => OnSlow(slow);

    public virtual void SlowEffect() {}

    protected void OnSlow(Slow slow) => _slowController.ApplyNewSlow(slow);
}

public class SunColdMonsterSlowController : MonsterSlowController
{
    readonly Multi_NormalEnemy _normalMonster;
    readonly int[] _suncoldDamages;
    readonly WorldAudioPlayer _audioPlayer;

    public SunColdMonsterSlowController(SlowController slowController, Multi_NormalEnemy normalMonster, int[] damages, WorldAudioPlayer audioPlayer) : base(slowController)
    {
        _normalMonster = normalMonster;
        _suncoldDamages = damages;
        _audioPlayer = audioPlayer;
    }

    public override void Slow(Slow slow, UnitFlags flag)
    {
        if (_slowController.IsSlow && PhotonNetwork.IsMasterClient) 
            _normalMonster.OnDamage(_suncoldDamages[flag.ClassNumber], isSkill: true);
        base.OnSlow(slow);
    }

    public override void SlowEffect()
    {
        if (_slowController.IsSlow)
        {
            Managers.Effect.PlayOneShotEffect("BlueLightning", _normalMonster.transform.position + Vector3.up * 3);
            _audioPlayer.PlayObjectEffectSound(_normalMonster.MonsterSpot, EffectSoundType.LightningClip);
        }
    }
}
