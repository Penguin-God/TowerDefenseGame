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
        _unitAttackControllerTemplate = new UnitAttackControllerGenerator().GenerateSwordmanAttacker(this);
    }

    [PunRPC] protected override void Attack() => _unitAttackControllerTemplate.DoAttack(1, AttackDelayTime); // StartCoroutine(nameof(SwordAttack));
}
