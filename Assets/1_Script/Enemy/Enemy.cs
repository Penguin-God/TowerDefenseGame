using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // 상태 변수
    public float speed;
    public int maxHp;
    public int currentHp;
    public bool isDead;
    public Slider hpSlider;

    // 이동, 회전 관련 변수
    private Transform parent;
    private Transform wayPoint;
    public Vector3 dir;
    private int pointIndex = -1;

    private void Awake()
    {
        parent = transform.parent.GetComponent<Transform>();
    }

    void OnEnable()
    {
        isDead = false;
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        SetNextPoint();
    }

    private void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        parent.Translate(dir * speed * Time.deltaTime, Space.World);
    }

    void SetNextPoint()
    {
        pointIndex++;
        if (pointIndex >= TurnPoint.enemyTurnPoints.Length) pointIndex = 0; // 무한반복을 위한 조건
        wayPoint = TurnPoint.enemyTurnPoints[pointIndex];
        dir = (wayPoint.position - parent.transform.position).normalized;
    }

    void SetTransfrom()
    {
        transform.rotation = Quaternion.Euler(0, -90 * pointIndex, 0);
        parent.transform.position = wayPoint.position;
    }

    void OnDamage(int damage)
    {
        currentHp -= damage;
        hpSlider.value = currentHp;
    }

    public void Dead() // 임시
    {
        parent.gameObject.SetActive(false);
        parent.position = new Vector3(500, 500, 500);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "WayPoint")
        {
            SetTransfrom();
            SetNextPoint();
        }
        else if(other.tag == "Attack") // 임시
        {
            AttackWeapon attackWeapon = other.GetComponentInParent<AttackWeapon>();
            TeamSoldier teamSoldier = attackWeapon.attackUnit.GetComponent<TeamSoldier>();

            if (teamSoldier.unitType == TeamSoldier.Type.rangeUnit) Destroy(other.gameObject); // 원거리 공격이면 총알 삭제

            OnDamage(attackWeapon.damage);
            if (currentHp <= 0)
            {
                Dead();
                teamSoldier.NextUpdateTarget();
            }
        }
    }
}
