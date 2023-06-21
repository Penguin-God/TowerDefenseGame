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
    readonly WorldSpawnPositionCalculator _positionCalculator = new WorldSpawnPositionCalculator(20, -10, 45, 2);
    public Vector3 ChangeWorld(GameObject tpObject)
    {
        Vector3 destination = EnterStoryWorld ? _positionCalculator.CalculateWorldPostion(WorldPos) : _positionCalculator.CalculateEnemyTowerPostion(EnemyTowerPos);
        ChangeWorld(tpObject, destination);
        return destination;
    }

    public void ChangeWorld(GameObject tpObject, Vector3 destination)
    {
        Managers.Effect.PlayParticle("UnitTpEffect", tpObject.transform.position + (Vector3.up * 3));
        tpObject.SetActive(false);
        tpObject.transform.position = destination;
        tpObject.SetActive(true);
        EnterStoryWorld = !EnterStoryWorld;
    }
}
