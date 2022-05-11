using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Multi_TowerEnemySpawner : Multi_EnemySpawnerBase
{
    public event Action<Multi_EnemyTower> OnSpawn;
    public event Action<Multi_EnemyTower> OnDead;

    public override void Init()
    {
        for (int i = 0; i < _enemys.Length; i++)
        {
            Multi_EnemyTower[] enemys = CreatePool_InGroup<Multi_EnemyTower>(_enemys[i], BuildPath(_rootPath, _enemys[i]), spawnCount);

            foreach (var enemy in enemys) SetEnemy(enemy);
        }
    }

    void SetEnemy(Multi_EnemyTower enemy)
    {
        enemy.enemyType = EnemyType.Tower;

        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => Multi_Managers.Pool.Push(enemy.GetComponent<Poolable>());
    }
}
