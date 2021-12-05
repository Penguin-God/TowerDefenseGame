using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMage : Unit_Mage
{
    public override void SetMageAwake()
    {
        base.SetMageAwake();
        StartCoroutine(Co_SetHitSkile(mageEffectObject));
        StartCoroutine(Co_UltimateSkile());
    }

    IEnumerator Co_SetHitSkile(GameObject skileObj)
    {
        yield return new WaitUntil(() => skileObj.GetComponentInChildren<HitSkile>() != null);
        skileObj.GetComponentInChildren<HitSkile>().OnHitSkile += (Enemy enemy) => OnHitSkile(enemy);
    }

    GameObject ultimateSKileObj = null;
    IEnumerator Co_UltimateSkile()
    {
        yield return new WaitUntil(() => isUltimate);
        ultimateSKileObj = Instantiate(mageEffectObject);
        StartCoroutine(Co_SetHitSkile(ultimateSKileObj));
        OnUltimateSkile += () => UltimateSkile();
    }

    void UltimateSkile()
    {
        ultimateSKileObj.transform.position = transform.position + (Vector3.up * 30);
        ultimateSKileObj.SetActive(true);
        Enemy enemy = EnemySpawn.instance.GetRandom_CurrentEnemy();
        ultimateSKileObj.GetComponent<Meteor>().OnChase(enemy);
    }

    public override void MageSkile()
    {
        base.MageSkile();
        SetSkilObject(transform.position + (Vector3.up * 30));
        mageEffectObject.GetComponent<Meteor>().OnChase(target.GetComponent<Enemy>());
    }

    void OnHitSkile(Enemy enemy)
    {
        enemy.EnemyStern(100, 5);
        enemy.OnDamage(400000);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9) Change_Unit_AttackDelayTime(other.gameObject, 0.5f);
    }
    
    private void OnTriggerExit(Collider other)
    { 
        if (other.gameObject.layer == 9) Change_Unit_AttackDelayTime(other.gameObject, 2f);
    }

    void Change_Unit_AttackDelayTime(GameObject unitObject, float rate)
    {
        unitObject.GetComponent<TeamSoldier>().attackDelayTime *= rate;
    }
}
