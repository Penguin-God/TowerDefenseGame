using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_EnemyTower : Multi_Enemy
{
    public int level;
    public int rewardGold;
    public int rewardFood;
    public bool isRespawn;

    [PunRPC]
    public override void Setup(int _hp, float _speed)
    {
        isRespawn = true;
        maxHp = _hp;
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        isDead = false;
        speed = 0;
        maxSpeed = 0;
        dir = Vector3.zero;
    }

    [PunRPC]
    public void Spawn(Vector3 _pos)
    {
        transform.position = _pos;
        gameObject.SetActive(true);
    }

    public override void Dead()
    {
        base.Dead();

        isRespawn = false;
        gameObject.SetActive(false);
        transform.position = new Vector3(300, 300, 300);
        GetTowerReword();
        //UnitManager.instance.UpdateTarget_CurrnetStroyWolrdUnit(null);
    }

    void GetTowerReword()
    {
        Multi_GameManager.instance.AddGold(rewardGold);
        Multi_GameManager.instance.AddFood(rewardFood);
    }
}
