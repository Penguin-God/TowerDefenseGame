using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMage : Unit_Mage
{
    BluePassive bluePassive = null;

    public override void SetMageAwake()
    {
        base.SetMageAwake();
        bluePassive = GetComponent<BluePassive>();
        GetComponent<SphereCollider>().radius = bluePassive.get_ColliderRange;
        bluePassive.OnBeefup += () => GetComponent<SphereCollider>().radius = bluePassive.get_ColliderRange;

        mageEffectObject.GetComponent<HitSkile>().OnHitSkile += (Enemy enemy) => enemy.EnemyFreeze(5f);
        StartCoroutine(Co_SkilleReinForce());        
    }

    IEnumerator Co_SkilleReinForce()
    {
        yield return new WaitUntil(() => isUltimate);
        mageEffectObject.GetComponent<HitSkile>().OnHitSkile += (Enemy enemy) => enemy.OnDamage(20000);
    }

    public override void MageSkile()
    {
        base.MageSkile();
        SetSkilObject(transform.position + (Vector3.up * 2));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<NomalEnemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.EnemySlow(bluePassive.get_SlowPercent, -1f); // 나가기 전까진 무한 슬로우
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<NomalEnemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.ExitSlow();
        }
    }
}
