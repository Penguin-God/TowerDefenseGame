using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_OrangeSkill : MonoBehaviour
{
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ps = GetComponent<ParticleSystem>();
    }

    WorldAudioPlayer _worldAudioPlayer;
    public void OnSkile(Multi_Enemy enemy, int damage, int count, float percent, WorldAudioPlayer worldAudioPlayer)
    {
        _worldAudioPlayer = worldAudioPlayer;
        StartCoroutine(Co_OrangeSkile(count, enemy, damage, percent));
    }

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
        OnSkillEffect(enemy.transform.position, enemy.MonsterSpot);
        
        if (PhotonNetwork.IsMasterClient)
        {
            int _applyDamage = damage + MathUtil.CalculatePercentage(enemy.currentHp, percent);
            enemy.OnDamage(_applyDamage, isSkill: true);
        }
    }

    void OnSkillEffect(Vector3 targetPos, ObjectSpot objectSpot)
    {
        transform.position = targetPos;
        ps.Play();
        OrangePlayAudio(objectSpot);
    }

    AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    void OrangePlayAudio(ObjectSpot objectSpot)
    {
        // 찾은 클립이 0.6초부터 소리가 나와서 그때부터 재생함.
        audioSource.time = 0.6f;
        _worldAudioPlayer.PlayObjectEffectSound(objectSpot, audioSource);
    }
}
