using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Archer : TeamSoldier
{
    private Rigidbody archerRigidbody;
    public Rigidbody arrowRigidbody;

    private void Awake()
    {
        archerRigidbody = GetComponent<Rigidbody>();
    }


    public override void NormalAttack()
    {
        base.NormalAttack();

        arrowRigidbody.gameObject.transform.rotation = Quaternion.Euler(new Vector3(parent.transform.rotation.x, parent.transform.rotation.y, parent.transform.rotation.z));
        arrowRigidbody.velocity = Vector3.back * 10;
        archerRigidbody.AddForce(Vector3.back * 2, ForceMode.Impulse);
        Invoke("StopKnockback", 1);
    }

    void StopKnockback()
    {
        archerRigidbody.velocity = Vector3.zero;
        AttackEnd();
    }
}
