using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTower : Enemy
{
    public int level;
    public int rewardGold;
    public int rewardFood;
    public bool isRespawn;

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
        base.Dead();

        isRespawn = false;
        gameObject.SetActive(false);
        transform.position = new Vector3(5000, 5000, 5000);
        GetTowerReword();
        UnitManager.instance.UpdateTarget_CurrnetStroyWolrdUnit(null);
    }

    //void SetUnitTarget()
    //{
    //    foreach (GameObject unit in UnitManager.instance.CurrentUnitList)
    //    {
    //        if (unit == null) continue;

    //        TeamSoldier teamSoldier = unit.GetComponent<TeamSoldier>();
    //        if (teamSoldier.enterStoryWorld) teamSoldier.target = null;
    //    }
    //}

    void GetTowerReword()
    {
        GameManager.instance.Wood += level; // 테스트용
        GameManager.instance.Iron += level; // 테스트용

        GameManager.instance.Gold += rewardGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        GameManager.instance.Food += rewardFood;
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }
}
