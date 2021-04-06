using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TeamSoldier : MonoBehaviour
{
    public int damage;
    public NavMeshAgent nav;
    public Transform target;

    public EnemySpaw enemySpaw;
    public CombineSoldier Combine;

    private void Start()
    {
        nav = GetComponentInParent<NavMeshAgent>();
        enemySpaw = FindObjectOfType<EnemySpaw>();
        target = UpdateTarget();
    }

    private void Update()
    {
        if(target != null)
            nav.SetDestination(target.position);
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

    private void OnMouseDown()
    {
        Combine.ButtonOn();
        GameManager.instance.Chilk();
    }
}