﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeSkill : MageSkill
{
    TeamSoldier teamSoldier;
    private void Awake()
    {
        teamSoldier = GetComponentInParent<TeamSoldier>();
        audioSource = GetComponent<AudioSource>();
        ps = GetComponent<ParticleSystem>();
    }

    public override void MageSkile(Unit_Mage mage)
    {
        count = mage.isUltimate ? 3 : 5;
        OrangeSkile(mage.target.GetComponent<Enemy>());
    }

    int count = -1;

    ParticleSystem ps = null;

    void OrangeSkile(Enemy enemy)
    {
        if (enemy == null) return;

        OrangeMageSkill(enemy);
        // 조건 검사하기 전에 이미 한번 실행해서 -- 써도 됨
        if (--count > 0) StartCoroutine(Co_Move(ps.startLifetime + 0.1f));
    }

    IEnumerator Co_Move(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // 껏다 킬때마다 조건을 확인하는 걸로 반복문 구현
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    void OrangeMageSkill(Enemy enemy)
    {
        if (!enemy.isDead) transform.position = enemy.transform.position;
        OrangePlayAudio();

        if (enemy != null && !enemy.isDead)
        {
            int damage = (teamSoldier.bossDamage / 2) + Mathf.RoundToInt((enemy.currentHp / 100) * 5);
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
