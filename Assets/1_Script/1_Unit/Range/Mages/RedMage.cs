using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMage : Unit_Mage
{
    RedPassive redPassive = null;

    public override void SetMageAwake()
    {
        SettingSkilePool(mageSkillObject, 3, SetHitSkile);
        redPassive = GetComponent<RedPassive>();
        StartCoroutine(Co_UltimateSkile());
    }

    void SetHitSkile(GameObject skileObj) => StartCoroutine(Co_SetHitSkile(skileObj));

    IEnumerator Co_SetHitSkile(GameObject skileObj)
    {
        yield return new WaitUntil(() => skileObj.GetComponentInChildren<HitSkile>() != null);
        skileObj.GetComponentInChildren<HitSkile>().OnHitSkile += (Enemy enemy) => OnHitSkile(enemy);
    }

    IEnumerator Co_UltimateSkile()
    {
        yield return new WaitUntil(() => isUltimate);

        SettingSkilePool(mageSkillObject, 2, SetHitSkile);
        OnUltimateSkile += () => ShootMeteor(transform.position + (Vector3.up * 30) + (Vector3.forward * -5), EnemySpawn.instance.GetRandom_CurrentEnemy());
    }
    
    void ShootMeteor(Vector3 _pos ,Enemy _enemy)
    {
        // 메테오를 위에 띄우고 적을 추적하게함 지면에 닿으면 폭발하는건 내부에서 실행됨
        GameObject meteor = UsedSkill(_pos);
        meteor.GetComponent<Meteor>().OnChase(_enemy);
    }

    public override void MageSkile()
    {
        base.MageSkile();
        ShootMeteor(transform.position + (Vector3.up * 30) + (Vector3.forward * 5), TargetEnemy);
    }

    void OnHitSkile(Enemy enemy)
    {
        enemy.EnemyStern(100, 5);
        enemy.OnDamage(400000);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9) Change_Unit_AttackCollDown(other.gameObject, redPassive.Get_DownDelayWeigh);
    }
    
    private void OnTriggerExit(Collider other)
    { 
        // redPassive.get_DownDelayWeigh 의 역수 곱해서 공속 되돌림
        if (other.gameObject.layer == 9) Change_Unit_AttackCollDown(other.gameObject, (1 / redPassive.Get_DownDelayWeigh));
    }

    void Change_Unit_AttackCollDown(GameObject unitObject, float rate)
    {
        unitObject.GetComponent<TeamSoldier>().attackDelayTime *= rate;
    }
}