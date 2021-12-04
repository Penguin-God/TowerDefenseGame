using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMage : Unit_Mage
{
    public override void MageSkile()
    {
        base.MageSkile();
        SetSkilObject(transform.position + (Vector3.up * 2));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<NomalEnemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.EnemySlow(bluePassiveFigure.y, -1f); // 나가기 전까진 무한 슬로우
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<NomalEnemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.ExitSlow();
        }
    }
}
