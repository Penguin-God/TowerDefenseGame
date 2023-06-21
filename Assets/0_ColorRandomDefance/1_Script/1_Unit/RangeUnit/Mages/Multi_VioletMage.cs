using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_VioletMage : Multi_Unit_Mage
{
    [SerializeField] int poisonCount;
    [SerializeField] float _damRate;

    protected override void OnAwake()
    {
        base.OnAwake();
        poisonCount = (int)skillStats[0];
        _damRate = skillStats[1];
    }

    protected override void MageSkile() => SkillSpawn(target.position + Vector3.up * 2).GetComponent<Multi_HitSkill>().SetHitActoin(Poison);
    protected override void PlaySkillSound() => PlaySound(EffectSoundType.VioletMageSkill);
    void Poison(Multi_Enemy enemy) => enemy.OnPoison_RPC(poisonCount, CalculateSkillDamage(_damRate), true);
}
