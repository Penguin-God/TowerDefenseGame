using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_OrangeMage : Multi_Unit_Mage
{
    [SerializeField] int count;
    [SerializeField] float percent;

    protected override void OnAwake()
    {
        base.OnAwake();
        count = (int)skillStats[0];
        percent = skillStats[1];
    }

    //protected override void MageSkile()
    //    => SkillSpawn(Vector3.zero).GetComponent<Multi_OrangeSkill>().OnSkile(TargetEnemy, BossDamage, count, percent);
}