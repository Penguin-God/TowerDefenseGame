using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Multi_EnemySpawnerBase : Multi_SpawnerBase
{
    [Header("Enemy Spawner Field")]
    [SerializeField] protected int _spawnableObjectCount;
    [SerializeField] protected int spawnCount;
    [SerializeField] protected Vector3[] spawnPositions;
}
