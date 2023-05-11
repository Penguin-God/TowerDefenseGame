﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_BlackMage : Multi_Unit_Mage
{
    [SerializeField] Transform skileShotPositions = null;
    [SerializeField] int _directionShotDamage;
    List<ProjectileThrowingUnit> _throwers;
    protected override void OnAwake()
    {
        base.OnAwake();
        _directionShotDamage = (int)base.skillStats[0];
        foreach (Transform child in skileShotPositions)
        {
            var thorwer = gameObject.AddComponent<ProjectileThrowingUnit>();
            thorwer.SetInfo(skillData.WeaponPath, child);
            _throwers.Add(thorwer);
        }
    }

    protected override void MageSkile()
    {
        MultiDirectionShot(skileShotPositions);
    }
    protected override void PlaySkillSound() => PlaySound(EffectSoundType.BlackMageSkill);

    void OnSkillHit(Multi_Enemy enemy) => base.SkillAttackToEnemy(enemy, _directionShotDamage);

    void MultiDirectionShot(Transform directions)
    {
        for (int i = 0; i < directions.childCount; i++)
        {
            Transform instantTransform = directions.GetChild(i);
            // _throwers[i].Throw(instantTransform.forward, OnSkillHit);
        }
    }
}
