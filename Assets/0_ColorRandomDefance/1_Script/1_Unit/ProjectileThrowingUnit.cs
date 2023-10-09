using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorwPathCalculator
{
    public Vector3 CalculateThorwPath_To_Monster(Multi_Enemy target, Transform attacker)
    {
        if (target == null) return attacker.forward.normalized;
        else if (target.enemyType == EnemyType.Tower) return (target.transform.position - attacker.position).normalized;
        else return new ThorwPathCalculator().CalculatePath_To_MoveTarget(attacker.position, target.transform.position, target.GetComponent<Multi_NormalEnemy>().Speed, target.dir);
    }

    readonly float WEIGHT_RATE = 2f; // 움직이는 타겟 보간 가중치
    Vector3 CalculatePath_To_MoveTarget(Vector3 shoterPos, Vector3 targetPos, float speed, Vector3 moveDir)
    {
        Vector3 dir = targetPos - shoterPos;
        float enemyWeightDir = Mathf.Lerp(0, WEIGHT_RATE, Vector3.Distance(targetPos, shoterPos) * 2 / 100);
        dir += moveDir.normalized * (0.5f * speed) * enemyWeightDir;
        return dir.normalized;
    }
}
