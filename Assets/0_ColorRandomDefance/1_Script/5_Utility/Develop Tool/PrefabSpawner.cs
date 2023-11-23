using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    Multi_NormalUnitSpawner _unitSpawner;
    Multi_BossEnemySpawner _bossSpawner;
    MonsterSpawnerContorller _monsterSpawnerContorller;
    public void DependencyInject(Multi_NormalUnitSpawner unitSpawner, Multi_BossEnemySpawner bossSpawner, MonsterSpawnerContorller monsterSpawnerContorller)
    {
        _unitSpawner = unitSpawner;
        _bossSpawner = bossSpawner;
        _monsterSpawnerContorller = monsterSpawnerContorller;
    }

    public void SpawnUnit(UnitFlags flag) => SpawnUnit(flag, 0);
    public void SpawnUnit_ByClient(UnitFlags flag) => SpawnUnit(flag, 1);
    void SpawnUnit(UnitFlags flag, byte id) => _unitSpawner.Spawn(flag, id);

    public void SpawnBoss(int bossLevel)
    {
        foreach (var id in PlayerIdManager.AllId)
            _bossSpawner.SpawnBoss(id, bossLevel);
    }

    public void StopSpawnMonster()
    {
        StageManager.Instance.StopAllCoroutines();
        _monsterSpawnerContorller.StopAllCoroutines();
    }
}