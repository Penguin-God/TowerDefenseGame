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

    protected override void OnAwake()
    {
        trail = GetComponentInChildren<TrailRenderer>().gameObject;
        arrawData = new ProjectileData(Multi_Managers.Data.WeaponDataByUnitFlag[UnitFlags].Paths[0],transform, arrawData.SpawnTransform);
        normalAttackSound = EffectSoundType.ArcherAttack;
    }

    public override void SetSkillDamage()
    {
        skillDamage = (int)(Damage * 1.2f);
    }

    public override void NormalAttack() => StartCoroutine("ArrowAttack");
    IEnumerator ArrowAttack()
    {
        base.StartAttack();

        LockMove();
        trail.SetActive(false);
        if (PhotonNetwork.IsMasterClient && target != null && enemyDistance < chaseRange)
        {
            ProjectileShotDelegate.ShotProjectile(arrawData, target, OnHit);
        }
        yield return new WaitForSeconds(1f);
        trail.SetActive(true);

        ReleaseMove();
        EndAttack();
    }

    public override void SpecialAttack() => StartCoroutine(Special_ArcherAttack());

    IEnumerator Special_ArcherAttack()
    {
        base.SpecialAttack();
        nav.angularSpeed = 1;
        trail.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            Transform[] targetArray =
                Multi_EnemyManager.Instance.GetProximateEnemys(transform.position, chaseRange, skillAttackTargetCount, target, rpcable.UsingId);
            int length = targetArray == null ? 0 : targetArray.Length;
            for (int i = 0; i < length; i++)
                ProjectileShotDelegate.ShotProjectile(arrawData, targetArray[i], OnSkileHit);
        }

        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.angularSpeed = 1000;

        SkillCoolDown(skillCoolDownTime);
    }
}
