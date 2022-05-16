﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Multi_SpawnManagers : MonoBehaviourPun
{
    private static Multi_SpawnManagers instance;
    public static Multi_SpawnManagers Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<Multi_SpawnManagers>();
                if(instance == null) instance = new GameObject("Multi_SpawnManagers").AddComponent<Multi_SpawnManagers>();
            }
            return instance;
        }
    }

    Dictionary<Type, Multi_SpawnerBase> _spawnerByType = new Dictionary<Type, Multi_SpawnerBase>();
    public IReadOnlyDictionary<Type, Multi_SpawnerBase> SpawnerByType => _spawnerByType;

    Multi_NormalEnemySpawner _normalEnemy;
    Multi_BossEnemySpawner _bossEnemy;
    Multi_TowerEnemySpawner _towerEnemy;
    Multi_NormalUnitSpawner _normalUnit;
    Multi_WeaponSpawner _weapon;

    public static Multi_NormalEnemySpawner NormalEnemy => Instance._normalEnemy;
    public static Multi_BossEnemySpawner BossEnemy => Instance._bossEnemy;
    public static Multi_TowerEnemySpawner TowerEnemy => Instance._towerEnemy;
    public static Multi_NormalUnitSpawner NormalUnit => Instance._normalUnit;
    public static Multi_WeaponSpawner Weapon => Instance._weapon;

    void Awake()
    {
        _normalEnemy = GetOrAddChildComponent<Multi_NormalEnemySpawner>();
        _bossEnemy = GetOrAddChildComponent<Multi_BossEnemySpawner>();
        _towerEnemy = GetOrAddChildComponent<Multi_TowerEnemySpawner>();
        _normalUnit = GetOrAddChildComponent<Multi_NormalUnitSpawner>();
        _weapon = GetOrAddChildComponent<Multi_WeaponSpawner>();

        _spawnerByType.Add(typeof(Multi_NormalEnemy), _normalEnemy);
        _spawnerByType.Add(typeof(Multi_ArcherEnemy), _normalEnemy);
        _spawnerByType.Add(typeof(Multi_SpearmanEnemy), _normalEnemy);
        _spawnerByType.Add(typeof(Multi_MageEnemy), _normalEnemy);
        _spawnerByType.Add(typeof(Multi_BossEnemy), _bossEnemy);
        _spawnerByType.Add(typeof(Multi_EnemyTower), _towerEnemy);
        _spawnerByType.Add(typeof(Multi_TeamSoldier), _normalUnit);
        _spawnerByType.Add(typeof(Multi_Projectile), _weapon);
    }

    void Start()
    {
        // 마스터 클라이언트만 풀링할 수 있고 나머지는 풀링을 요청하고 오브젝트를 받는 형식
        if (!PhotonNetwork.IsMasterClient) return;

        _normalEnemy.Init();
        _bossEnemy.Init();
        _towerEnemy.Init();
        _normalUnit.Init();
        _weapon.Init();
    }

    T GetOrAddChildComponent<T>() where T : Component
    {
        T component = GetComponentInChildren<T>();
        if (component == null)
        {
            component = new GameObject(typeof(T).Name).AddComponent<T>();
            component.transform.SetParent(transform);
        }

        return component;
    }
}