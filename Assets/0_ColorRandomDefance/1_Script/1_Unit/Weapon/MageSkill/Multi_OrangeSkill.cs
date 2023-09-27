using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_OrangeSkill : MonoBehaviour
{
    private void Awake()
    {
        _renderer = gameObject.GetOrAddComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        ps = GetComponent<ParticleSystem>();
    }

    public void OnSkile(Multi_Enemy enemy, int damage, int count, float percent)
        => StartCoroutine(Co_OrangeSkile(count, enemy, damage, percent));

    ParticleSystem ps = null;
    IEnumerator Co_OrangeSkile(int count, Multi_Enemy enemy, int damage, float percent)
    {
        for (int i = 0; i < count; i++)
        {
            if (enemy == null || enemy.IsDead) yield break;
            OrangeMageSkill(enemy, damage, percent);
            yield return new WaitForSeconds(0.4f);
        }

        Managers.Resources.Destroy(gameObject);
    }

    void OrangeMageSkill(Multi_Enemy enemy, int damage, float percent)
    {
        OnSkillEffect(enemy.transform.position);
        
        if (PhotonNetwork.IsMasterClient)
        {
            int _applyDamage = damage + Mathf.RoundToInt(enemy.currentHp / 100 * percent);
            enemy.OnDamage(_applyDamage, isSkill: true);
        }
    }

    Renderer _renderer;
    void OnSkillEffect(Vector3  targetPos)
    {
        transform.position = targetPos;
        ps.Play();
        OrangePlayAudio();
    }

    AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    void OrangePlayAudio()
    {
        if (_renderer.isVisible == false) return; // 문제
        // 찾은 클립이 0.6초부터 소리가 나와서 그때부터 재생함. 이거 때문에 얘는 사운드매니저 안 씀
        audioSource.time = 0.6f;
        audioSource.Play();
    }
}
