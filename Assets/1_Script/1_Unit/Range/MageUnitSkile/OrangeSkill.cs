using System.Collections;
using UnityEngine;
using System;

public class OrangeSkill : MageSkill
{
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ps = GetComponent<ParticleSystem>();
    }

    public TeamSoldier team = null;
    public override void OnSkile(Enemy enemy)
    {
        gameObject.SetActive(true);
        int count = team.GetComponent<Unit_Mage>().isUltimate ? 5 : 3;
        StartCoroutine(Co_OrangeSkile(count, enemy));
    }

    ParticleSystem ps = null;
    IEnumerator Co_OrangeSkile(int count, Enemy enemy)
    {
        for(int i = 0; i < count; i++)
        {
            OrangeMageSkill(enemy);
            yield return new WaitForSeconds(ps.startLifetime + 0.1f);
        }

        gameObject.SetActive(false);
    }

    void OrangeMageSkill(Enemy enemy)
    {
        if (!enemy.isDead) transform.position = enemy.transform.position;
        OrangePlayAudio();

        if (enemy != null && !enemy.isDead)
        {
            ps.Play();
            int damage = (team.bossDamage / 2) + Mathf.RoundToInt((enemy.currentHp / 100) * 5);
            enemy.OnDamage(damage);
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
