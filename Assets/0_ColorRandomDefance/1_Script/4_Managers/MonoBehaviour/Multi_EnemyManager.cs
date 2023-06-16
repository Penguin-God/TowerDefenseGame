using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_EnemyManager : Singleton<Multi_EnemyManager>
{
    void Awake()
    {
        Init();
    }

    RPCData<Multi_BossEnemy> _currentBoss = new RPCData<Multi_BossEnemy>();
    public void SetSpawnBoss(int id, Multi_BossEnemy boss)
    {
        _currentBoss.Set(id, boss);
        boss.OnDead += died => _currentBoss.Set(id, null);
    }
    public bool TryGetCurrentBoss(int id, out Multi_BossEnemy boss)
    {
        boss = _currentBoss.Get(id);
        return boss != null;
    }

    RPCData<Multi_EnemyTower> _currentTower = new RPCData<Multi_EnemyTower>();
    public void SetSpawnTower(int id, Multi_EnemyTower tower)
    {
        _currentTower.Set(id, tower);
        tower.OnDead += died => _currentTower.Set(id, null);
    }
    public Multi_EnemyTower GetCurrnetTower(int id) => _currentTower.Get(id);
}
