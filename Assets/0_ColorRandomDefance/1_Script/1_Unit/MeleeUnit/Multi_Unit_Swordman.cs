using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Swordman : Multi_TeamSoldier
{
    [Header("기사 변수")]
    [SerializeField] GameObject trail;

    UnitAttackControllerTemplate UnitAttackControllerTemplate; // 이거 사용해서 공격 부분 바꾸기

    protected override void OnAwake()
    {
        normalAttackSound = EffectSoundType.SwordmanAttack;
        _chaseSystem = gameObject.AddComponent<MeeleChaser>();
    }

    [PunRPC] protected override void Attack() => StartCoroutine(nameof(SwordAttack));

    IEnumerator SwordAttack()
    {
        base.StartAttack();
        animator.SetTrigger("isSword");
        animator.speed = 2;
        yield return new WaitForSeconds(0.8f / 2);
        trail.SetActive(true);

        yield return new WaitForSeconds(0.3f / 2);
        if (PhotonNetwork.IsMasterClient) // 거리 확인 필요
            UnitAttacker.NormalAttack(TargetEnemy);
        trail.SetActive(false);

        EndAttack();
    }
}
