﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit_Archer : RangeUnit, IEvent
{
    [Header("아처 변수")]
    public GameObject arrow;
    public Transform arrowTransform;
    private GameObject trail;

    private void Awake()
    {
        if(!enterStoryWorld) trail = GetComponentInChildren<TrailRenderer>().gameObject;
    }

    public override void SetPassive()
    {
        switch (unitColor)
        {
            case UnitColor.red:
                attackDelayTime *= redPassiveFigure;
                break;
            case UnitColor.blue:
                break;
            case UnitColor.yellow:
                break;
            case UnitColor.green:
                specialAttackPercent *= greenPassiveFigure;
                break;
            case UnitColor.orange:
                damage *= orangePassiveFigure;
                break;
            case UnitColor.violet:
                break;
        }
    }

    public override void NormalAttack()
    {
        StartCoroutine("ArrowAttack");
    }

    IEnumerator ArrowAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;

        nav.angularSpeed = 1;
        trail.SetActive(false);
        GameObject instantArrow = CreateBullte(arrow, arrowTransform);
        ShotBullet(instantArrow, 1.5f, 50f, target);
        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.angularSpeed = 1000;

        isAttack = false;
        base.NormalAttack();
        if (enemySpawn.currentEnemyList.Count != 0 && !target.gameObject.CompareTag("Tower") && !target.gameObject.CompareTag("Boss")) UpdateTarget();
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
            instantArrow.GetComponent<SphereCollider>().radius = 5f; // 적이 잘 안맞아서 반지름 늘림
            ShotBullet(instantArrow, 3f, 50f, targetArray[i]);
        }
        if (enterStoryWorld == GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(normalAttackClip);

        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.angularSpeed = 1000;
        isAttack = false;
        base.NormalAttack();
        if (enemySpawn.currentEnemyList.Count != 0 && !target.gameObject.CompareTag("Tower") && !target.gameObject.CompareTag("Boss")) UpdateTarget();
    }

    // 첫번째에 targetTransform을 넣고 currentEnemyList에서 targetTransform을 가장 가까운 transform을 count 크기만큼 가지는 array를 return하는 함수
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

        for (int i = 1; i < count; i++) // 위에서 array에 targetTransform을 넣었으니 i가 1부타 시작
        {
            if(enemyList.Count != 0 && !target.gameObject.CompareTag("Tower") &&  !target.gameObject.CompareTag("Boss"))
            {
                foreach (GameObject enemyObject in enemyList)
                {
                    if (enemyObject != null)
                    {
                        float distanceToEnemy = Vector3.Distance(targetTransform.position, enemyObject.transform.position);
                        if (distanceToEnemy < shortDistance)
                        {
                            shortDistance = distanceToEnemy;
                            targetObject = enemyObject; 
                        }
                    }
                }
                shortDistance = 150f;
                if (targetObject != null)
                {
                    targetArray[i] = targetObject.transform;
                    enemyList.Remove(targetObject);
                }
            }
            else targetArray[i] = targetTransform;
        }
        return targetArray;
    }

    public override void RangeUnit_PassiveAttack(Enemy enemy)
    {
        switch (unitColor)
        {
            case UnitColor.red:
                break;
            case UnitColor.blue:
                enemy.EnemySlow(bluePassiveFigure, 2);
                break;
            case UnitColor.yellow:
                Add_PassiveGold(yellowPassiveFigure, 2);
                break;
            case UnitColor.green:
                break;
            case UnitColor.orange:
                break;
            case UnitColor.violet:
                enemy.EnemyStern(violetPassiveFigure, 2);
                break;
        }
    }


    // 이벤트
    public void SkillPercentUp()
    {
        specialAttackPercent += 20;
    }

    public void SkillPercentDown()
    {
        specialAttackPercent -= 20;
    }

    // 패시브 이벤트
    private float redPassiveFigure = 0.25f;
    private int bluePassiveFigure = 30;
    private int yellowPassiveFigure = 1;
    private int greenPassiveFigure = 2;
    private int orangePassiveFigure = 2;
    private int violetPassiveFigure = 5;
    public void ReinforcePassive()
    {
        redPassiveFigure = 0.1f;
        bluePassiveFigure = 60;
        yellowPassiveFigure = 10;
        greenPassiveFigure = 3;
        orangePassiveFigure = 3;
        violetPassiveFigure = 20;
    }

    public void WeakenPassive()
    {
        redPassiveFigure = 1f;
        bluePassiveFigure = 0;
        yellowPassiveFigure = 0;
        greenPassiveFigure = 1;
        orangePassiveFigure = 1;
        violetPassiveFigure = 0;
    }
}


// 스킬 코드
//NavMeshAgent arrowNav = instantArrow.GetComponent<NavMeshAgent>();
//    while (instantArrow != null)
//    {
//        arrowNav.SetDestination(target.position);
//        yield return new WaitForSeconds(0.08f);
//    }
