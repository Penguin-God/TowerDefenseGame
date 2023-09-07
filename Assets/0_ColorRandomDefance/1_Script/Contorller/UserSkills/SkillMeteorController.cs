using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkillMeteorController : MonoBehaviourPun
{
    MeteorController _meteorController;
    void Start()
    {
        _meteorController = gameObject.AddComponent<MeteorController>();
    }

    ServerMonsterManager _monsterManager;
    Multi_EnemyManager _enemyManager;
    public void RecevieInject(ServerMonsterManager monsterManager, Multi_EnemyManager enemyManager)
    {
        _monsterManager = monsterManager;
        _enemyManager = enemyManager;
    }

    public void ShotMeteor(byte id, int damage, float stunTime) => photonView.RPC(nameof(RPC_ShotMeteor), RpcTarget.MasterClient, id, damage, stunTime);

    [PunRPC]
    void RPC_ShotMeteor(byte id, int damage, float stunTime) => _meteorController.ShotMeteor(FindMonster(id), damage, stunTime, GetSpawnPos(id));

    Multi_NormalEnemy FindMonster(byte id)
    {
        if (_enemyManager.TryGetCurrentBoss(id, out Multi_BossEnemy boss)) return boss;

        var monsters = _monsterManager.GetNormalMonsters(id);
        if (monsters.Count == 0) return null;
        else return monsters[Random.Range(0, monsters.Count)];
    }
    Vector3 GetSpawnPos(byte id) => PlayerIdManager.IsMasterId(id) ? new Vector3(0, 30, 0) : new Vector3(0, 30, 500);
}
