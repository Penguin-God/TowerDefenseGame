using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : NomalEnemy
{
    private void Awake()
    {
        nomalEnemy = GetComponent<NomalEnemy>();
        enemySpawn = GetComponentInParent<EnemySpawn>();
        parent = transform.parent.GetComponent<Transform>();
        parentRigidbody = GetComponentInParent<Rigidbody>();

        enemySpawn.currentBossList.Add(gameObject);
        foreach (GameObject unit in UnitManager.instance.currentUnitList)
        {
            if (unit == null) continue;

            TeamSoldier teamSoldier = unit.GetComponent<TeamSoldier>();
            if (!teamSoldier.enterStoryWorld) 
            {
                teamSoldier.SetChaseSetting(gameObject);
            }
        }

        GameManager.instance.ChangeBGM(GameManager.instance.bossbgmClip);
    }

    public override void Dead()
    {
        enemySpawn.currentBossList.Remove(gameObject);
        enemySpawn.bossRespawn = false;
        parent.gameObject.SetActive(false);
        parent.position = new Vector3(500, 500, 500);
        isDead = true;

        foreach (GameObject unit in UnitManager.instance.currentUnitList)
        {
            if (unit == null) continue;

            TeamSoldier teamSoldier = unit.GetComponent<TeamSoldier>();
            if (!teamSoldier.enterStoryWorld)
                teamSoldier.UpdateTarget();
        }

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
    }

    void GetBossReword(int rewardGold, int rewardFood)
    {
        GameManager.instance.Gold += rewardGold * Mathf.FloorToInt(enemySpawn.stageNumber / 10);
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


        GameManager.instance.Food += rewardFood * Mathf.FloorToInt(enemySpawn.stageNumber / 10);
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }
}
