using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillMeteorController
{
    MeteorController _meteorController;
    MonsterManagerController _monsterManager;
    public SkillMeteorController(MeteorController meteorController, MonsterManagerController monsterManager)
    {
        _meteorController = meteorController;
        _monsterManager = monsterManager;
    }

    public void ShotMeteor(byte id, int damage, float stunTime)
    {
        var target = FindMonster(id);
        if (target == null) return;
        _meteorController.ShotMeteorToAll(target, damage, stunTime, GetSpawnPos(id), id);
    }

    Multi_NormalEnemy FindMonster(byte id)
    {
        if (Multi_EnemyManager.Instance.TryGetCurrentBoss(id, out Multi_BossEnemy boss)) return boss;

        var monsters = _monsterManager.GetNormalMonsters(id).ToList();
        if (monsters.Count == 0) return null;
        else return monsters[Random.Range(0, monsters.Count)];
    }
    Vector3 GetSpawnPos(byte id) => PlayerIdManager.IsMasterId(id) ? new Vector3(0, 30, 0) : new Vector3(0, 30, 500);
}
