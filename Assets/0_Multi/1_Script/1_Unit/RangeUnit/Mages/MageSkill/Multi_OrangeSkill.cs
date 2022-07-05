using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_OrangeSkill : MonoBehaviourPun
{
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ps = GetComponent<ParticleSystem>();
    }

    public void OnSkile(Multi_Enemy enemy, bool isUltimate, int damage)
    {
        if (!photonView.IsMine) return;

        int count = isUltimate ? 5 : 3;
        StartCoroutine(Co_OrangeSkile(count, enemy, damage));
    }

    ParticleSystem ps = null;
    IEnumerator Co_OrangeSkile(int count, Multi_Enemy enemy, int damage)
    {
        for (int i = 0; i < count; i++)
        {
            OrangeMageSkill(enemy, damage);
            yield return new WaitForSeconds(ps.startLifetime + 0.1f); // startLifetime
        }

        RPC_Utility.Instance.RPC_Active(photonView.ViewID, false);
        //myPunRPC.RPC_Active(false);
    }

    void OrangeMageSkill(Multi_Enemy enemy, int damage)
    {
        if (enemy == null || enemy.isDead) return;

        photonView.RPC("OnSkillEffect", RpcTarget.All, enemy.transform.position);

        int _applyDamage = (damage / 2) + Mathf.RoundToInt((enemy.currentHp / 100) * 5);
        enemy.photonView.RPC("OnDamage", RpcTarget.MasterClient, _applyDamage);
    }

    [PunRPC]
    void OnSkillEffect(Vector3  _pos)
    {
        transform.position = _pos;
        ps.Play();
        OrangePlayAudio();
    }

    AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    public void OrangePlayAudio()
    {
        // 찾은 클립이 초반에 소리가 비는 부분이 있어서 0.6초부터 재생함
        audioSource.time = 0.6f;
        audioSource.Play();
    }
}
