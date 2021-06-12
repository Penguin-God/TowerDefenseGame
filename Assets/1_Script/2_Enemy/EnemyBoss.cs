using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : NomalEnemy
{

    public CreateDefenser createDefenser;
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
        enemySpawn.shop.OnEnvetShop();
        enemySpawn.currentBossList.Remove(gameObject);
        enemySpawn.bossRespawn = false;
        transform.parent.transform.position = new Vector3(500, 500, 500);
        isDead = true;
        GetBossReword(enemySpawn.bossRewordGold, enemySpawn.bossRewordFood);
        Destroy(transform.parent.gameObject);

        if (enemySpawn.bossLevel == 1 || enemySpawn.bossLevel == 2)
        {
            createDefenser.CreateSoldier(7, 1);
        }
        else if (enemySpawn.bossLevel == 3 || enemySpawn.bossLevel == 4)
        {
            createDefenser.CreateSoldier(7, 2);
        }
        // enemySpawn.bossLevel  //보스 레밸
    }

    void GetBossReword(int rewardGold, int rewardFood)
    {
        GameManager.instance.Gold += rewardGold * Mathf.FloorToInt(enemySpawn.stageNumber / 10);
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


        GameManager.instance.Food += rewardFood * Mathf.FloorToInt(enemySpawn.stageNumber / 10);
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }
}
