using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_WorldPosUtility : MonoBehaviour
{
    private static Multi_WorldPosUtility instance;
    public static Multi_WorldPosUtility Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Multi_WorldPosUtility>();
                if (instance == null)
                    instance = new GameObject("Multi_WorldPosUtility").AddComponent<Multi_WorldPosUtility>();
            }

            return instance;
        }
    }

    void Awake()
    {
        offset = towerPositionRange.transform.position - Multi_Data.instance.EnemyTowerWorldPosition;
        enemyTowerSpawnRange_X = towerPositionRange.bounds.size.x / 2;
        enemyTowerSpawnRange_Z = towerPositionRange.bounds.size.z / 2;
    }


    Vector3 offset;
    [SerializeField] float spawnRange = 20;
    [SerializeField] BoxCollider towerPositionRange = null;
    [SerializeField] float enemyTowerSpawnRange_X;
    [SerializeField] float enemyTowerSpawnRange_Z;

    public Vector3 GetUnitSpawnPositon() => GetRandomPos_InRange(Multi_Data.instance.WorldPostion, spawnRange);
    public Vector3 GetUnitSpawnPositon(int id) => GetRandomPos_InRange(Multi_Data.instance.GetWorldPosition(id), spawnRange);

    public Vector3 GetEnemyTower_TP_Position()
                    => GetRandomPos_InRange(Multi_Data.instance.EnemyTowerWorldPosition, enemyTowerSpawnRange_X, enemyTowerSpawnRange_Z);
    public Vector3 GetEnemyTower_TP_Position(int id)
                => GetRandomPos_InRange(Multi_Data.instance.EnemyTowerWorldPositions[id] + offset, enemyTowerSpawnRange_X, enemyTowerSpawnRange_Z);

    Vector3 GetRandomPos_InRange(Vector3 _pivot, float _range) => GetRandomPos_InRange(_pivot, _range, _range);

    Vector3 GetRandomPos_InRange(Vector3 _pivot, float _xRange, float _zRange)
    {
        float _x = Random.Range(_pivot.x - _xRange, _pivot.x + _xRange);
        float _z = Random.Range(_pivot.z - _zRange, _pivot.z + _zRange);
        Vector3 _randomPos = new Vector3(_x, _pivot.y, _z);
        return _randomPos;
    }
}
