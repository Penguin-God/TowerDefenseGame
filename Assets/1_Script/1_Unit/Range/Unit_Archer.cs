using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit_Archer : RangeUnit
{
    [Header("아처 변수")]
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
        //if (audioSource != null) audioSource.Play();
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
            instantArrow.GetComponent<SphereCollider>().radius = 5f; // 적이 잘 안맞아서 반지름 늘림
            ShotBullet(instantArrow, 3f, 50f, targetArray[i]);
        }
        //if (audioSource != null) audioSource.Play();

        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.angularSpeed = 1000;
        isAttack = false;
        base.NormalAttack();
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
            if(enemyList.Count != 0)
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
                targetArray[i] = targetObject.transform;
                enemyList.Remove(targetObject);
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
                enemy.EnemySlow(30, 2);
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
