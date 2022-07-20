using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// TODO : 여기는 싹 다 구현이 안되있으니 구현하기
public class Multi_TowerEnemySpawner : Multi_EnemySpawnerBase
{
    public event Action<Multi_EnemyTower> OnSpawn;
    public event Action<Multi_EnemyTower> OnDead;

    protected override void MasterInit()
    {
        CreatePool();
    }

    void CreatePool()
    {
        for (int i = 0; i < _enemys.Length; i++)
            CreatePool_InGroup<Multi_EnemyTower>(_enemys[i], BuildPath(_rootPath, _enemys[i]), spawnCount);
    }

    public override void SettingPoolObject(object obj)
    {
        Multi_EnemyTower enemy = obj as Multi_EnemyTower;
        Debug.Assert(enemy != null, "캐스팅 실패!!");
        SetEnemy(enemy);
    }

    void SetEnemy(Multi_EnemyTower enemy)
    {
        enemy.enemyType = EnemyType.Tower;

        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => Multi_Managers.Pool.Push(enemy.GetComponent<Poolable>());
    }
}
