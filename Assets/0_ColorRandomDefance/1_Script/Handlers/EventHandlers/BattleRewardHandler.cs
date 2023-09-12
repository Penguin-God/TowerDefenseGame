using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BattleRewardHandler : MonoBehaviourPun
{
    Multi_GameManager _gameManager;
    void Start()
    {
        _gameManager = Multi_GameManager.Instance;
        _dispatcher.OnStageUpExcludingFirst += _stage => _gameManager.AddGold(_stageUpGoldRewardCalculator.CalculateRewradGold());

        if (PhotonNetwork.IsMasterClient)
        {
            _bossSpawner.OnDead += GetBossReward;
            Multi_SpawnManagers.TowerEnemy.OnDead += GetTowerReward;
        }
    }

    Multi_BossEnemySpawner _bossSpawner;
    BattleEventDispatcher _dispatcher;
    StageUpGoldRewardCalculator _stageUpGoldRewardCalculator;
    public void Inject(BattleEventDispatcher dispatcher, Multi_BossEnemySpawner bossSpawner, StageUpGoldRewardCalculator stageUpGoldRewardCalculator)
    {
        _bossSpawner = bossSpawner;
        _dispatcher = dispatcher;
        _stageUpGoldRewardCalculator = stageUpGoldRewardCalculator;
    }

    void GetBossReward(Multi_BossEnemy enemy)
    {
        if (enemy.UsingId == PlayerIdManager.Id)
            GetReward(enemy.BossData);
        else
            photonView.RPC(nameof(GetReward), RpcTarget.Others, enemy.BossData.Gold, enemy.BossData.Food);
    }

    void GetTowerReward(Multi_EnemyTower enemy)
    {
        if (enemy.UsingId == PlayerIdManager.Id)
            GetReward(enemy.TowerData);
        else
            photonView.RPC(nameof(GetReward), RpcTarget.Others, enemy.TowerData.Gold, enemy.TowerData.Food);
    }

    void GetReward(BossData data) => GetReward(data.Gold, data.Food);

    [PunRPC]
    void GetReward(int gold, int food)
    {
        _gameManager.AddGold(gold);
        _gameManager.AddFood(food);
    }
}

public class StageUpGoldRewardCalculator
{
    int _rewardGold;
    public StageUpGoldRewardCalculator(int rewardGold) => _rewardGold = rewardGold;
    public virtual int CalculateRewradGold() => _rewardGold;
}
