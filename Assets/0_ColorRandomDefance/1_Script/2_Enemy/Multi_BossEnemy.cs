using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class Multi_BossEnemy : Multi_NormalEnemy
{
    protected override void Init()
    {
        base.Init();
        enemyType = EnemyType.Boss;
    }

    public BossData BossData { get; private set; }
    public void Inject(BossData bossData, UnitManagerController unitManagerController, MonsterSlowController monsterSlowController, SpeedManager speedManager)
    {
        BossData = bossData;
        Inject(bossData.Hp, monsterSlowController, speedManager);
        Go();
        AggroUnit(unitManagerController);
    }

    [PunRPC]
    public override void Dead()
    {
        base.Dead();
        if(PlayerIdManager.Id == UsingId)
        {
            Managers.Sound.PlayBgm(BgmType.Default);
            Managers.Sound.PlayEffect(EffectSoundType.BossDeadClip);
        }
    }

    void AggroUnit(UnitManagerController unitManagerController)
        => unitManagerController.GetUnits(UsingId)
        .ToList()
        .ForEach(x => x.UpdateTarget());
}
