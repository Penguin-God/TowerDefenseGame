using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTower : Enemy
{
    public int towerLevel;
    public int rewardGold;
    public int rewardFood;
    public bool isRespawn;

    private void Awake()
    {
        Set_RespawnStatus(EnemySpawn.instance.arr_TowersHp[EnemySpawn.instance.currentTowerLevel]);
    }

    public void Set_RespawnStatus(int hp)
    {
        isRespawn = true;
        maxHp = hp;
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
        EnemySpawn.instance.RespawnNextTower(towerLevel, 1.5f);
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
