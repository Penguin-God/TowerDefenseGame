using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterFinder
{
    readonly WorldChangeController _worldChangeController;
    readonly MonsterManager _monsterManager;
    readonly int OwnerId = -1;

    public MonsterFinder(WorldChangeController worldChangeController, MonsterManager monsterManager, byte owerId)
    {
        _worldChangeController = worldChangeController;
        _monsterManager = monsterManager;
        OwnerId = owerId;
    }

    public Multi_Enemy FindTarget(Vector3 finderPos)
    {
        if (_worldChangeController.EnterStoryWorld) return Multi_EnemyManager.Instance.GetCurrnetTower(OwnerId);
        else if (Multi_EnemyManager.Instance.TryGetCurrentBoss(OwnerId, out Multi_BossEnemy boss)) return boss;
        else return GetProximateNormalMonster(finderPos);
    }

    Multi_NormalEnemy GetProximateNormalMonster(Vector3 finderPos) => GetProximateEnemys(finderPos, 1).FirstOrDefault();
    public Multi_NormalEnemy[] GetProximateEnemys(Vector3 finderPos, int maxCount)
        => _monsterManager.GetNormalMonsters().OrderBy(x => Vector3.Distance(finderPos, x.transform.position)).Take(maxCount).ToArray();
}
