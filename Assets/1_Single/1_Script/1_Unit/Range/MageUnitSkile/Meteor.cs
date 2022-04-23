using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Meteor : MonoBehaviour
{
    Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void OnChase(Enemy enemy)
    {
        StartCoroutine(Co_ShotMeteor(enemy));
    }

    IEnumerator Co_ShotMeteor(Enemy enemy)
    {
        Vector3 chasePosition = enemy.transform.position + (enemy.dir.normalized * enemy.speed);

        yield return new WaitForSeconds(1f);
        ChasePosition(chasePosition);
    }

    [SerializeField] float speed;
    void ChasePosition(Vector3 chasePosition)
    {
        Vector3 enemyDirection = (chasePosition - this.transform.position).normalized;
        rigid.velocity = enemyDirection * speed;
    }

    public GameObject explosionObject;
    public GameObject[] meteors;
    // 충돌 2번 감지되는 버그 방지
    bool isExplosion = false;
    void MeteorExplosion() // 메테오 폭발
    {
        foreach (GameObject meteor in meteors) meteor.SetActive(false);

        isExplosion = true;
        explosionObject.SetActive(true);
        SoundManager.instance.PlayEffectSound_ByName("MeteorExplosion", 0.12f);
        StartCoroutine(Co_HideObject());
    }

    IEnumerator Co_HideObject()
    {
        yield return new WaitForSeconds(0.25f);

        isExplosion = false;
        rigid.velocity = Vector3.zero ;
        explosionObject.SetActive(false);
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
        foreach (GameObject meteor in meteors) meteor.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "World" && !isExplosion) MeteorExplosion();
    }
}
