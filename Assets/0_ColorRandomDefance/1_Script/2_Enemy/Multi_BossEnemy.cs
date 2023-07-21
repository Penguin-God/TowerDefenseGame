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
    public void Spawn(int level)
    {
        BossData = Managers.Data.BossDataByLevel[level];
        SetStatus_RPC(BossData.Hp, BossData.Speed, false);
        AggroUnit();
    }

    public void Inject(BossData bossData, SpeedManager speedManager)
    {
        BossData = bossData;
        base.Inject(speedManager, bossData.Hp);
        AggroUnit();
    }

    void AggroUnit()
        => MultiServiceMidiator.Server.GetUnits(UsingId)
        .ToList()
        .ForEach(x => x.UpdateTarget());
}
