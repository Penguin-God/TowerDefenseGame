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

    [SerializeField] int _skillDamage;
    [SerializeField] int _useSkillPercent;

    protected override void OnAwake()
    {
        trail = GetComponentInChildren<TrailRenderer>().gameObject;
        arrawData = new ProjectileData(Multi_Managers.Data.WeaponDataByUnitFlag[UnitFlags].Paths[0],transform, arrawData.SpawnTransform);
        normalAttackSound = EffectSoundType.ArcherAttack;
        _useSkillPercent = 30;
    }

    public override void SetSkillDamage()
    {
        _skillDamage = (int)(Damage * 1.2f);
    }

    [PunRPC]
    protected override void Attack() => new UnitRandomSkillSystem().Attack(NormalAttack, SpecialAttack, _useSkillPercent);

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
        trail.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
            ShotSkill();

        yield return new WaitForSeconds(1f);
        trail.SetActive(true);

        SkillCoolDown(skillCoolDownTime);
    }

    readonly int SKILL_ARROW_COUNT = 3;
    void ShotSkill()
    {
        Transform[] targetArray =
            Multi_EnemyManager.Instance.GetProximateEnemys(transform.position, chaseRange, skillAttackTargetCount, target, rpcable.UsingId);
        if (targetArray == null || targetArray.Length == 0) return;

        for (int i = 0; i <= SKILL_ARROW_COUNT; i++)
        {
            int targetIndex = i % targetArray.Length;
            ProjectileShotDelegate.ShotProjectile(arrawData, targetArray[targetIndex], OnSkillHit);
        }
    }

    void OnSkillHit(Multi_Enemy enemy) => base.SkillAttackToEnemy(enemy, _skillDamage);
}
