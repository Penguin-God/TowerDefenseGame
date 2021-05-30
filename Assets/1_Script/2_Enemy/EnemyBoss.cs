﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : NomalEnemy
{
    private void Awake()
    {
        nomalEnemy = GetComponent<NomalEnemy>();
        enemySpawn = GetComponentInParent<EnemySpawn>();
        enemySpawn.currentBossList.Add(gameObject);
        parent = transform.parent.GetComponent<Transform>();
        parentRigidbody = GetComponentInParent<Rigidbody>();

        foreach (GameObject unit in UnitManager.instance.currentUnitList)
        {
            if (unit == null) continue;

            TeamSoldier teamSoldier = unit.GetComponent<TeamSoldier>();
            if(!teamSoldier.enterStoryWorld)
                unit.GetComponent<TeamSoldier>().target = transform;
        }
    }

    public override void Dead()
    {
        enemySpawn.currentBossList.Remove(gameObject);
        enemySpawn.bossRespawn = false;
        transform.parent.transform.position = new Vector3(500, 500, 500);
        isDead = true;
        GetBossReword(10, 1);
        Destroy(transform.parent.gameObject);
        //Debug.Log("Hello World");
    }

    void GetBossReword(int rewardGold, int rewardFood)
    {
        GameManager.instance.Gold += rewardGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        GameManager.instance.Food += rewardFood;
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }
}
