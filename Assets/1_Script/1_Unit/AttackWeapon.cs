﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWeapon : MonoBehaviour
{
    TeamSoldier teamSoldier;
    public int damage;
    public GameObject attackUnit; // 무기와 적이 충돌할때 Enemy script에서 관련 정보를 가져가도록 하기위한 변수 
    // attackUnit 변수 TeamSolider Script 변수로 만들기

    private void Start()
    {
        teamSoldier = attackUnit.GetComponent<TeamSoldier>();
        this.damage = teamSoldier.damage;
        if(teamSoldier.unitType == TeamSoldier.Type.archer || teamSoldier.unitType == TeamSoldier.Type.mage) Destroy(gameObject, 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            if(teamSoldier.unitType == TeamSoldier.Type.archer)
            {
                teamSoldier.target.GetComponent<Enemy>().OnDamage(damage);
                if (!teamSoldier.target.GetComponent<Enemy>().isDead)
                    attackUnit.GetComponent<RangeUnit>().RangeUnit_PassiveAttack();
            }
            else if(teamSoldier.unitType == TeamSoldier.Type.mage)
            {
                other.gameObject.GetComponent<Enemy>().OnDamage(damage);
                if (!other.gameObject.GetComponent<Enemy>().isDead)
                    attackUnit.GetComponent<RangeUnit>().RangeUnit_PassiveAttack();
            }

            if(teamSoldier.unitType == TeamSoldier.Type.archer) Destroy(gameObject);
        }
    }
}
