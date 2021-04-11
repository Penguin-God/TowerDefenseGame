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
        speed = 6f;
        attackDelayTime = 3f;
        attackRange = 30f;
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

        GameObject instantArrow = Instantiate(arrow, arrowTransform.position, arrowTransform.rotation);
        AttackWeapon attackWeapon = instantArrow.GetComponent<AttackWeapon>();
        attackWeapon.attackUnit = this.gameObject; // 화살과 적의 충돌감지를 위한 대입

        ShotBullet(instantArrow, 2f, 50f);

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
