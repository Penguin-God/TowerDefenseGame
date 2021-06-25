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

        GameManager.instance.ChangeBGM(GameManager.instance.bossbgmClip);
    }

    public override void Dead()
    {
        enemySpawn.currentBossList.Remove(gameObject);
        enemySpawn.bossRespawn = false;
        parent.position = new Vector3(500, 500, 500);
        isDead = true;

        GameManager.instance.ChangeBGM(GameManager.instance.bgmClip);

        if (enemySpawn.bossLevel == 5)
        {
            GameManager.instance.Clear();
            return;
        }

        enemySpawn.enemyAudioSource.PlayOneShot(enemySpawn.bossDeadClip, 0.7f);
        enemySpawn.shop.OnShop(enemySpawn.bossLevel, enemySpawn.shop.bossShopWeighDictionary);
        enemySpawn.shop.SetGuideText("보스를 처치하였습니다");

        GetBossReword(enemySpawn.bossRewordGold, enemySpawn.bossRewordFood);

        //if (enemySpawn.bossLevel == 1 || enemySpawn.bossLevel == 2)
        //{
        //    enemySpawn.createDefenser.CreateSoldier(7, 1);
        //}
        //else if (enemySpawn.bossLevel == 3 || enemySpawn.bossLevel == 4)
        //{
        //    enemySpawn.createDefenser.CreateSoldier(7, 2);
        //}



        switch (enemySpawn.bossLevel)
        {
            case 1:
            case 2:
                enemySpawn.createDefenser.CreateSoldier(7, 1);
                break;
            case 3: 
            case 4:
                enemySpawn.createDefenser.CreateSoldier(7, 2);
                break;
        }

        Destroy(transform.parent.gameObject);
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
