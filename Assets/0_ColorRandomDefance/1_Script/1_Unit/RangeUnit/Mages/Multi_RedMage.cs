using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_RedMage : Multi_Unit_Mage
{
    [SerializeField] float _damRate;
    [SerializeField] float meteorStunTime;
    MeteorController _meteorController = new MeteorController();

    public override void SetMageAwake()
    {
        _damRate = skillStats[0];
        meteorStunTime = skillStats[1];

        GetComponentInChildren<SphereCollider>().radius = skillStats[2];
        attackDelayRate = skillStats[3];
    }

    [SerializeField] Vector3 meteorPos = (Vector3.up * 30) + (Vector3.forward * 5);
    Vector3 CalculateMeteorSawpnPos() => transform.position + meteorPos;
    protected override void MageSkile() => StartCoroutine(_meteorController.Co_ShotMeteor(TargetEnemy, CalculateSkillDamage(_damRate), meteorStunTime, CalculateMeteorSawpnPos()));
    protected override void PlaySkillSound() => PlaySound(EffectSoundType.RedMageSkill);

    // passive
    [SerializeField] float attackDelayRate;
    [SerializeField] List<Multi_TeamSoldier> _passiveTargets = new List<Multi_TeamSoldier>();
    void OnTriggerEnter(Collider other)
    {
        var unit = other.GetComponentInParent<Multi_TeamSoldier>();
        if (unit != null && _passiveTargets.Contains(unit) == false)
        {
            Change_Unit_AttackCollDown(unit, attackDelayRate);
            _passiveTargets.Add(unit);
        }
    }

    void OnTriggerExit(Collider other) // redPassive.get_DownDelayWeigh 의 역수 곱해서 공속 되돌림
    {
        var unit = other.GetComponentInParent<Multi_TeamSoldier>();
        if (unit != null && _passiveTargets.Contains(unit))
        {
            Change_Unit_AttackCollDown(unit, (1 / attackDelayRate));
            _passiveTargets.Remove(unit);
        }
    }

    void Change_Unit_AttackCollDown(Multi_TeamSoldier _unit, float rate)
    {
        _unit.AttackDelayTime *= rate;
    }
}
