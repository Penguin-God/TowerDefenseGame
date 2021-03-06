using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Archer : Multi_RangeUnit
{
    [Header("아처 변수")]
    [SerializeField] ProjectileData arrawData;
    [SerializeField] int skillAttackTargetCount = 3;
    private GameObject trail;

    public override void OnAwake()
    {
        trail = GetComponentInChildren<TrailRenderer>().gameObject;
        arrawData = new ProjectileData(Multi_Managers.Data.WeaponDataByUnitFlag[UnitFlags].Paths[0],transform, arrawData.SpawnTransform);
    }

    public override void SetInherenceData()
    {
        skillDamage = (int)(Damage * 1.2f);
    }

    public override void NormalAttack() => StartCoroutine("ArrowAttack");
    IEnumerator ArrowAttack()
    {
        base.StartAttack();

        nav.isStopped = true;
        trail.SetActive(false);
        if (PhotonNetwork.IsMasterClient && target != null && enemyDistance < chaseRange)
        {
            ProjectileShotDelegate.ShotProjectile(arrawData, target, OnHit);
        }
        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.isStopped = false;

        EndAttack();
    }

    public override void SpecialAttack() => StartCoroutine(Special_ArcherAttack());

    // TODO : 아처 스킬 구조 뜯어 고치기
    IEnumerator Special_ArcherAttack()
    {
        base.SpecialAttack();
        nav.angularSpeed = 1;
        trail.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            Transform[] targetArray =
            Multi_EnemyManager.Instance.GetProximateEnemys(transform.position, chaseRange, skillAttackTargetCount, target, GetComponent<RPCable>().UsingId);
            for (int i = 0; i < targetArray.Length; i++)
            {
                ProjectileShotDelegate.ShotProjectile(arrawData, targetArray[i], OnSkileHit);
            }
        }

        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.angularSpeed = 1000;

        SkillCoolDown(skillCoolDownTime);
    }
}
