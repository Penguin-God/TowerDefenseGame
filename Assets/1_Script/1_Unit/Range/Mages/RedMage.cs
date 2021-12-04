using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMage : Unit_Mage
{
    public override void MageSkile()
    {
        base.MageSkile();
        SetSkilObject(transform.position + (Vector3.up * 30));
        mageSkill.OnSkile(target.GetComponent<Enemy>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9) Change_Unit_AttackDelayTime(other.gameObject, 0.5f);
    }
    
    private void OnTriggerExit(Collider other)
    { 
        if (other.gameObject.layer == 9) Change_Unit_AttackDelayTime(other.gameObject, 2f);
    }

    void Change_Unit_AttackDelayTime(GameObject unitObject, float rate)
    {
        unitObject.GetComponent<TeamSoldier>().attackDelayTime *= rate;
    }
}
