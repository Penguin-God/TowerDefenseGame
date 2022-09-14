using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_VioletMage : Multi_Unit_Mage
{
    // TODK : 독 관련 구조체 만들기
    [SerializeField] int percent;
    [SerializeField] int count;
    [SerializeField] float delay;
    [SerializeField] int maxDamage;

    protected override void OnAwake()
    {
        base.OnAwake();
        percent = (int)skillStats[0];
        count = (int)skillStats[1];
        delay = skillStats[2];
        maxDamage = (int)skillStats[3];
    }

    protected override void MageSkile()
        => SkillSpawn(target.position + Vector3.up * 2).GetComponent<Multi_HitSkill>().SetHitActoin(Poison);
    protected override void PlaySkillSound() => PlaySound(EffectSoundType.VioletMageSkill);
    void Poison(Multi_Enemy enemy) => enemy.OnPoison_RPC(percent, count, delay, maxDamage, true);
}
