using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    Multi_NormalEnemySpawner _normalEnemy;
    Multi_BossEnemySpawner _bossEnemy;
    Multi_TowerEnemySpawner _towerEnemy;
    Multi_NormalUnitSpawner _normalUnit;

    public static Multi_NormalEnemySpawner NormalEnemy => Instance._normalEnemy;
    public static Multi_BossEnemySpawner BossEnemy => Instance._bossEnemy;
    public static Multi_TowerEnemySpawner TowerEnemy => Instance._towerEnemy;
    public static Multi_NormalUnitSpawner NormalUnit => Instance._normalUnit;

    // TODO : 딱 봐도 지옥인데 이거 좀 개선하기
    void Awake()
    {
        _normalEnemy = GetOrAddChildComponent<Multi_NormalEnemySpawner>();
        _bossEnemy = GetOrAddChildComponent<Multi_BossEnemySpawner>();
        _towerEnemy = GetOrAddChildComponent<Multi_TowerEnemySpawner>();
        _normalUnit = GetOrAddChildComponent<Multi_NormalUnitSpawner>();
    }

    public void Init()
    {
        _normalEnemy = GetOrAddChildComponent<Multi_NormalEnemySpawner>();
        _bossEnemy = GetOrAddChildComponent<Multi_BossEnemySpawner>();
        _towerEnemy = GetOrAddChildComponent<Multi_TowerEnemySpawner>();
        _normalUnit = GetOrAddChildComponent<Multi_NormalUnitSpawner>();
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