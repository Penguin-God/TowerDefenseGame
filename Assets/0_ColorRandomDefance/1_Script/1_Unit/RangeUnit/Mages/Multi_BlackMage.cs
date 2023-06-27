using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_BlackMage : Multi_Unit_Mage
{
    [SerializeField] Transform skileShotPositions = null;
    [SerializeField] float _damRate;

    protected override void OnAwake()
    {
        base.OnAwake();
        _damRate = base.skillStats[0];
    }

    protected override void MageSkile()
    {
        for (int i = 0; i < skileShotPositions.childCount; i++)
        {
            Transform instantTransform = skileShotPositions.GetChild(i);
            ProjectileShotDelegate.ShotProjectile(skillData, instantTransform.forward, OnSkillHit);
        }
    }
    protected override void PlaySkillSound() => PlaySound(EffectSoundType.BlackMageSkill);

    void OnSkillHit(Multi_Enemy enemy) => base.SkillAttack(enemy, CalculateSkillDamage(_damRate));
}
