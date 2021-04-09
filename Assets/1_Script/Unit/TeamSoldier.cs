using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TeamSoldier : MonoBehaviour
{
    public enum Type { rangeUnit, meleeUnit } // rangeUnit = 원거리 공격 유닛,  meleeUnit = 근거리 공격 유닛
    public Type unitType;
    public float speed;
    public float attackDelayTime;
    public float attackRange;
    public bool isAttack;

    public NavMeshAgent nav;
    public Transform target;

    private EnemySpaw enemySpaw;
    public CombineSoldier Combine;

    private void Start()
    {
        nav = GetComponentInParent<NavMeshAgent>();
        enemySpaw = FindObjectOfType<EnemySpaw>();
        UpdateTarget();
        StartCoroutine(NavCoroutine());
    }

    IEnumerator NavCoroutine()
    {
        while (true)
        {
            if (target != null)
            {
                float dir = Vector3.Distance(target.position, this.transform.position);
                if (dir < attackRange)
                {
                    if (unitType == Type.rangeUnit) nav.speed = 0.1f;
                    if (!isAttack) NormalAttack();
                }
                else nav.speed = speed;
                nav.SetDestination(target.position);
            }
            else
            {
                UpdateTarget();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void UpdateTarget()
    {
        float shortDistance = 1000f;
        GameObject targetObject = null;
        if (enemySpaw.currentEnemyList.Count > 0)
        {
            foreach (GameObject enemyObject in enemySpaw.currentEnemyList)
            {
                if (enemyObject != null)
                { 
                    float distanceToEnemy = Vector3.Distance(this.transform.position, enemyObject.transform.position);
                    if (distanceToEnemy < shortDistance)
                    {
                        shortDistance = distanceToEnemy;
                        targetObject = enemyObject;
                    }
                }
            }
        }
        if (targetObject != null)
        {
            nav.isStopped = false;
            target = targetObject.transform;
        }
        else nav.isStopped = true;
    }

    public void NextUpdateTarget()
    {
        bool hasRemoveEnemy = enemySpaw.currentEnemyList.Contains(target.gameObject);
        if (hasRemoveEnemy) enemySpaw.currentEnemyList.Remove(target.gameObject);
        UpdateTarget();
    }

    public virtual void NormalAttack()
    {
        isAttack = true;
        Invoke("ReadyAttack", attackDelayTime);
    }

    void ReadyAttack()
    {
        isAttack = false;
    }

    private void OnMouseDown()
    {
        Combine.ButtonOn();
        GameManager.instance.Chilk();
    }
}