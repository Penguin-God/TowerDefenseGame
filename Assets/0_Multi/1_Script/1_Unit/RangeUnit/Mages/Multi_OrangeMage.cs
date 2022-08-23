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

    // TODO : 법사 스킬 중에 기본 공격 허용할건지 물어보기
    protected override void MageSkile()
        => SkillSpawn(Vector3.zero).GetComponent<Multi_OrangeSkill>().OnSkile(TargetEnemy, BossDamage, count, percent);
}