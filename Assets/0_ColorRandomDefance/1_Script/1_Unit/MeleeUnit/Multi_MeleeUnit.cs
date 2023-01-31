using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_MeleeUnit : Multi_TeamSoldier
{
    protected override ChaseSystem AddCahseSystem() => gameObject.AddComponent<MeeleChaser>();

    protected void HitMeeleAttack() // 근접공격 타겟팅
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        if (target != null && enemyDistance < AttackRange * 2)
            OnHit?.Invoke(TargetEnemy);
    }
}
