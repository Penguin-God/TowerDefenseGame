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
        enemySpawn.RespawnNextTower(towerLevel, 0.5f); // 다음 타워 소환
    }

    void GetTowerReword()
    {
        GameManager.instance.Gold += rewardGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        GameManager.instance.Food += rewardFood;
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }
}
