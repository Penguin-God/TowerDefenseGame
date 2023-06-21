using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpawnPositionCalculator
{
    readonly float WorldSpawnRange; // 정사각형

    readonly float EnemyTowerOffsetZ;
    readonly float EnemyTowerSpawnRange_X;
    readonly float EnemyTowerSpawnRange_Z;

    public WorldSpawnPositionCalculator(float worldSpawnRange, float enemyTowerOffsetZ, float enemyTowerSpawnRange_X, float enemyTowerSpawnRange_Z)
    {
        WorldSpawnRange = worldSpawnRange;
        EnemyTowerOffsetZ = enemyTowerOffsetZ;
        EnemyTowerSpawnRange_X = enemyTowerSpawnRange_X;
        EnemyTowerSpawnRange_Z = enemyTowerSpawnRange_Z;
    }

    public Vector3 CalculateWorldPostion(Vector3 worldPos) => GetRandomPos_InRange(worldPos, WorldSpawnRange);
    public Vector3 CalculateEnemyTowerPostion(Vector3 towerPos)
        => GetRandomPos_InRange(new Vector3(towerPos.x, towerPos.y, towerPos.z + EnemyTowerOffsetZ), EnemyTowerSpawnRange_X, EnemyTowerSpawnRange_Z);

    Vector3 GetRandomPos_InRange(Vector3 _pivot, float _range) => GetRandomPos_InRange(_pivot, _range, _range);
    Vector3 GetRandomPos_InRange(Vector3 _pivot, float _xRange, float _zRange)
    {
        float _x = Random.Range(_pivot.x - _xRange, _pivot.x + _xRange);
        float _z = Random.Range(_pivot.z - _zRange, _pivot.z + _zRange);
        Vector3 _randomPos = new Vector3(_x, _pivot.y, _z);
        return _randomPos;
    }
}

