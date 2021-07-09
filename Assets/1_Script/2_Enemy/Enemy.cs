using System.Collections;
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
        meshList = new List<MeshRenderer>();

        meshList.Add(GetComponent<MeshRenderer>());
        MeshRenderer[] addMeshs = GetComponentsInChildren<MeshRenderer>();
        
        for(int i = 0; i < addMeshs.Length; i++)
        {
            meshList.Add(addMeshs[i]);
        }
    }

    // 대미지 관련 함수
    public void OnDamage(int damage)
    {
        currentHp -= damage;
        hpSlider.value = currentHp;
        if (currentHp <= 0)
        {
            Dead();
        }
    }

    public virtual void Dead()
    {

    }

    protected NomalEnemy nomalEnemy;

    Coroutine sternCoroutine = null;
    public void EnemyStern(int sternPercent, float sternTime)
    {
        if (this.gameObject.CompareTag("Tower") || isDead) return;

        int random = Random.Range(0, 100);
        if (random < sternPercent)
        {
            if (sternCoroutine != null) StopCoroutine(sternCoroutine);
            sternCoroutine = StartCoroutine(SternCoroutine(sternTime));
        }
    }
    protected bool isSturn;
    public GameObject sternEffect;
    IEnumerator SternCoroutine(float sternTime)
    {
        isSturn = true;
        sternEffect.SetActive(true);
        nomalEnemy.speed = 0;
        parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.speed;
        yield return new WaitForSeconds(sternTime);
        nomalEnemy.speed = nomalEnemy.maxSpeed;
        parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.maxSpeed;
        sternEffect.SetActive(false);
        isSturn = false;
        sternCoroutine = null;
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
        if (!isSturn)
        {
            nomalEnemy.speed = nomalEnemy.maxSpeed;
            parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.maxSpeed;
        }
        isSlow = false;
    }

    public void EnemyPoisonAttack(int poisonPercent, int poisonCount, float poisonDelay, int maxDamage)
    {
        if (isDead) return; 

        StartCoroutine(PoisonAttack(poisonPercent, poisonCount, poisonDelay, maxDamage));
    }

    IEnumerator PoisonAttack(int poisonPercent, int poisonCount, float poisonDelay, int maxDamage)
    {
        if (!this.gameObject.CompareTag("Tower")) 
            ChangeColor(new Color32(141, 49, 231, 255));

        int poisonDamage = Mathf.RoundToInt(currentHp * poisonPercent / 100); 
        for (int i = 0; i < poisonCount; i++)
        {
            yield return new WaitForSeconds(poisonDelay);
            if (poisonDamage <= 0) poisonDamage = 1; // 독 최소뎀
            if (poisonDamage >= maxDamage) poisonDamage = maxDamage; // 독 최대뎀
            OnDamage(poisonDamage);
        }

        if (!this.gameObject.CompareTag("Tower"))
            ChangeColor(new Color32(255, 255, 255, 255));
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
