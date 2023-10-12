using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Swordman : Multi_TeamSoldier
{
    [Header("기사 변수")]
    [SerializeField] GameObject trail;

    UnitAttackControllerTemplate _unitAttackControllerTemplate;

    protected override void OnAwake()
    {
        normalAttackSound = EffectSoundType.SwordmanAttack;
        _chaseSystem = gameObject.AddComponent<MeeleChaser>();
        _unitAttackControllerTemplate = gameObject.GetComponent<SwordmanAttackController>();
        _unitAttackControllerTemplate.DependencyInject(animator, "isSword", _state, Unit);
        _unitAttackControllerTemplate.GetComponent<SwordmanAttackController>().RecevieInject(this);
    }

    [PunRPC] protected override void Attack() => _unitAttackControllerTemplate.DoAttack(1, AttackDelayTime); // StartCoroutine(nameof(SwordAttack));

    void SwordAttack()
    {
        base.StartAttack();
        //animator.SetTrigger("isSword");
        //animator.speed = 2;
        //yield return new WaitForSeconds(0.8f / 2);
        //trail.SetActive(true);

        //yield return new WaitForSeconds(0.3f / 2);
        //if (PhotonNetwork.IsMasterClient) // 거리 확인 필요
        //    UnitAttacker.NormalAttack(TargetEnemy);
        //trail.SetActive(false);
        _unitAttackControllerTemplate.DoAttack(1, AttackDelayTime);
        EndAttack();
    }
}
