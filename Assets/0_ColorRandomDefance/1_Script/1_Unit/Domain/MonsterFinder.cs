using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterFinder
{
    readonly MonsterManagerController _monsterManagerController;
    readonly byte WorldId = 255;

    public MonsterFinder(MonsterManagerController monsterManagerController, byte worldId)
    {
        _monsterManagerController = monsterManagerController;
        WorldId = worldId;
    }

    public Multi_Enemy FindTarget(bool isInDefenseWorld, Vector3 finderPos)
    {
        if (isInDefenseWorld)
        {
            if (Multi_EnemyManager.Instance.TryGetCurrentBoss(WorldId, out Multi_BossEnemy boss)) return boss;
            else return GetProximateNormalMonster(finderPos);
        }
        else return Multi_EnemyManager.Instance.GetCurrnetTower(WorldId);
    }

    Multi_NormalEnemy GetProximateNormalMonster(Vector3 finderPos) => GetProximateEnemys(finderPos, 1).FirstOrDefault();
    public Multi_NormalEnemy[] GetProximateEnemys(Vector3 finderPos, int maxCount)
        => _monsterManagerController.GetNormalMonsters(WorldId).OrderBy(x => Vector3.Distance(finderPos, x.transform.position)).Take(maxCount).ToArray();
}
