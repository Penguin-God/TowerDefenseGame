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

    IMonsterManager _monsterManager;
    Multi_EnemyManager _enemyManager;
    Vector3 _spawnPos;
    public void RecevieInject(IMonsterManager monsterManager, Multi_EnemyManager enemyManager, Vector3 spawnPos)
    {
        _monsterManager = monsterManager;
        _enemyManager = enemyManager;
        _spawnPos = spawnPos;
    }

    public void ShotMeteor(int damage, float stunTime) => photonView.RPC(nameof(RPC_ShotMeteor), RpcTarget.MasterClient, damage, stunTime);

    [PunRPC]
    void RPC_ShotMeteor(int damage, float stunTime) => _meteorController.ShotMeteor(FindMonster(), damage, stunTime, _spawnPos);

    Multi_NormalEnemy FindMonster()
    {
        if (_enemyManager.TryGetCurrentBoss(PlayerIdManager.Id, out Multi_BossEnemy boss)) return boss;

        var monsters = _monsterManager.GetNormalMonsters();
        if (monsters.Count == 0) return null;
        else return monsters[Random.Range(0, monsters.Count)];
    }
}
