using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Archer : TeamSoldier
{
    private Enemy enemy;
    private GameObject trail;
    public GameObject arrow;
    public Transform arrowTransform;

    private void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>().gameObject;
        attackDelayTime = 2.5f;
        attackRange = 40f;
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
        StartCoroutine(ArrowAttack());
    }

    IEnumerator ArrowAttack()
    {
        nav.isStopped = true;
        trail.SetActive(false);

        GameObject instantArrow = Instantiate(arrow, arrowTransform.position, arrowTransform.rotation);
        AttackWeapon attackWeapon = instantArrow.GetComponent<AttackWeapon>();
        attackWeapon.attackUnit = this.gameObject; // 충돌감지를 위한 대입

        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
        Vector3 dir = target.position - instantArrow.transform.position;
        enemy = target.gameObject.GetComponentInChildren<Enemy>();
        float enemyWeightDir = Mathf.Lerp(0, enemy.speed, (2 * Vector3.Distance(target.position, this.transform.position)) / 100);
        dir += enemy.dir * enemyWeightDir;
        arrowRigid.velocity = dir.normalized * 50;

        yield return new WaitForSeconds(1.5f);
        nav.isStopped = false;
        trail.SetActive(true);
    }
}
