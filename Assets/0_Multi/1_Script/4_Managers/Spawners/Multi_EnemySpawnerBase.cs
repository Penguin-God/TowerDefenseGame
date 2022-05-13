using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class Multi_EnemySpawnerBase : Multi_SpawnerBase
{
    [SerializeField] protected GameObject[] _enemys;
    [SerializeField] protected int spawnCount;


    // => enemys.Select(x => SetEnemy(x, type, deadAction)).ToArray();
    //protected void SetEnemys<T>(T[] enemys, EnemyType type, Action<T> deadAction) where T : Multi_Enemy
    //    => enemys.Select(x => SetEnemy(x, type, deadAction));

    //T SetEnemy<T>(T enemy, EnemyType type, Action<T> deadAction) where T : Multi_Enemy
    //{
    //    print(deadAction == null);
    //    enemy.enemyType = type;
    //    enemy.OnDeath += () => deadAction(enemy);
    //    enemy.OnDeath += () => Multi_Managers.Pool.Push(enemy.GetComponent<Poolable>());
    //    return enemy;
    //}
}
