using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TeamSoldier : MonoBehaviour
{
    public SellDefenser sellDefenser;
    public int damage;

    private void Awake()
    {
        StartCoroutine(SwordAttack());
    }

    public Rigidbody arrowRigidbody;
    void ArrowAttack()
    {
        arrowRigidbody.velocity = Vector3.back * 10;
    }

    private void OnMouseDown()
    {
        UIManager.instance.SetActiveButton(true);
        GameManager.instance.Chilk();

    }
    public Animator animator;
    public BoxCollider swordCollider;
    IEnumerator SwordAttack() 
    {
        animator.SetTrigger("isSword");
        yield return new WaitForSeconds(0.15f);
        swordCollider.enabled = true;
        yield return new WaitForSeconds(0.15f);
        swordCollider.enabled = false;
    }
}