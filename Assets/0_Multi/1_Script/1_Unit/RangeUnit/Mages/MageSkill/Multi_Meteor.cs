using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Multi_Meteor : MonoBehaviourPun
{
    [SerializeField] Multi_Projectile multi_Projectile;
    [SerializeField] int speed;
    void Awake()
    {
        multi_Projectile = GetComponent<Multi_Projectile>();
        multi_Projectile.SetSpeed(speed);
    }

    public void OnChase(Multi_Enemy enemy) => StartCoroutine(Co_ShotMeteor(enemy));
    IEnumerator Co_ShotMeteor(Multi_Enemy enemy)
    {
        Vector3 chasePosition = enemy.transform.position + (enemy.dir.normalized * enemy.speed);
        yield return new WaitForSeconds(1f);
        ChasePosition(chasePosition);
    }

    void ChasePosition(Vector3 chasePosition)
    {
        Vector3 _chaseVelocity = ((chasePosition - this.transform.position).normalized) * speed;
        RPC_Utility.Instance.RPC_Velocity(photonView.ViewID, _chaseVelocity);
    }

    public void Shot(Multi_Enemy enemy, Vector3 enemyPos, Action<Multi_Enemy> hitAction)
    {
        Vector3 chasePos = enemyPos + ( (enemy != null) ? enemy.dir.normalized * enemy.speed : Vector3.zero);
        multi_Projectile.Shot((chasePos - transform.position).normalized, null);
        explosionObject.GetComponent<Multi_HitSkill>().SetHitActoin(hitAction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (other.tag == "World" && !isExplosion) photonView.RPC("MeteorExplosion", RpcTarget.All);
    }

    [SerializeField] GameObject explosionObject;
    [SerializeField] GameObject[] meteors;
    bool isExplosion = false; // 충돌 2번 감지되는 버그 방지용 변수
    [PunRPC]
    void MeteorExplosion() // 메테오 폭발
    {
        foreach (GameObject meteor in meteors) meteor.SetActive(false);

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        isExplosion = true;
        explosionObject.SetActive(true);
        StartCoroutine(Co_HideObject());
    }

    IEnumerator Co_HideObject()
    {
        yield return new WaitForSeconds(0.25f);

        isExplosion = false;
        explosionObject.SetActive(false);
        transform.position = Vector3.one * 500;
        gameObject.SetActive(false);
        foreach (GameObject meteor in meteors) meteor.SetActive(true);
    }
}
