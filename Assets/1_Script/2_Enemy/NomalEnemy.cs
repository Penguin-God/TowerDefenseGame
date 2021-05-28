using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NomalEnemy : Enemy
{
    EnemySpawn enemySpawn;
    // 능력치 변수
    public float maxSpeed;
    public float speed;

    // 이동, 회전 관련 변수
    private Transform parent;
    private Transform wayPoint;
    public Vector3 dir;
    private int pointIndex = -1;

    private void Awake()
    {
        nomalEnemy = GetComponent<NomalEnemy>();
        enemySpawn = GetComponentInParent<EnemySpawn>();
        parent = transform.parent.GetComponent<Transform>();
        parentRigidbody = GetComponentInParent<Rigidbody>();
    }

    void OnEnable() // 리스폰 시 상태 초기화
    {
        isDead = false;
        SetNextPoint();
        Invoke("AddListMe", 0.05f);
    }

    void AddListMe()
    {
        enemySpawn.currentEnemyList.Add(this.gameObject);
    }

    public void ResetStatus(int hp, float speed)
    {
        maxHp = hp;
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        this.maxSpeed = speed;
        this.speed = maxSpeed;
    }

    void SetNextPoint()
    {
        pointIndex++;
        if (pointIndex >= TurnPoint.enemyTurnPoints.Length) pointIndex = 0; // 무한반복을 위한 조건

        // 실제 이동을 위한 속도 설정
        wayPoint = TurnPoint.enemyTurnPoints[pointIndex];
        dir = (wayPoint.position - parent.transform.position).normalized;
        parentRigidbody.velocity = dir * speed;
    }

    void SetTransfrom()
    {
        transform.rotation = Quaternion.Euler(0, -90 * pointIndex, 0);
        parent.transform.position = wayPoint.position;
    }

    public override void Dead()
    {
        parent.gameObject.SetActive(false);
        parent.position = new Vector3(500, 500, 500);
        enemySpawn.currentEnemyList.Remove(this.gameObject);
        ResetVariable();

        GameManager.instance.Gold += 1;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold); // 돈벌기 추가
    }

    void ResetVariable()
    {
        pointIndex = -1;
        maxHp = 0;
        currentHp = 0;
        hpSlider.maxValue = 0;
        hpSlider.value = 0;
        isDead = true;
        this.speed = 0;
        transform.rotation = Quaternion.identity;
        ChangeColor(mat.color);
        sternEffect.SetActive(false);
    }

    //private TeamSoldier teamSoldier;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "WayPoint")
        {
            SetTransfrom();
            SetNextPoint();
        }
    }
}
