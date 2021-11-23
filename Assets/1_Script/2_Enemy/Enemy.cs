﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // 상태 변수
    public float maxSpeed;

    public float speed;
    public int maxHp;
    public int currentHp;
    public bool isDead = true;
    public Slider hpSlider;

    public Vector3 dir;

    protected Rigidbody parentRigidbody;
    protected List<MeshRenderer> meshList;
    [SerializeField]
    protected Material originMat;
    private void Start()
    {
        originMat = GetComponent<MeshRenderer>().material;

        meshList = new List<MeshRenderer> { GetComponent<MeshRenderer>() };
        MeshRenderer[] addMeshs = GetComponentsInChildren<MeshRenderer>();
        for(int i = 0; i < addMeshs.Length; i++) meshList.Add(addMeshs[i]);
    }

    // 대미지 관련 함수
    public void OnDamage(int damage)
    {
        currentHp -= damage;
        hpSlider.value = currentHp;

        if (currentHp <= 0) Dead();
    }

    public virtual void Dead() 
    {
        queue_GetSturn.Clear();
        queue_HoldingPoison.Clear();
    }

    protected NomalEnemy nomalEnemy;


    void Set_OriginSpeed() // 나중에 이동 tralslate로 바꿔서 스턴이랑 이속 다르게 처리하는거 시도해보기
    {
        nomalEnemy.speed = nomalEnemy.maxSpeed;
        parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.maxSpeed;
    }

    Coroutine sternCoroutine = null;

    public void EnemyStern(int sternPercent, float sternTime)
    {
        if (this.gameObject.CompareTag("Tower") || isDead) return;

        int random = Random.Range(0, 100);
        if (random < sternPercent) StartCoroutine(SternCoroutine(sternTime));
    }

    Queue<int> queue_GetSturn = new Queue<int>();
    public GameObject sternEffect;
    //public int debugCoung = 0;
    //public int queueCount = 0;
    IEnumerator SternCoroutine(float sternTime)
    {
        queue_GetSturn.Enqueue(-1);
        sternEffect.SetActive(true);
        nomalEnemy.speed = 0;
        parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.speed;
        yield return new WaitForSeconds(sternTime);

        queue_GetSturn.Dequeue();
        if (queue_GetSturn.Count == 0) ExitSturn();
    }
    void ExitSturn()
    {
        sternEffect.SetActive(false);
        //sternCoroutine = null;
        Set_OriginSpeed();
    }


    protected bool isSlow;
    Coroutine exitSlowCoroutine = null;
    [SerializeField]
    private Material slowSkillMat;
    public void EnemySlow(float slowPercent, float slowTIme, bool isSkill = false)
    {
        if (this.gameObject.CompareTag("Tower") || isDead) return;
        
        // 만약 더 높은 슬로우 공격을 받으면큰 슬로우 적용후 return
        if (nomalEnemy.maxSpeed - nomalEnemy.maxSpeed * (slowPercent / 100) <= nomalEnemy.speed)
        {
            // 더 강한 슬로우가 들어왔는데 이전 약한 슬로우 때문에 슬로우에서 빠져나가는거 방지
            if (isSlow && exitSlowCoroutine != null) StopCoroutine(exitSlowCoroutine); 

            isSlow = true;
            nomalEnemy.speed = nomalEnemy.maxSpeed - nomalEnemy.maxSpeed * (slowPercent / 100);
            parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.speed;

            if (!isSkill) ChangeColor(new Color32(50, 175, 222, 1));
            else ChangeMat(slowSkillMat);

            if (slowTIme > 0)
            {
                exitSlowCoroutine = StartCoroutine(ExitSlow_Coroutine(slowTIme)); // 더 강한 슬로우 적용 시 코루틴 중지를 위한 코드
            }
        }
    }

    IEnumerator ExitSlow_Coroutine(float slowTime)
    {
        yield return new WaitForSeconds(slowTime);
        ExitSlow();
    }

    public void ExitSlow()
    {
        ChangeMat(originMat);
        ChangeColor(new Color32(255, 255, 255, 255));
        isSlow = false;

        if (queue_GetSturn.Count <= 0) Set_OriginSpeed();
    }

    public void EnemyPoisonAttack(int poisonPercent, int poisonCount, float poisonDelay, int maxDamage)
    {
        if (isDead) return; 

        StartCoroutine(PoisonAttack(poisonPercent, poisonCount, poisonDelay, maxDamage));
    }

    // Queue를 사용해서 현재 코루틴이 중복으로 돌아가고 있지 않으면 색깔 복귀하기
    Queue<int> queue_HoldingPoison = new Queue<int>();
    IEnumerator PoisonAttack(int poisonPercent, int poisonCount, float poisonDelay, int maxDamage)
    {
        queue_HoldingPoison.Enqueue(-1);
        ChangeColor(new Color32(141, 49, 231, 255));
        int poisonDamage = GetPoisonDamage(poisonPercent, maxDamage);
        for (int i = 0; i < poisonCount; i++)
        {
            yield return new WaitForSeconds(poisonDelay);
            OnDamage(poisonDamage);
        }

        queue_HoldingPoison.Dequeue();
        if (queue_HoldingPoison.Count == 0) ChangeColor(new Color32(255, 255, 255, 255));
    }

    int GetPoisonDamage(int poisonPercent, int maxDamage)
    {
        int poisonDamage = Mathf.RoundToInt(currentHp * poisonPercent / 100);
        if (poisonDamage <= 0) poisonDamage = 1; // 독 최소뎀
        if (poisonDamage >= maxDamage) poisonDamage = maxDamage; // 독 최대뎀
        return poisonDamage;
    }

    protected void ChangeColor(Color32 colorColor)
    {
        foreach(MeshRenderer mesh in meshList)
        {
            mesh.material.color = colorColor;
        }
    }

    protected void ChangeMat(Material mat)
    {
        foreach (MeshRenderer mesh in meshList)
        {
            mesh.material = mat;
        }
    }
}
