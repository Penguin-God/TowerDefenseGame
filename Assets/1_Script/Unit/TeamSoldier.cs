using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TeamSoldier : MonoBehaviour
{
    public int damage;
    public float attackRange;
    public bool isAttack;

    private NavMeshAgent nav;
    public Transform target;
    public Transform parent;

    private EnemySpaw enemySpaw;
    public CombineSoldier Combine;

    private void Start()
    {
        parent = GetComponentInParent<Transform>();
        nav = GetComponentInParent<NavMeshAgent>();
        enemySpaw = FindObjectOfType<EnemySpaw>();
    }

    private void Update()
    {
        if(target != null)
        {
            if(Vector3.Distance(target.position, this.transform.position) < attackRange && !isAttack)
            {
                NormalAttack();
            }
            nav.SetDestination(target.position);
        }
        else
            target = UpdateTarget();
    }

    Transform UpdateTarget()
    {
        float shortDistance = 1000f;
        GameObject targetObject = null;

        foreach(GameObject enemy in enemySpaw.currentEnemyList)
        {
            float distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);
            if(distanceToEnemy < shortDistance)
            {
                shortDistance = distanceToEnemy;
                targetObject = enemy;
            }
        }

        if (targetObject != null) return targetObject.transform;
        else return null;
    }

    public virtual void NormalAttack()
    {
        nav.isStopped = true;
        isAttack = true;
    }

    protected void AttackEnd()
    {
        nav.isStopped = false;
        isAttack = false;
    }

    private void OnMouseDown()
    {
        Combine.ButtonOn();
        GameManager.instance.Chilk();
    }
}