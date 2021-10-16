using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBoss : NomalEnemy
{
    private void Awake()
    {
        nomalEnemy = GetComponent<NomalEnemy>();
        enemySpawn = GetComponentInParent<EnemySpawn>();
        parent = transform.parent.GetComponent<Transform>();
        parentRigidbody = GetComponentInParent<Rigidbody>();

        enemySpawn.currentBossList.Add(gameObject);
        SetUnitTarget();

        GameManager.instance.ChangeBGM(GameManager.instance.bossbgmClip);
    }

    void SetUnitTarget()
    {
        foreach (GameObject unit in UnitManager.instance.currentUnitList)
        {
            if (unit == null) continue;

            TeamSoldier teamSoldier = unit.GetComponent<TeamSoldier>();
            if (!teamSoldier.enterStoryWorld)
            {
                teamSoldier.UpdateTarget();
            }
        }
    }

    public override void Dead()
    {
        SetDeadVariable();
        SetUnitTarget();

        GameManager.instance.ChangeBGM(GameManager.instance.bgmClip);

        if (enemySpawn.stageNumber >= enemySpawn.maxStage && !GameManager.instance.isChallenge) // 게임클리어
        {
            GameManager.instance.Clear();
            return;
        }

        GetBossReword(enemySpawn.bossRewordGold, enemySpawn.bossRewordFood); // 재화보상
        ShowBossShop();
        Get_UunitReword();

        Destroy(parent.gameObject);
    }

    void SetDeadVariable()
    {
        enemySpawn.currentBossList.Remove(gameObject);
        enemySpawn.bossRespawn = false;
        parent.gameObject.SetActive(false);
        parent.position = new Vector3(500, 500, 500);
        isDead = true;
    }

    void ShowBossShop()
    {
        enemySpawn.enemyAudioSource.PlayOneShot(enemySpawn.bossDeadClip, 0.7f);
        enemySpawn.shop.OnShop( (enemySpawn.bossLevel < 5) ? enemySpawn.bossLevel : 4, enemySpawn.shop.bossShopWeighDictionary);
        enemySpawn.shop.SetGuideText("보스를 처치하였습니다");
    }

    void GetBossReword(int rewardGold, int rewardFood)
    {
        GameManager.instance.Gold += rewardGold * Mathf.FloorToInt(enemySpawn.stageNumber / 10);
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        GameManager.instance.Food += rewardFood * Mathf.FloorToInt(enemySpawn.stageNumber / 10);
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }

    void Get_UunitReword()
    {
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
    }
}
