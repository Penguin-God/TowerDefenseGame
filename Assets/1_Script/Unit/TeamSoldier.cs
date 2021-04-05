using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TeamSoldier : MonoBehaviour
{
    public Transform target;
    public SellDefenser sellDefenser;
    public int damage;
    public NavMeshAgent nav;

    //private void Awake()
    //{
    //    nav = GetComponent<NavMeshAgent>();
    //}

    private void Update()
    {
        nav.SetDestination(target.position);
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
    public GameObject trail;
    IEnumerator SwordAttack() 
    {
        animator.SetTrigger("isSword");
        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        swordCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        swordCollider.enabled = false;
        trail.SetActive(false);
    }
}