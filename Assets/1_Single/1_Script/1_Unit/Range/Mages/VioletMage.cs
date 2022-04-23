using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VioletMage : Unit_Mage
{
    public override void SetMageAwake()
    {
        SettingSkilePool(mageSkillObject, 3, SetSkill);
        StartCoroutine(Co_SkilleReinForce());
    }

    void SetSkill(GameObject _skill) => _skill.GetComponent<HitSkile>().OnHitSkile += (Enemy enemy) => enemy.EnemyPoisonAttack(25, 8, 0.3f, 120000);

    IEnumerator Co_SkilleReinForce()
    {
        yield return new WaitUntil(() => isUltimate);
        SettingSkilePool(mageSkillObject, 3, SetSkill);
        OnUltimateSkile += () => UsedSkill(EnemySpawn.instance.GetRandom_CurrentEnemy().transform.position);
    }

    public override void MageSkile()
    {
        base.MageSkile();
        UsedSkill(target.position);
    }
}
