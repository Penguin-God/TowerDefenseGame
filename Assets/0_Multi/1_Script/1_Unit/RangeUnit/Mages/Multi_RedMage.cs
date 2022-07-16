using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_RedMage : Multi_Unit_Mage
{
    Multi_RedPassive redPassive = null;

    public override void SetMageAwake()
    {
        redPassive = GetComponent<Multi_RedPassive>();
    }

    void HitMeteor(Multi_Enemy enemy)
    {
        enemy.OnDamage(400000);
        enemy.OnStun(RpcTarget.MasterClient, 100, 5f);
    }

    [SerializeField] Vector3 meteorPos = (Vector3.up * 30) + (Vector3.forward * 5);

    protected override void _MageSkile()
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

    // 패시브
    // 최적화 때문에 Raycasting으로 바꾸기? : 효과 확실하면
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Multi_TeamSoldier>() != null)
            Change_Unit_AttackCollDown(other.GetComponentInParent<Multi_TeamSoldier>(), redPassive.Get_DownDelayWeigh);
    }

    private void OnTriggerExit(Collider other)
    {
        // redPassive.get_DownDelayWeigh 의 역수 곱해서 공속 되돌림
        if (other.GetComponentInParent<Multi_TeamSoldier>() != null) 
            Change_Unit_AttackCollDown(other.GetComponentInParent<Multi_TeamSoldier>(), (1 / redPassive.Get_DownDelayWeigh));
    }

    void Change_Unit_AttackCollDown(Multi_TeamSoldier _unit, float rate)
    {
        _unit.attackDelayTime *= rate;
    }
}
