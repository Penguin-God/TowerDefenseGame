﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_OrangeMage : Multi_Unit_Mage
{
    //public override void MageSkile()
    //{
    //    if (pv.IsMine)
    //    {
    //        GameObject _skill = UsedSkill(Vector3.one);
    //        _skill.GetComponent<Multi_OrangeSkill>().OnSkile(target.GetComponentInParent<Multi_Enemy>(), isUltimate, bossDamage);
    //    } 
    //}

    // TODO : 법사 스킬 중에 기본 공격 허용할건지 물어보기
    protected override void _MageSkile()
    {
        SkillSpawn(Vector3.zero).GetComponent<Multi_OrangeSkill>().OnSkile(TargetEnemy, bossDamage);
    }
}