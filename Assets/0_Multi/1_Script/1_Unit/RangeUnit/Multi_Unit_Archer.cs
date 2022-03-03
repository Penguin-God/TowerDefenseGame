using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Archer : Multi_RangeUnit
{
    [Header("아처 변수")]
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowTransform;
    private GameObject trail;

    public override void OnAwake()
    {
        trail = GetComponentInChildren<TrailRenderer>().gameObject;
        SetPoolObj(arrow, 15);
    }

    public override void SetInherenceData()
    {
        skillDamage = (int)(damage * 1.2f);
    }

    public override void NormalAttack()
    {
        StartCoroutine("ArrowAttack");
    }

    IEnumerator ArrowAttack()
    {
        base.StartAttack();

        nav.isStopped = true;
        trail.SetActive(false);
        if (target != null && enemyDistance < chaseRange && pv.IsMine)
        {
            poolManager.UsedWeapon(arrowTransform, Get_ShootDirection(2f, target), 50, (Multi_Enemy enemy) => delegate_OnHit(enemy));
        }
        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.isStopped = false;

        EndAttack();
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

    IEnumerator Special_ArcherAttack()
    {
        base.StartAttack();
        isAttackDelayTime = true;
        nav.angularSpeed = 1;
        trail.SetActive(false);

        int enemyCount = 3;
        Transform[] targetArray = Set_AttackTarget(target, Multi_EnemySpawner.instance.currentNormalEnemyList, enemyCount);
        for (int i = 0; i < targetArray.Length; i++)
        {
            poolManager.UsedWeapon(arrowTransform, Get_ShootDirection(2f, targetArray[i]), 50, (Multi_Enemy enemy) => delegate_OnSkile(enemy));
        }

        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.angularSpeed = 1000;

        EndAttack();
    }

    // 현재 타겟을 포함해서 가장 가까운 적 count만큼의 Transform[]를 return하는 함수
    Transform[] Set_AttackTarget(Transform p_Target, List<GameObject> currentEnemyList, int count)
    {
        if (currentEnemyList.Count == 0) return null;

        List<GameObject> _EnemyList = new List<GameObject>(); // 새로운 리스트 생성
        for (int i = 0; i < currentEnemyList.Count; i++) _EnemyList.Add(currentEnemyList[i]);

        Transform[] targetArray = new Transform[count];
        targetArray[0] = p_Target;
        _EnemyList.Remove(p_Target.gameObject);

        for (int i = 1; i < count; i++) // 위에서 array에 targetTransform을 넣었으니 i가 1부터 시작
        {
            if (_EnemyList.Count != 0 && TargetIsNormalEnemy)
            {
                GameObject _obj = GetProximateEnemy_AtList(_EnemyList);
                if (_obj != null)
                {
                    targetArray[i] = _obj.transform;
                    _EnemyList.Remove(_obj);
                }
            }
            else targetArray[i] = p_Target; // 적이 부족하거나 보스이면 1명한테 올인
        }

        return targetArray;
    }
}
