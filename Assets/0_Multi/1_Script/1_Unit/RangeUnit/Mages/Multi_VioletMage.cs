using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_VioletMage : Multi_Unit_Mage
{
    public override void SetMageAwake()
    {
        skillPoolManager.SettingSkilePool(mageSkillObject, 3, SetSkill);
        StartCoroutine(Co_SkilleReinForce());
    }

    void SetSkill(GameObject _skill) => _skill.GetComponent<HitSkile>().OnHitSkile += (Enemy enemy) => enemy.EnemyPoisonAttack(25, 8, 0.3f, 120000);

    IEnumerator Co_SkilleReinForce()
    {
        yield return new WaitUntil(() => isUltimate);
        skillPoolManager.SettingSkilePool(mageSkillObject, 3, SetSkill);
        OnUltimateSkile += () => skillPoolManager.UsedSkill(EnemySpawn.instance.GetRandom_CurrentEnemy().transform.position);
    }

    public override void MageSkile()
    {
        base.MageSkile();
        skillPoolManager.UsedSkill(target.position);
    }
}
