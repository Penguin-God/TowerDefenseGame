using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Enemy : MonoBehaviour
{
    // 상태 변수
    public float maxSpeed = 0;
    public float speed = 0;
    public int maxHp = 0;
    public int currentHp = 0;
    public bool isDead = true;
    public Slider hpSlider = null;

    public Vector3 dir = Vector3.zero;

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


    public Action OnDeath = null;

    public void OnDamage(int damage)
    {
        currentHp -= damage;
        hpSlider.value = currentHp;

        if (currentHp <= 0 && !isDead) Dead();
    }

    public virtual void Dead() 
    {
        ResetValue();

        if (OnDeath != null) OnDeath();
    }

    void ResetValue()
    {
        queue_GetSturn.Clear();
        queue_HoldingPoison.Clear();

        maxSpeed = 0;
        speed = 0;
        isDead = true;
        maxHp = 0;
        currentHp = 0;
        hpSlider.maxValue = 0;
        hpSlider.value = 0;
    }

    protected NomalEnemy nomalEnemy;

    // 타워에서 안쓰는 함수들은 NomalEnemy로 옮기기
    public bool IsNoneSKile { get { return  GetComponent<EnemyTower>() != null || GetComponent<MageEnemy>() != null; } }

    void Set_OriginSpeed() // 나중에 이동 tralslate로 바꿔서 스턴이랑 이속 다르게 처리하는거 시도해보기
    {
        nomalEnemy.speed = nomalEnemy.maxSpeed;
        parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.maxSpeed;
    }

    public void EnemyStern(int sternPercent, float sternTime)
    {
        if (IsNoneSKile || isDead) return;

        int random = UnityEngine.Random.Range(0, 100);
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

        if(queue_GetSturn.Count != 0) queue_GetSturn.Dequeue();
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
    public void EnemySlow(float slowPercent, float slowTime)
    {
        if (IsNoneSKile || isDead) return;
        
        // 만약 더 높은 슬로우 공격을 받으면큰 슬로우 적용후 return
        if (nomalEnemy.maxSpeed - nomalEnemy.maxSpeed * (slowPercent / 100) <= nomalEnemy.speed)
        {
            // 더 강한 슬로우가 들어왔는데 이전 약한 슬로우 때문에 슬로우에서 빠져나가는거 방지
            if (isSlow && exitSlowCoroutine != null) StopCoroutine(exitSlowCoroutine); 

            isSlow = true;
            nomalEnemy.speed = nomalEnemy.maxSpeed - nomalEnemy.maxSpeed * (slowPercent / 100);
            parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.speed;

            ChangeColor(new Color32(50, 175, 222, 1));

            // 더 강한 슬로우 적용을 위한 코드
            // 더 강한 슬로우가 들어오면 작동 준비중이던 슬로우 탈출 코루틴은 나가리 되고 새로운 탈출 코루틴이 돌아감
            if (exitSlowCoroutine != null) StopCoroutine(exitSlowCoroutine);
            if (slowTime > 0)
                exitSlowCoroutine = StartCoroutine(ExitSlow_Coroutine(slowTime));
        }
    }

    // 얼리는 스킬
    public void EnemyFreeze(float slowTime)
    {
        isSlow = true;
        nomalEnemy.speed = 0;
        parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.speed;
        ChangeMat(slowSkillMat);
        
        if(exitSlowCoroutine != null) StopCoroutine(exitSlowCoroutine);
        exitSlowCoroutine = StartCoroutine(ExitSlow_Coroutine(slowTime));
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

        // 혹시 스턴중이 아니라면 속도 복구
        if (queue_GetSturn.Count <= 0) Set_OriginSpeed();
    }

    public void EnemyPoisonAttack(int poisonPercent, int poisonCount, float poisonDelay, int maxDamage)
    {
        if (GetComponent<MageEnemy>() != null || isDead) return;

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

        if(queue_HoldingPoison.Count != 0) queue_HoldingPoison.Dequeue();
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