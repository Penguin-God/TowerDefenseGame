using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Multi_BossEnemy : Multi_NormalEnemy
{
    protected override void Init()
    {
        base.Init();
        enemyType = EnemyType.Boss;
    }

    public BossData BossData { get; private set; }
    public void Inject(BossData bossData)
    {
        BossData = bossData;
        base.Inject(bossData.Hp);
        AggroUnit();
    }

    public override void Dead()
    {
        base.Dead();
        if(PlayerIdManager.Id == UsingId)
        {
            Managers.Sound.PlayBgm(BgmType.Default);
            Managers.Sound.PlayEffect(EffectSoundType.BossDeadClip);
        }
    }

    void AggroUnit()
        => MultiServiceMidiator.Server.GetUnits(UsingId)
        .ToList()
        .ForEach(x => x.UpdateTarget());
}
