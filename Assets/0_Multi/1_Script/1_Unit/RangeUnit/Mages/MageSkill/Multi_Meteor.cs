using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Meteor : MonoBehaviourPun
{
    Rigidbody rigid;
    private void Awake() => rigid = GetComponent<Rigidbody>();

    public void OnChase(Multi_Enemy enemy) => StartCoroutine(Co_ShotMeteor(enemy));
    IEnumerator Co_ShotMeteor(Multi_Enemy enemy)
    {
        yield return new WaitForSeconds(1f);
        Vector3 chasePosition = enemy.transform.position;
        ChasePosition(chasePosition);
    }

    [SerializeField] float speed;
    void ChasePosition(Vector3 chasePosition)
    {
        Vector3 _chaseVelocity = ((chasePosition - this.transform.position).normalized) * speed;
        GetComponent<MyPunRPC>().RPC_Velocity(_chaseVelocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (other.tag == "World" && !isExplosion) MeteorExplosion();
    }

    [SerializeField] GameObject explosionObject;
    [SerializeField] GameObject[] meteors;
    bool isExplosion = false; // 충돌 2번 감지되는 버그 방지용 변수
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
        rigid.velocity = Vector3.zero;
        explosionObject.SetActive(false);
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
        foreach (GameObject meteor in meteors) meteor.SetActive(true);
    }
}
