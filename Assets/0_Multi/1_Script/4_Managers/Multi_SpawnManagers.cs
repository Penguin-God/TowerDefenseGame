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

    // TODO : 그냥 다 모노비헤비어 상속 받아서 스폰 각각 스폰할 오브젝트 가지고 있고 알아서 스폰하는 형태로 바꾸기
    [SerializeField] GameObject[] normalEnemys;

    Multi_NormalEnemySpawner _normalEnemy = new Multi_NormalEnemySpawner();
    Multi_BossEnemySpawner _bossEnemy = new Multi_BossEnemySpawner();
    Multi_TowerEnemySpawner _towerEnemy = new Multi_TowerEnemySpawner();
    Multi_NormalUnitSpawner _normalUnit = new Multi_NormalUnitSpawner();

    public static Multi_NormalEnemySpawner NormalEnemy => Instance._normalEnemy;
    public static Multi_BossEnemySpawner BossEnemy => Instance._bossEnemy;
    public static Multi_TowerEnemySpawner TowerEnemy => Instance._towerEnemy;
    public static Multi_NormalUnitSpawner NormalUnit => Instance._normalUnit;

    void Start()
    {
        if (!photonView.IsMine) return;


    }
}
