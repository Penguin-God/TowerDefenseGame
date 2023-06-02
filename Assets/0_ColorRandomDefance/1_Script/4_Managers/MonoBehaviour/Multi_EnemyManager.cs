﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun;

public class Multi_EnemyManager : Singleton<Multi_EnemyManager>
{
    MasterManager _master = new MasterManager();
    EnemyFinder _finder = new EnemyFinder();

    void Awake()
    {
        Init();
    }

    public void AddNormalMonster(Multi_NormalEnemy monster)
    {
        _master.AddEnemy(monster);
        monster.OnDead += (_died) => _master.RemoveEnemy(monster);
    }

    RPCData<Multi_BossEnemy> _currentBoss = new RPCData<Multi_BossEnemy>();
    public void SetSpawnBoss(int id, Multi_BossEnemy boss)
    {
        _currentBoss.Set(id, boss);
        boss.OnDead += died => _currentBoss.Set(id, null);
    }
    public bool TryGetCurrentBoss(int id, out Multi_BossEnemy boss)
    {
        boss = _currentBoss.Get(id);
        return boss != null;
    }

    RPCData<Multi_EnemyTower> _currentTower = new RPCData<Multi_EnemyTower>();
    public void SetSpawnTower(int id, Multi_EnemyTower tower)
    {
        _currentTower.Set(id, tower);
        tower.OnDead += died => _currentTower.Set(id, null);
    }
    public Multi_EnemyTower GetCurrnetTower(int id) => _currentTower.Get(id);

    public Multi_Enemy GetProximateEnemy(Vector3 finderPos, int unitId) => _finder.GetProximateEnemy(finderPos, _master.GetEnemys(unitId));

    public Multi_Enemy[] GetProximateEnemys(Vector3 finderPos, int maxCount, int unitId)
    {
        if (maxCount >= _master.GetEnemys(unitId).Count) return _master.GetEnemys(unitId).ToArray();
        return _finder.GetProximateEnemys(finderPos, maxCount, _master.GetEnemys(unitId));
    }

    class MasterManager
    {
        RPCData<List<Multi_NormalEnemy>> _enemyCountData = new RPCData<List<Multi_NormalEnemy>>();
        public IReadOnlyList<Multi_NormalEnemy> GetEnemys(int id) => _enemyCountData.Get(id);
        public RPCAction<int, int> OnEnemyCountChanged = new RPCAction<int, int>();

        public void AddEnemy(Multi_NormalEnemy _enemy)
        {
            if (PhotonNetwork.IsMasterClient == false) return;

            int id = _enemy.GetComponent<RPCable>().UsingId;
            _enemyCountData.Get(id).Add(_enemy);
            OnEnemyCountChanged.RaiseAll(id, _enemyCountData.Get(id).Count);
        }

        public void RemoveEnemy(Multi_NormalEnemy _enemy)
        {
            if (PhotonNetwork.IsMasterClient == false) return;

            int id = _enemy.GetComponent<RPCable>().UsingId;
            _enemyCountData.Get(id).Remove(_enemy);
            OnEnemyCountChanged.RaiseAll(id, _enemyCountData.Get(id).Count);
        }
    }


    class EnemyFinder
    {
        public Multi_Enemy GetProximateEnemy(Vector3 _unitPos, IEnumerable<Multi_Enemy> _enemyList)
        {
            if (_enemyList == null || _enemyList.Count() == 0) return null;

            float shortDistance = Mathf.Infinity;

            Multi_Enemy _returnEnemy = null;
            foreach (Multi_Enemy _enemy in _enemyList)
            {
                if (_enemy != null && _enemy.IsDead == false)
                {
                    float distanceToEnemy = Vector3.Distance(_unitPos, _enemy.transform.position);
                    if (distanceToEnemy < shortDistance)
                    {
                        shortDistance = distanceToEnemy;
                        _returnEnemy = _enemy;
                    }
                }
            }

            return _returnEnemy;
        }

        public Multi_Enemy[] GetProximateEnemys(Vector3 _unitPos, int count, IReadOnlyList<Multi_Enemy> enemys)
        {
            Debug.Assert(enemys.Count > count, $"적 카운트 수가 {enemys.Count}이 배열의 크기인 {count}보다 \n 크지 않은 상태에서 함수가 실행됨.");

            List<Multi_Enemy> targets = new List<Multi_Enemy>(enemys);
            Multi_Enemy[] result = new Multi_Enemy[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = GetProximateEnemy(_unitPos, targets);
                targets.Remove(result[i]);
            }
            return result;
        }
    }
}
