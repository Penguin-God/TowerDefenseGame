using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TeamSoldier : MonoBehaviour
{
    public int damage;
    public NavMeshAgent nav;
    public Transform target;

    public CombineSoldier Combine;

    private void Start()
    {
        nav = GetComponentInParent<NavMeshAgent>();
    }

    private void Update()
    {
        if(target != null)
            nav.SetDestination(target.position);
    }

    private void OnMouseDown()
    {
        Combine.ButtonOn();
        GameManager.instance.Chilk();
    }
}