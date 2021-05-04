using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleUnit : TeamSoldier
{
    bool rayHit;
    RaycastHit rayHitObject;
    bool enemyIsForward;
    //int layerMask;

    //private void Start()
    //{
    //    layerMask = 1 << LayerMask.NameToLayer("Enemy"); // Enemy 레이어만 충돌 체크함
    //}

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position + Vector3.up, -1 * transform.forward * attackRange, Color.green);
        rayHit = Physics.Raycast(transform.parent.position + Vector3.up, 
            -1 * transform.forward, out rayHitObject, attackRange, layerMask);
        if (rayHit)
        {
            Debug.Log(rayHitObject.transform.gameObject);
            Debug.Log(target.GetChild(0).gameObject);
            if (rayHitObject.transform.gameObject == target.parent.gameObject) enemyIsForward = true;
            else enemyIsForward = false;
        }
        else enemyIsForward = false;
        //enemyIsForward = Physics.Raycast(transform.parent.position + Vector3.up, -1 * transform.forward, out hit, attackRange, LayerMask.GetMask("Enemy"));
        Stop_or_Move();
    }

    void Stop_or_Move()
    {
        if (enemyIsForward && enemyDistance < stopDistanc)
        {
            nav.isStopped = true;
            //Debug.Log("isStop");
        }
        else nav.isStopped = false;
    }

    public override bool CanAttack()
    {
        if (enemyIsForward && enemyDistance < attackRange) return true;
        else return false;
    }
}
