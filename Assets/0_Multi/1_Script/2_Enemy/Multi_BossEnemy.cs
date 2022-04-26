using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Multi_BossEnemy : Multi_Enemy
{
    [SerializeField] int level;
    public int Level => level;

    void OnEnable()
    {
        enemyType = EnemyType.Boss;
    }

    public override void Dead()
    {
        base.Dead();
        gameObject.SetActive(false);
    }
}
