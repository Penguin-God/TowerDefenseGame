using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ISpawner
{
    void Spawn();
}

namespace EnemySpawnRules
{
    class EnemySpawnRule // 부활, 숫자
    {
        ISpawner _normalEnemySpanwer;
        ISpawner _bossEnemySpanwer;
        int[] _enemyNums = new int[] { 0, 0 };
        public EnemySpawnRule(ISpawner normalEnemySpanwer, ISpawner bossEnemySpanwer)
        {
            _normalEnemySpanwer = normalEnemySpanwer;
            _bossEnemySpanwer = bossEnemySpanwer;
        }

        public void StageSpawn()
        {
            
        }
    }
}
