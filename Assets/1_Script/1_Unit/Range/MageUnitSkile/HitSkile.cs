using System.Collections;
using UnityEngine;
using System;

public class HitSkile : MonoBehaviour
{
    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public event Action<Enemy> OnHitSkile;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            if (OnHitSkile != null) 
                OnHitSkile(other.GetComponent<Enemy>());
        }
    }

    [SerializeField] protected SphereCollider sphereCollider;
    [SerializeField] private float hitTime; // 콜라이더가 켜지기 전 공격 대기 시간

    private void OnEnable()
    {
        StartCoroutine(Co_OnCollider(hitTime));
    }
    IEnumerator Co_OnCollider(float delayTIme)
    {
        yield return new WaitForSeconds(delayTIme);
        sphereCollider.enabled = true;
    }
    private void OnDisable()
    {
        sphereCollider.enabled = false;
    }
}