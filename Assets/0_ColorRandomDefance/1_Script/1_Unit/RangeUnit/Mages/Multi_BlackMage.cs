﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_BlackMage : Multi_Unit_Mage
{
    [SerializeField] Transform skileShotPositions = null;
    [SerializeField] int _directionShotDamage;

    protected override void OnAwake()
    {
        base.OnAwake();
        _directionShotDamage = (int)base.skillStats[0];
    }

    protected override void MageSkile()
    {
        //foreach (Transform child in skileShotPositions)
        //    child.GetComponent<ProjectileThrowingUnit>().FlatThrow(GetSkillPath(), child.forward, OnSkillHit);
    }
    protected override void PlaySkillSound() => PlaySound(EffectSoundType.BlackMageSkill);

    void OnSkillHit(Multi_Enemy enemy) => base.SkillAttackToEnemy(enemy, _directionShotDamage);
}
