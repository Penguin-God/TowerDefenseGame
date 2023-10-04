using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_RedMage : Multi_Unit_Mage
{
    [SerializeField] float _damRate;
    [SerializeField] float meteorStunTime;
    MeteorController _meteorController;

    public override void SetMageAwake()
    {
        _meteorController = gameObject.AddComponent<MeteorController>();
        _damRate = skillStats[0];
        meteorStunTime = skillStats[1];
    }

    [SerializeField] Vector3 meteorPos = (Vector3.up * 30) + (Vector3.forward * 5);
    Vector3 CalculateMeteorSawpnPos() => transform.position + meteorPos;
    protected override void MageSkile() => _meteorController.ShotMeteor(TargetEnemy, CalculateSkillDamage(_damRate), meteorStunTime, CalculateMeteorSawpnPos());
    protected override void PlaySkillSound() => PlaySound(EffectSoundType.RedMageSkill);
}
