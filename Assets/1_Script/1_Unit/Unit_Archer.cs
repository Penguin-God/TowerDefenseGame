using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit_Archer : RangeUnit
{
    private GameObject trail;
    public GameObject arrow;
    public Transform arrowTransform;

    private void Awake()
    {
        if(!enterStoryWorld) trail = GetComponentInChildren<TrailRenderer>().gameObject;
    }

    public override void SetPassive()
    {
        switch (unitColor)
        {
            case UnitColor.red:
                attackDelayTime *= 0.5f;
                break;
            case UnitColor.blue:
                break;
            case UnitColor.yellow:
                break;
            case UnitColor.green:
                speed *= 2;
                break;
            case UnitColor.orange:
                damage *= 2;
                break;
            case UnitColor.violet:
                break;
        }
    }

    public override void NormalAttack()
    {
        SpecialAttack();
        //StartCoroutine("ArrowAttack");
    }

    IEnumerator ArrowAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;

        nav.angularSpeed = 1;
        trail.SetActive(false);
        GameObject instantArrow = CreateBullte(arrow, arrowTransform);
        ShotBullet(instantArrow, 2f, 50f, target);

        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.angularSpeed = 1000;

        isAttack = false;
        base.NormalAttack();
    }

    public override void SpecialAttack()
    {
        StartCoroutine(Special_ArcherAttack());
    }

    IEnumerator Special_ArcherAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;
        nav.angularSpeed = 1;
        trail.SetActive(false);

        int enemyCount = 3;
        Transform[] targetArray = Set_AttackTarget(target, enemySpawn.currentEnemyList, enemyCount);
        for (int i = 0; i < targetArray.Length; i++)
        {
            GameObject instantArrow = CreateBullte(arrow, arrowTransform);
            ShotBullet(instantArrow, 4f, 50f, targetArray[i]);
        }

        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.angularSpeed = 1000;
        isAttack = false;
        base.NormalAttack();
    }

    Transform[] Set_AttackTarget(Transform targetTransform, List<GameObject> currentEnemyList, int count)
    {
        if (currentEnemyList.Count == 0) return null;

        List<GameObject> enemyList = new List<GameObject>();
        for(int i = 0; i < currentEnemyList.Count; i++)
        {
            enemyList.Add(currentEnemyList[i]);
        }
        Transform[] targetArray = new Transform[count];
        targetArray[0] = targetTransform;
        enemyList.Remove(targetTransform.gameObject);

        float shortDistance = 150f;
        GameObject targetObject = null;
        Debug.Log(targetTransform);
        for (int i = 1; i < count; i++)
        {
            if(enemyList.Count != 0)
            {
                foreach (GameObject enemyObject in enemyList)
                {
                    if (enemyObject != null)
                    {
                        float distanceToEnemy = Vector3.Distance(targetTransform.position, enemyObject.transform.position);
                        Debug.Log(distanceToEnemy);
                        if (distanceToEnemy < shortDistance)
                        {
                            shortDistance = distanceToEnemy;
                            targetObject = enemyObject; 
                        }
                    }
                }
                shortDistance = 150f;
                targetArray[i] = targetObject.transform;
                enemyList.Remove(targetObject);
            }
            else targetArray[i] = targetTransform;
        }
        return targetArray;
    }

    public override void RangeUnit_PassiveAttack(Enemy enemy)
    {
        //Enemy enemy = GetEnemyScript();
        switch (unitColor)
        {
            case UnitColor.red:
                break;
            case UnitColor.blue:
                enemy.EnemySlow(30);
                break;
            case UnitColor.yellow:
                Add_PassiveGold(1, 2);
                break;
            case UnitColor.green:
                break;
            case UnitColor.orange:
                break;
            case UnitColor.violet:
                enemy.EnemyStern(5, 2);
                break;
        }
    }
}


// 스킬 코드
//NavMeshAgent arrowNav = instantArrow.GetComponent<NavMeshAgent>();
//    while (instantArrow != null)
//    {
//        arrowNav.SetDestination(target.position);
//        yield return new WaitForSeconds(0.08f);
//    }
