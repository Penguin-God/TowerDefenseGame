using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NormalMonsterSpawner
{
    public Multi_NormalEnemy SpawnMonster(byte num, int id, int stage)
    {
        var enemy = Managers.Multi.Instantiater.PhotonInstantiate(new ResourcesPathBuilder().BuildMonsterPath(num), id).GetComponent<Multi_NormalEnemy>();
        NormalEnemyData data = Managers.Data.NormalEnemyDataByStage[stage];
        enemy.SetStatus_RPC(data.Hp, data.Speed, false);
        Multi_EnemyManager.Instance.AddNormalMonster(enemy);
        return enemy;
    }
}

// 이게 실시간으로 바뀌면 어쩔 수 없는데, 스테이지 들어간 후에 낙장불입이면 그냥 스테이지 시작할 때만 RPC 보내는게 좋을 듯.
public class EnemySpawnNumManager : MonoBehaviourPun
{
    byte[] _spawnEnemyNums = new byte[2];

    public byte GetSpawnEnemyNum(int id) => _spawnEnemyNums[id];
    public void SetSpawnNumber(byte num)
    {
        if (PhotonNetwork.IsMasterClient)
            _spawnEnemyNums[0] = num;
        else
            photonView.RPC(nameof(SetClientSpawnNumber), RpcTarget.MasterClient, num);
    }
    [PunRPC]
    void SetClientSpawnNumber(byte num) => _spawnEnemyNums[1] = num;
}

public class MonsterSpawnerContorller : MonoBehaviour
{
    EnemySpawnNumManager _numManager;

    void Start()
    {
        _numManager = gameObject.GetOrAddComponent<EnemySpawnNumManager>();
        if (PhotonNetwork.IsMasterClient == false) return;
        Multi_StageManager.Instance.OnUpdateStage += SpawnMonsterOnStageChange; // normal
        Multi_StageManager.Instance.OnUpdateStage += SpawnBossOnStageMultipleOfTen; // boss
        Multi_GameManager.instance.OnGameStart += SpawnTowerOnStart; // tower
    }

    void SpawnMonsterOnStageChange(int stage)
    {
        if (IsBossStage(stage)) return;
        StartCoroutine(Co_StageSpawn(0, stage));
        StartCoroutine(Co_StageSpawn(1, stage));
    }

    Multi_NormalEnemy SpawnMonsterToOther(byte num, int id, int stage) 
        => new NormalMonsterSpawner().SpawnMonster(num, id == 0 ? 1 : 0, stage);

    [SerializeField] float _spawnDelayTime = 1.8f;
    [SerializeField] int _stageSpawnCount = 15;
    IEnumerator Co_StageSpawn(byte id, int stage)
    {
        byte num = _numManager.GetSpawnEnemyNum(id);
        for (int i = 0; i < _stageSpawnCount; i++)
        {
            var enemy = SpawnMonsterToOther(num, id, stage); // num 인라인 안 한 이유는 스테이지 한 번 들어가면 못 바꾸게 할려고
            enemy.OnDead += (died) => ResurrectionMonsterToOther(enemy);
            yield return new WaitForSeconds(_spawnDelayTime);
        }
    }

    void ResurrectionMonsterToOther(Multi_NormalEnemy enemy)
    {
        if (enemy.IsResurrection) return;
        var spawnEnemy = SpawnMonsterToOther(_numManager.GetSpawnEnemyNum(enemy.UsingId), enemy.UsingId, enemy.SpawnStage);
        spawnEnemy.Resurrection();
    }

    void SpawnBossOnStageMultipleOfTen(int stage)
    {
        if (IsBossStage(stage) == false) return;
        Multi_SpawnManagers.BossEnemy.Spawn(0);
        Multi_SpawnManagers.BossEnemy.Spawn(1);
    }

    void SpawnTowerOnStart()
    {
        Multi_SpawnManagers.TowerEnemy.Spawn(0);
        Multi_SpawnManagers.TowerEnemy.Spawn(1);
    }

    bool IsBossStage(int stage) => stage % 10 == 0;
}
