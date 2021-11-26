using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MageSkill
{
    Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public override void MageSkile(Unit_Mage mage)
    {
        transform.position = mage.transform.position + (Vector3.up * 30);
        StartCoroutine(ShotMeteor(mage));
    }

    IEnumerator ShotMeteor(TeamSoldier team)
    {
        Transform target = team.target;
        Enemy enemy = target.GetComponent<Enemy>();
        Vector3 chasePosition = target.position + (enemy.dir.normalized * enemy.speed);

        yield return new WaitForSeconds(1f);
        ChasePosition(chasePosition);
    }

    [SerializeField] float speed;
    public GameObject explosionObject;

    void ChasePosition(Vector3 chasePosition)
    {
        Vector3 enemyDirection = (chasePosition - this.transform.position).normalized;
        rigid.velocity = enemyDirection * speed;
    }

    public GameObject[] meteors;
    void MeteotExplosion() // 메테오 폭발
    {
        foreach (GameObject meteor in meteors) meteor.SetActive(false);

        explosionObject.SetActive(true);
        explosionObject.GetComponent<AudioSource>().Play();
        StartCoroutine(Co_HideObject());
    }

    IEnumerator Co_HideObject()
    {
        yield return new WaitForSeconds(0.25f);

        rigid.velocity = Vector3.zero ;
        explosionObject.SetActive(false);
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
        foreach (GameObject meteor in meteors) meteor.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "World") MeteotExplosion();
    }
}
