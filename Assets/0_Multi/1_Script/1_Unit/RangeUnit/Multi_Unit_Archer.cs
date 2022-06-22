using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Archer : Multi_RangeUnit
{
    [Header("아처 변수")]
    [SerializeField] ProjectileData projectileData;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowTransform;
    [SerializeField] int skillAttackTargetCount = 3;
    private GameObject trail;

    public override void OnAwake()
    {
        trail = GetComponentInChildren<TrailRenderer>().gameObject;
        Debug.Assert(projectileData.Original != null && projectileData.SpawnTransform != null, "projectileData가 설정되어 있지 않음");
        projectileData = new ProjectileData(projectileData.Original, projectileData.SpawnTransform, OnSkileHit);
    }

    public override void SetInherenceData()
    {
        skillDamage = (int)(damage * 1.2f);
    }

    public override void NormalAttack() => StartCoroutine("ArrowAttack");
    IEnumerator ArrowAttack()
    {
        base.StartAttack();

        nav.isStopped = true;
        trail.SetActive(false);
        if (target != null && enemyDistance < chaseRange && pv.IsMine)
        {
            ShotProjectile(projectileData, Get_ShootDirection(2f, target));
        }
        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.isStopped = false;

        EndAttack();
    }

    [ContextMenu("set data")]
    void SetDatassss()
    {
        projectileData = new ProjectileData(projectileData.Original, arrowTransform, null);
    }

    public override void SpecialAttack()
    {
        if (!TargetIsNormalEnemy)
        {
            NormalAttack();
            return;
        }
        StartCoroutine(Special_ArcherAttack());
    }

    // TODO : 아처 스킬 구조 뜯어 고치기
    IEnumerator Special_ArcherAttack()
    {
        base.SpecialAttack();
        nav.angularSpeed = 1;
        trail.SetActive(false);

        // TODO : 보스 나와있을 때는 보스만 때려야 함
        Transform[] targetArray = Multi_EnemyManager.Instance.GetProximateEnemys(transform.position, chaseRange, skillAttackTargetCount, target);
        for (int i = 0; i < targetArray.Length; i++)
        {
            ShotProjectile(projectileData, Get_ShootDirection(2f, targetArray[i]));
        }

        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.angularSpeed = 1000;

        SkillCoolDown(skillCoolDownTime);
    }
}
