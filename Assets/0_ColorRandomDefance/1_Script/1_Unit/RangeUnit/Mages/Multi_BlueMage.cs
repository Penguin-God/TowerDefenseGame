using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_BlueMage : Multi_Unit_Mage
{
    [SerializeField] float passiveSlowPercent;
    [SerializeField] float freezeTime;
    public override void SetMageAwake()
    {
        freezeTime = skillStats[0];
    }

    protected override void MageSkile() => SkillSpawn(transform.position + (Vector3.up * 2)).GetComponent<Multi_HitSkill>().SetHitActoin(FreezeMonster);
    void FreezeMonster(Multi_Enemy monster) => monster.GetComponent<Multi_NormalEnemy>()?.OnFreeze(freezeTime, UnitFlags);
    protected override void PlaySkillSound() => PlaySound(EffectSoundType.BlueMageSkill);
}
