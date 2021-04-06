using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Archer : TeamSoldier
{
    private Rigidbody archerRigidbody;
    private TrailRenderer trail;
    public Rigidbody arrowRigidbody;

    private void Awake()
    {
        archerRigidbody = GetComponent<Rigidbody>();
        trail = GetComponentInChildren<TrailRenderer>();
    }

    void ArrowAttack()
    {
        trail.gameObject.SetActive(true);
        arrowRigidbody.velocity = Vector3.forward * 10;
        archerRigidbody.AddForce(Vector3.back * 2, ForceMode.Impulse);
        Invoke("StopKnockback", 1);
    }

    void StopKnockback()
    {
        archerRigidbody.velocity = Vector3.zero;
    }
}
