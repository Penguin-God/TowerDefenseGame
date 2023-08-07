using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnPositionCalculator
{
    const float WolrdRange = 10f;
    static readonly RandomPositionCalculator _randomPositionCalculator = new RandomPositionCalculator();
    public static Vector3 CalculateWorldSpawnPostion() => CalculateWorldSpawnPostion(PlayerIdManager.Id);
    public static Vector3 CalculateWorldSpawnPostion(byte id) => CalculateWorldSpawnPostion(Multi_Data.instance.GetWorldPosition(id));
    public static Vector3 CalculateWorldSpawnPostion(Vector3 pivot) => _randomPositionCalculator.CalculateRandomPosInRange(pivot, WolrdRange);


    const float TowerOffSetZ = -22.5f;
    const float TowerRangeX = 40f;
    const float TowerRangeZ = 2.5f;
    public static Vector3 CalculateTowerWolrdSpawnPostion(Vector3 pivot)
        => _randomPositionCalculator.CalculateRandomPosInRange(new Vector3(pivot.x, pivot.y, pivot.z + TowerOffSetZ), TowerRangeX, TowerRangeZ);
}
