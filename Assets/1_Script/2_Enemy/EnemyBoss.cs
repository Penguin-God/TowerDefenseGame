using System.Collections;
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
                teamSoldier.target = transform;
        }
    }

    public override void Dead()
    {
        //enemySpawn.shop.OnEnvetShop();
        enemySpawn.currentBossList.Remove(gameObject);
        enemySpawn.bossRespawn = false;
        transform.parent.transform.position = new Vector3(500, 500, 500);
        isDead = true;
        GetBossReword(enemySpawn.bossRewordGold, enemySpawn.bossRewordFood);
        Destroy(transform.parent.gameObject);
    }

    void GetBossReword(int rewardGold, int rewardFood)
    {
        GameManager.instance.Gold += rewardGold * Mathf.FloorToInt(enemySpawn.stageNumber / 10);
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


        GameManager.instance.Food += rewardFood * Mathf.FloorToInt(enemySpawn.stageNumber / 10);
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }
}
