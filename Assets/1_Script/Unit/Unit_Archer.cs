using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit_Archer : TeamSoldier
{
    private Enemy enemy;
    private GameObject trail;
    public GameObject arrow;
    public Transform arrowTransform;

    private void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>().gameObject;
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
        StartCoroutine(ArrowAttack());
    }

    IEnumerator ArrowAttack()
    {
        LookEnemy();
        yield return new WaitForSeconds(0.2f);
        nav.isStopped = true;
        trail.SetActive(false);

        if (target != null && Vector3.Distance(target.position, transform.position) < 150f)
        {
            GameObject instantArrow = CreateBullte(arrow, arrowTransform);
            ShotBullet(instantArrow, 2f, 50f);
        }

        yield return new WaitForSeconds(1.5f);
        nav.isStopped = false;
        trail.SetActive(true);
    }

    // 스킬 코드
    //NavMeshAgent arrowNav = instantArrow.GetComponent<NavMeshAgent>();
    //    while (instantArrow != null)
    //    {
    //        arrowNav.SetDestination(target.position);
    //        yield return new WaitForSeconds(0.08f);
    //    }
}
