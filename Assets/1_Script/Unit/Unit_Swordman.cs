using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Swordman : TeamSoldier
{
    public Animator animator;
    public BoxCollider swordCollider;
    public GameObject trail;

    private void Awake()
    {
        
    }

    IEnumerator SwordAttack()
    {
        animator.SetTrigger("isSword");
        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        swordCollider.enabled = true;
        yield return new WaitForSeconds(0.4f);
        swordCollider.enabled = false;
        trail.SetActive(false);
    }
}
