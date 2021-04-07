using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Archer : TeamSoldier
{
    private GameObject trail;
    public GameObject arrow;
    public Transform arrowTransform;
    private Rigidbody archerRigidbody;

    private void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>().gameObject;
        archerRigidbody = GetComponent<Rigidbody>();
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
        StartCoroutine(ArrowAttack());
    }

    IEnumerator ArrowAttack()
    {
        GameObject instantArrow = Instantiate(arrow, arrowTransform.position, arrowTransform.rotation);
        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
        Vector3 dir = target.position - instantArrow.transform.position;
        arrowRigid.velocity = (targetEnemy.dir.normalized*targetEnemy.speed + dir).normalized * 30;
        trail.SetActive(false);
        //archerRigidbody.AddForce(Vector3.back * 2, ForceMode.Impulse);
        yield return new WaitForSeconds(2f);
        trail.SetActive(true);
        AttackEnd();
    }
}
