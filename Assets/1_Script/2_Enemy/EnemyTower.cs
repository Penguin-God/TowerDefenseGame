using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTower : Enemy
{
    public EnemySpawn enemySpawn;
    public int towerLevel;
    public int rewardGold;
    public int rewardFood;
    public bool isRespawn;

    private void Awake()
    {
        isRespawn = true;
        maxHp = enemySpawn.arr_TowersHp[enemySpawn.currentTowerLevel];
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        isDead = false;
        speed = 0;
        maxSpeed = 0;
        dir = Vector3.zero;
    }

    public override void Dead()
    {
        if (isDead) return;

        isDead = true;
        gameObject.SetActive(false);
        transform.position = new Vector3(5000, 5000, 5000);
        GetTowerReword();
        enemySpawn.RespawnNextTower(towerLevel, 1.5f); // 다음 타워 소환
        SetUnitTarget();
    }

    void SetUnitTarget()
    {
        foreach (GameObject unit in UnitManager.instance.currentUnitList)
        {
            if (unit == null) continue;

            TeamSoldier teamSoldier = unit.GetComponent<TeamSoldier>();
            if (teamSoldier.enterStoryWorld) teamSoldier.target = null;
        }
    }

    void GetTowerReword()
    {
        GameManager.instance.Wood += towerLevel; // 테스트용
        GameManager.instance.Iron += towerLevel; // 테스트용

        GameManager.instance.Gold += rewardGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        GameManager.instance.Food += rewardFood;
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }
}
