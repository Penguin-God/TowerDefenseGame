using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeSkill : MonoBehaviour
{
    AudioSource audioSource;
    MageSkill mageSkill;
    public TeamSoldier teamSoldier;
    Transform target;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mageSkill = GetComponent<MageSkill>();
        this.teamSoldier = mageSkill.teamSoldier;
        if(teamSoldier != null) target = teamSoldier.target;
    }

    [SerializeField] int count = 3;

    private void OnEnable()
    {
        StartCoroutine(Co_Wait());
        if (--count > 0)
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            Invoke("Seta", ps.startLifetime + 0.1f);
        }
    }

    IEnumerator Co_Wait()
    {
        yield return new WaitUntil(() => teamSoldier != null);
        OrangeMageSkill();
    }

    void Seta()
    {
        transform.position = target.position;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    void OrangeMageSkill()
    {
        Enemy enemy = null;
        if (target != null) enemy = target.GetComponent<Enemy>();
        if (enemy == null || enemy.isDead) return;
        OrangePlayAudio();
        int damage = (teamSoldier.bossDamage / 2) + Mathf.RoundToInt((enemy.currentHp / 100) * 5);
        enemy.OnDamage(damage);
    }

    [SerializeField] AudioClip audioClip;
    public void OrangePlayAudio()
    {
        // 찾은 클립이 처음부터 재생하면 딜레이가 좀 있어서 0.6초부터 재생함
        audioSource.time = 0.6f;
        audioSource.Play();
    }

    void ReSetAudioTime()
    {
        audioSource.volume = 0.15f;
        audioSource.time = 0f;
    }
}
