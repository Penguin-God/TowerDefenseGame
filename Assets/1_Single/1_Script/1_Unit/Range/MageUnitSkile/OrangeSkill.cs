using System.Collections;
using UnityEngine;
using System;

public class OrangeSkill : MonoBehaviour
{
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ps = GetComponent<ParticleSystem>();
    }

    public void OnSkile(Enemy enemy, bool isUltimate, int damage)
    {
        int count = isUltimate ? 5 : 3;
        StartCoroutine(Co_OrangeSkile(count, enemy, damage));
    }

    ParticleSystem ps = null;
    IEnumerator Co_OrangeSkile(int count, Enemy enemy, int damage)
    {
        for(int i = 0; i < count; i++)
        {
            OrangeMageSkill(enemy, damage);
            yield return new WaitForSeconds(ps.startLifetime + 0.1f);
        }

        gameObject.SetActive(false);
    }

    void OrangeMageSkill(Enemy enemy, int damage)
    {
        if (!enemy.isDead) transform.position = enemy.transform.position;
        OrangePlayAudio();

        if (enemy != null && !enemy.isDead)
        {
            ps.Play();
            int onDamage = (damage / 2) + Mathf.RoundToInt((enemy.currentHp / 100) * 5);
            enemy.OnDamage(onDamage);
        }
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
