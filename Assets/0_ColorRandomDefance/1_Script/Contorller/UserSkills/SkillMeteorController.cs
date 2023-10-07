using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillMeteorController
{
    MeteorController _meteorController;
    MonsterManagerController _monsterManager;
    Multi_EnemyManager _enemyManager;
    public void DependencyInject(MeteorController meteorController, MonsterManagerController monsterManager, Multi_EnemyManager enemyManager)
    {
        _meteorController = meteorController;
        _monsterManager = monsterManager;
        _enemyManager = enemyManager;
    }

    public void ShotMeteor(byte id, int damage, float stunTime) => _meteorController.ShotMeteor(FindMonster(id), damage, stunTime, GetSpawnPos(id));

    Multi_NormalEnemy FindMonster(byte id)
    {
        if (_enemyManager.TryGetCurrentBoss(id, out Multi_BossEnemy boss)) return boss;

        var monsters = _monsterManager.GetNormalMonsters(id).ToList();
        if (monsters.Count == 0) return null;
        else return monsters[Random.Range(0, monsters.Count)];
    }
    Vector3 GetSpawnPos(byte id) => PlayerIdManager.IsMasterId(id) ? new Vector3(0, 30, 0) : new Vector3(0, 30, 500);
}
