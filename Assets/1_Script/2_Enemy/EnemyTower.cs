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

    private void Awake()
    {
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
    }

    public override void Dead()
    {
        if (isDead) return;

        isDead = true;
        GetTowerReword();
        enemySpawn.SetNextTower(towerLevel);
    }

    void GetTowerReword()
    {
        GameManager.instance.Gold += rewardGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        GameManager.instance.Food += rewardFood;
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Attack") // 임시
    //    {
    //        AttackWeapon attackWeapon = other.GetComponentInParent<AttackWeapon>();
    //        TeamSoldier teamSoldier = attackWeapon.attackUnit.GetComponent<TeamSoldier>();
    //        if (teamSoldier.unitType == TeamSoldier.Type.archer) Destroy(other.gameObject); // 아처 공격이면 총알 삭제

    //        OnDamage(attackWeapon.damage);
    //    }
    //}
}
