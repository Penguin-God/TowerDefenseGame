﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_OrangeMage : Multi_Unit_Mage
{
    // TODO : 법사 스킬 중에 기본 공격 허용할건지 물어보기
    protected override void MageSkile()
    {
        // TODO : 적 죽으면 멈추기
        SkillSpawn(Vector3.zero).GetComponent<Multi_OrangeSkill>().OnSkile(TargetEnemy, BossDamage);
    }
}