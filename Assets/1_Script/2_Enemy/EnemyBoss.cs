using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : NomalEnemy
{

    public override void Dead()
    {
        transform.parent.transform.position = new Vector3(500, 500, 500);
        isDead = true;
        GetBossReword(10, 1);
        Destroy(transform.parent.gameObject);
        Debug.Log("Hello World");
    }

    void GetBossReword(int rewardGold, int rewardFood)
    {
        GameManager.instance.Gold += rewardGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        GameManager.instance.Food += rewardFood;
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }
}
