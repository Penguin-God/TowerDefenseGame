using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Multi_EnemySpawnerBase : Multi_SpawnerBase
{
    [SerializeField] protected GameObject[] _enemys;
    [SerializeField] protected int spawnCount;
}
