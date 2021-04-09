using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Archer : TeamSoldier
{
    private Transform parent;
    private GameObject trail;
    public GameObject arrow;
    public Transform arrowTransform;

    private void Awake()
    {
        parent = GetComponentInParent<Transform>();
        trail = GetComponentInChildren<TrailRenderer>().gameObject;
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
        StartCoroutine(ArrowAttack());
    }

    IEnumerator ArrowAttack()
    {
        GameObject instantArrow = Instantiate(arrow, arrowTransform.position, arrowTransform.rotation);
        AttackWeapon attackWeapon = instantArrow.GetComponent<AttackWeapon>();
        attackWeapon.attackUnit = this.gameObject;

        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
        Vector3 dir = target.position - instantArrow.transform.position;
        arrowRigid.velocity = dir.normalized * 50;
        trail.SetActive(false);
        yield return new WaitForSeconds(2f);
        trail.SetActive(true);
        AttackEnd();
    }
}
