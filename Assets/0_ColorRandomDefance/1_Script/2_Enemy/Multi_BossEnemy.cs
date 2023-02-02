using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Multi_BossEnemy : Multi_NormalEnemy
{
    [SerializeField] int _level;

    protected override void Init()
    {
        base.Init();
        enemyType = EnemyType.Boss;
    }

    public BossData BossData { get; private set; }
    public void Spawn(int level)
    {
        _level = level;
        BossData = Managers.Data.BossDataByLevel[_level];
        SetStatus_RPC(BossData.Hp, BossData.Speed, false);
        AggroUnit(); 
    }

    void AggroUnit() 
        => Multi_UnitManager.Instance.Master
        .GetUnits(UsingId)
        .ToList()
        .ForEach(x => x.UpdateTarget());
}
