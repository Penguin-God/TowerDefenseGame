using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetFinder
{
    readonly WorldChangeController _worldChangeController;
    readonly MonsterManager _monsterManager;
    readonly int _owerId = -1;

    public TargetFinder(WorldChangeController worldChangeController, MonsterManager monsterManager, byte owerId)
    {
        _worldChangeController = worldChangeController;
        _monsterManager = monsterManager;
        _owerId = owerId;
    }

    public Multi_Enemy FindTarget(Vector3 finderPos)
    {
        if (_worldChangeController.EnterStoryWorld)
            return Multi_EnemyManager.Instance.GetCurrnetTower(_owerId);
        if (Multi_EnemyManager.Instance.TryGetCurrentBoss(_owerId, out Multi_BossEnemy boss))
            return boss;

        return GetProximateNormalMonster(finderPos);
    }

    Multi_NormalEnemy GetProximateNormalMonster(Vector3 finderPos) => GetProximateEnemys(finderPos, 1).FirstOrDefault();
    public Multi_NormalEnemy[] GetProximateEnemys(Vector3 finderPos, int maxCount)
        => _monsterManager.GetNormalMonsters().OrderBy(x => Vector3.Distance(finderPos, x.transform.position)).Take(maxCount).ToArray();
}
