using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_RedMage : Multi_Unit_Mage
{
    [SerializeField] int meteorDamage;
    [SerializeField] float meteorStunTime;

    public override void SetMageAwake()
    {
        meteorDamage = (int)skillStats[0];
        meteorStunTime = skillStats[1];

        GetComponentInChildren<SphereCollider>().radius = skillStats[2];
        attackDelayRate = skillStats[3];
    }

    [SerializeField] Vector3 meteorPos = (Vector3.up * 30) + (Vector3.forward * 5);

    protected override void MageSkile()
    {
        Multi_Meteor meteor = SkillSpawn(transform.position + meteorPos).GetComponent<Multi_Meteor>();
        StartCoroutine(Co_ShotMeteor(meteor));
    }

    IEnumerator Co_ShotMeteor(Multi_Meteor meteor)
    {
        Multi_Enemy tempEnemyPos = TargetEnemy;
        Vector3 tempPos = target.position;
        yield return new WaitForSeconds(1f);

        if (target == null)
            meteor.Shot(null, tempPos, HitMeteor);
        else
            meteor.Shot(TargetEnemy, target.position, HitMeteor);
    }

    protected override void PlaySkillSound() => PlaySound(EffectSoundType.RedMageSkill);

    void HitMeteor(Multi_Enemy enemy)
    {
        SkillAttackToEnemy(enemy, meteorDamage);
        enemy.OnStun_RPC(100, meteorStunTime);
    }

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
