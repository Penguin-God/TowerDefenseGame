using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldChangeController
{
    readonly Vector3 WorldPos;
    readonly Vector3 EnemyTowerPos;

    public WorldChangeController(Vector3 worldPos, Vector3 enemyTowerPos)
    {
        WorldPos =  worldPos;
        EnemyTowerPos = enemyTowerPos;
    }

    public bool EnterStoryWorld { get; set; }
    //public Vector3 ChangeWorld(GameObject tpObject)
    //{
    //    Vector3 destination = EnterStoryWorld ? SpawnPositionCalculator.CalculateWorldSpawnPostion(WorldPos) : SpawnPositionCalculator.CalculateTowerWolrdSpawnPostion(EnemyTowerPos);
    //    ChangeWorld(tpObject, destination);
    //    return destination;
    //}
    // public Vector3 GetTpPos() => EnterStoryWorld ? SpawnPositionCalculator.CalculateWorldSpawnPostion(WorldPos) : SpawnPositionCalculator.CalculateTowerWolrdSpawnPostion(EnemyTowerPos);
    
    public void ChangeWorld(GameObject tpObject, Vector3 destination)
    {
        Managers.Effect.PlayOneShotEffect("UnitTpEffect", tpObject.transform.position + (Vector3.up * 3));
        tpObject.SetActive(false);
        tpObject.transform.position = destination;
        tpObject.SetActive(true);
        EnterStoryWorld = !EnterStoryWorld;
    }
}
