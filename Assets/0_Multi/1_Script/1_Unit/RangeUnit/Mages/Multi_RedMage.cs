﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_RedMage : Multi_Unit_Mage
{
    Multi_RedPassive redPassive = null;

    public override void SetMageAwake()
    {
        skillPoolManager.SettingSkilePool(mageSkillObject, 3, SetHitSkile);
        redPassive = GetComponent<Multi_RedPassive>();
        StartCoroutine(Co_UltimateSkile());
    }

    void SetHitSkile(GameObject skileObj) => StartCoroutine(Co_SetHitSkile(skileObj));

    IEnumerator Co_SetHitSkile(GameObject skileObj)
    {
        yield return new WaitUntil(() => skileObj.GetComponentInChildren<Multi_HitSkill>() != null);
        skileObj.GetComponentInChildren<Multi_HitSkill>().OnHitSkile += (Multi_Enemy enemy) => HitMeteor(enemy);
    }

    void HitMeteor(Multi_Enemy enemy)
    {
        enemy.photonView.RPC("OnStern", RpcTarget.MasterClient, 100, 5);
        enemy.photonView.RPC("OnDamage", RpcTarget.MasterClient, 400000);
    }

    [SerializeField] Vector3 meteorPos = (Vector3.up * 30) + (Vector3.forward * 5);
    public override void MageSkile() => ShootMeteor(transform.position + meteorPos, TargetEnemy);

    void ShootMeteor(Vector3 _pos, Multi_Enemy _enemy)
    {
        // 메테오를 위에 띄우고 바닥에 꽂음. 지면에 닿을 시 폭발하는건 메테오 내부에서 실행됨
        GameObject meteor = UsedSkill(_pos);
        meteor.GetComponent<Multi_Meteor>().OnChase(_enemy);
    }

    [SerializeField] Vector3 ultimateMeteorPos = (Vector3.up * 30) + (Vector3.forward * -5);
    IEnumerator Co_UltimateSkile()
    {
        yield return new WaitUntil(() => isUltimate);

        skillPoolManager.SettingSkilePool(mageSkillObject, 2, SetHitSkile);
        OnUltimateSkile += () => ShootMeteor(transform.position + ultimateMeteorPos, Multi_EnemySpawner.instance.GetRandom_CurrentEnemy());
    }

    // 유닛 강화를 어떻게 적용할지 아직 정하지 않아서 일단 임시로 코드 사용
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
            Change_Unit_AttackCollDown(other.GetComponentInParent<Multi_TeamSoldier>(), redPassive.Get_DownDelayWeigh);
    }

    private void OnTriggerExit(Collider other)
    {
        // redPassive.get_DownDelayWeigh 의 역수 곱해서 공속 되돌림
        if (other.gameObject.layer == 9) 
            Change_Unit_AttackCollDown(other.GetComponentInParent<Multi_TeamSoldier>(), (1 / redPassive.Get_DownDelayWeigh));
    }

    void Change_Unit_AttackCollDown(Multi_TeamSoldier _unit, float rate)
    {
        _unit.attackDelayTime *= rate;
    }
}