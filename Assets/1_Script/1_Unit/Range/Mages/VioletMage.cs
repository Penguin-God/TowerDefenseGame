using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VioletMage : Unit_Mage
{
    public override void SetMageAwake()
    {
        base.SetMageAwake();
        mageEffectObject.GetComponent<HitSkile>().OnHitSkile += (Enemy enemy) => enemy.EnemyPoisonAttack(25, 8, 0.3f, 120000);
        StartCoroutine(Co_SkilleReinForce());
    }

    GameObject ultimateSKileObj = null;

    void UltimateSkile()
    {
        ultimateSKileObj.SetActive(true);

        Enemy rnad_enemy = EnemySpawn.instance.GetRandom_CurrentEnemy();
        ultimateSKileObj.transform.position = rnad_enemy.transform.position;
        ultimateSKileObj.GetComponent<HitSkile>().OnHitSkile += (Enemy enemy) => enemy.EnemyPoisonAttack(25, 8, 0.3f, 120000);
    }

    IEnumerator Co_SkilleReinForce()
    {
        yield return new WaitUntil(() => isUltimate);
        ultimateSKileObj = Instantiate(mageEffectObject);
        OnUltimateSkile += () => UltimateSkile();
    }

    public override void MageSkile()
    {
        base.MageSkile();
        SetSkilObject(target.position);
    }
}
