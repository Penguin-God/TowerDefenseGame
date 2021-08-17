using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeSkill : MonoBehaviour
{
    AudioSource audioSource;
    MageSkill mageSkill;

    public TeamSoldier teamSoldier;
    Enemy enemy;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mageSkill = GetComponent<MageSkill>();
        this.teamSoldier = mageSkill.teamSoldier;
        if(teamSoldier != null) enemy = teamSoldier.target.GetComponent<Enemy>();
        count = teamSoldier.gameObject.GetComponent<Unit_Mage>().isUltimate ? 5 : 3;
    }

    int count = -1;

    private void OnEnable()
    {
        StartCoroutine(Co_Wait());
    }

    IEnumerator Co_Wait()
    {
        yield return new WaitUntil(() => count != -1);
        if (enemy == null) yield break; // 코루틴 끝내기
        OrangeMageSkill();

        if (--count > 0)
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            Invoke("SetOnEnalble", ps.startLifetime + 0.1f);
        }
    }

    void SetOnEnalble()
    {
        if(!enemy.isDead) transform.position = enemy.transform.position;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    void OrangeMageSkill()
    {
        OrangePlayAudio();

        if (enemy != null && !enemy.isDead)
        {
            int damage = (teamSoldier.bossDamage / 2) + Mathf.RoundToInt((enemy.currentHp / 100) * 5);
            enemy.OnDamage(damage);
        }
    }

    [SerializeField] AudioClip audioClip;
    public void OrangePlayAudio()
    {
        // 찾은 클립이 초반에 소리가 비는 부분이 있어서 0.6초부터 재생함
        audioSource.time = 0.6f;
        audioSource.Play();
    }
}
