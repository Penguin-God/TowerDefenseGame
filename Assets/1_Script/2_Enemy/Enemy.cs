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
        if (this.gameObject.CompareTag("Tower")) return;

        int random = Random.Range(0, 100);
        if (random < sternPercent)
        {
            StartCoroutine(SternCoroutine(sternTime));
        }
    }

    IEnumerator SternCoroutine(float sternTime)
    {
        parentRigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(sternTime);
        parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.speed;
    }

    //public bool isSlow;
    public void EnemySlow(float slowPercent)
    {
        if (this.gameObject.CompareTag("Tower")) return;
        // 만약 더 높은 슬로우가 공격을 받으면큰 슬로우 적용후 return
        if (nomalEnemy.maxSpeed - nomalEnemy.maxSpeed * (slowPercent / 100) < nomalEnemy.speed)
        {
            nomalEnemy.speed = nomalEnemy.maxSpeed - nomalEnemy.maxSpeed * (slowPercent / 100);
            parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.speed;
            ChangeColor(new Color32(50, 175, 222, 1));
        }
        //isSlow = true;
        //nomalEnemy.speed -= nomalEnemy.speed * (slowPercent / 100);
        //parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.speed;
    }

    public void ExitSlow()
    {
        parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.maxSpeed;
    }

    public IEnumerator PoisonAttack(int poisonPercent, int poisonCount, float poisonDelay)
    {
        ChangeColor(new Color32(141, 49, 231, 255));
        int poisonDamage = Mathf.RoundToInt(currentHp * poisonPercent / 100);
        for (int i = 0; i < poisonCount; i++)
        {
            if (poisonDamage <= 0) poisonDamage = 1; // 독 최소뎀
            if (currentHp > 1) OnDamage(poisonDamage); // 독으로는 못죽임
            yield return new WaitForSeconds(poisonDelay);
        }
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
