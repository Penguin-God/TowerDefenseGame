using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Swordman : Multi_TeamSoldier
{
    UnitAttackControllerTemplate _unitAttackControllerTemplate;
    protected override void OnAwake()
    {
        _chaseSystem = gameObject.AddComponent<MeeleChaser>();
        _unitAttackControllerTemplate = new UnitAttackControllerGenerator().GenerateSwordmanAttacker(this);
    }

    [PunRPC] protected override void Attack() => _unitAttackControllerTemplate.DoAttack(AttackDelayTime);
}
