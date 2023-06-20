using Photon.Pun;
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

    public bool EnterStoryWorld { get; private set; }
    readonly WorldSpawnPositionCalculator _positionCalculator = new WorldSpawnPositionCalculator(20, -10, 45, 20);
    protected void ChangeWorld(GameObject tpObject)
    {
        Managers.Effect.PlayParticle("UnitTpEffect", tpObject.transform.position + (Vector3.up * 3));
        tpObject.SetActive(false);
        tpObject.transform.position = 
            EnterStoryWorld ? _positionCalculator.CalculateWorldPostion(WorldPos) : _positionCalculator.CalculateEnemyTowerPostion(EnemyTowerPos);
        tpObject.SetActive(true);
        EnterStoryWorld = !EnterStoryWorld;
        Managers.Sound.PlayEffect(EffectSoundType.UnitTp);
    }
}
