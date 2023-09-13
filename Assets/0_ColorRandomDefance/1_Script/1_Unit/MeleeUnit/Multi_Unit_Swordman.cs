using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Swordman : Multi_TeamSoldier
{
    [Header("기사 변수")]
    [SerializeField] GameObject trail;
    
    protected override void OnAwake()
    {
        normalAttackSound = EffectSoundType.SwordmanAttack;
        _chaseSystem = gameObject.AddComponent<MeeleChaser>();
    }

    [PunRPC]
    protected override void Attack() => NormalAttack();

    public override void NormalAttack() => StartCoroutine(nameof(SwordAttack));

    IEnumerator SwordAttack()
    {
        base.StartAttack();
        animator.SetTrigger("isSword");
        yield return new WaitForSeconds(0.8f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        if (photonView.IsMine)
            ArcherNormalAttack(TargetEnemy);
        trail.SetActive(false);

        EndAttack();
    }

    void ArcherNormalAttack(Multi_Enemy target) => Attack(target, false);
    void Attack(Multi_Enemy target, bool isSkill)
    {
        if (target == null) return;
        target.OnDamage(CalculateAttack(), isSkill);
        new UnitPassiveCreator(Managers.Data).CreatePassive(UnitFlags)?.DoUnitPassive(this, target);
    }
}
