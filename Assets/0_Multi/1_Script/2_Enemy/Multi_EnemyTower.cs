using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_EnemyTower : Multi_Enemy
{
    public int level;
    public int rewardGold;
    public int rewardFood;

    [PunRPC]
    protected override void SetStatus(int _hp, float _speed, bool _isDead, int id)
    {
        base.SetStatus(_hp, _speed, _isDead, id);
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
