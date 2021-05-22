using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // 상태 변수
    public int maxHp;
    public int currentHp;
    public bool isDead;
    public Slider hpSlider;

    protected Rigidbody parentRigidbody;
    protected List<MeshRenderer> meshList;
    [SerializeField]
    private Material mat;
    private void Start()
    {
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

    public void EnemyStern(int sternPercent, float sternTime)
    {
        if (this.gameObject.CompareTag("Tower") || isDead) return;

        int random = Random.Range(0, 100);
        if (random < sternPercent)
        {
            StartCoroutine(SternCoroutine(sternTime));
        }
    }
    [SerializeField]
    private GameObject sternEffect;
    IEnumerator SternCoroutine(float sternTime)
    {
        sternEffect.SetActive(true);
        parentRigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(sternTime);
        parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.speed;
        sternEffect.SetActive(false);
    }

    //public bool isSlow;
    public void EnemySlow(float slowPercent, float slowTIme)
    {
        if (this.gameObject.CompareTag("Tower") || isDead) return;
        // 만약 더 높은 슬로우가 공격을 받으면큰 슬로우 적용후 return
        if (nomalEnemy.maxSpeed - nomalEnemy.maxSpeed * (slowPercent / 100) < nomalEnemy.speed)
        {
            nomalEnemy.speed = nomalEnemy.maxSpeed - nomalEnemy.maxSpeed * (slowPercent / 100);
            parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.speed;
            ChangeColor(new Color32(50, 175, 222, 1));
        }
        if(slowTIme > 0) Invoke("ExitSlow", slowTIme); // slowTIme이 0보다 작으면 무한 슬로우를 의미
        //isSlow = true;
        //nomalEnemy.speed -= nomalEnemy.speed * (slowPercent / 100);
        //parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.speed;
    }

    public void ExitSlow()
    {
        ChangeColor(mat.color);
        nomalEnemy.speed = nomalEnemy.maxSpeed;
        parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.maxSpeed;
    }

    public void EnemyPoisonAttack(int poisonPercent, int poisonCount, float poisonDelay)
    {
        if (isDead) return;

        StartCoroutine(PoisonAttack(poisonPercent, poisonCount, poisonDelay));
    }

    IEnumerator PoisonAttack(int poisonPercent, int poisonCount, float poisonDelay)
    {
        if (!this.gameObject.CompareTag("Tower")) 
            ChangeColor(new Color32(141, 49, 231, 255));
        int poisonDamage = Mathf.RoundToInt(currentHp * poisonPercent / 100);
        for (int i = 0; i < poisonCount; i++)
        {
            yield return new WaitForSeconds(poisonDelay);
            if (poisonDamage <= 0) poisonDamage = 1; // 독 최소뎀
            //if (currentHp > 1) OnDamage(poisonDamage); // 독으로는 못죽임
            if (poisonDamage >= 500) poisonDamage = 2000; // 독 최대뎀
            OnDamage(poisonDamage);
        }
        if (!this.gameObject.CompareTag("Tower"))
            ChangeColor(mat.color);
    }

    void ChangeColor(Color32 colorColor)
    {
        foreach(MeshRenderer mesh in meshList)
        {
            mesh.material.color = colorColor;
        }
    }
}
