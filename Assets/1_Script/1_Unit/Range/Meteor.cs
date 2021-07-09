using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(ShotMeteor());
    }

    IEnumerator ShotMeteor()
    {
        yield return new WaitForSeconds(1f);
        ChaseTargetPosition(target);
    }

    [SerializeField] float speed;
    public GameObject explosionObject;
    public Transform target;
    public TeamSoldier teamSoldier;

    void ChaseTargetPosition(Transform tf_Enemy)
    {
        Enemy enemy = tf_Enemy.gameObject.GetComponent<Enemy>();
        Vector3 chasePosition = tf_Enemy.position + ( enemy.dir.normalized * enemy.speed);

        Vector3 enemyDirection = (chasePosition - this.transform.position).normalized;
        Rigidbody rigid = this.GetComponent<Rigidbody>();
        rigid.velocity = enemyDirection * speed;
    }

    public GameObject[] meteors;
    void MeteotExplosion() // 메테오 폭발
    {
        foreach (GameObject meteor in meteors) meteor.SetActive(false);
        explosionObject.GetComponent<MageSkill>().teamSoldier = this.teamSoldier;
        explosionObject.SetActive(true);
        explosionObject.GetComponent<AudioSource>().Play();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "World") MeteotExplosion();
    }
}
