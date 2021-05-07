using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTower : Enemy
{
    private void Awake()
    {
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        TeamSoldier teamSoldier = other.GetComponent<TeamSoldier>();
        if (teamSoldier != null)
        {
            teamSoldier.target = this.transform;
        }
    }
}
