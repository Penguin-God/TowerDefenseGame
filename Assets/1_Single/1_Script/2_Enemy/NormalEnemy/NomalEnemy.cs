using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NomalEnemy : Enemy
{
    protected EnemySpawn enemySpawn;

    // 이동, 회전 관련 변수
    protected Transform parent;
    private Transform wayPoint;
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
        pointIndex = 0;
        ChaseToPoint();
        isDead = false;

        Passive();
    }

    public void SetStatus(int hp, float speed)
    {
        maxHp = hp;
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        this.maxSpeed = speed;
        this.speed = maxSpeed;
    }

    void ChaseToPoint()
    {
        if (pointIndex >= TurnPoint.enemyTurnPoints.Length) pointIndex = 0; // 무한반복을 위한 조건

        // 실제 이동을 위한 속도 설정
        wayPoint = TurnPoint.enemyTurnPoints[pointIndex];
        dir = (wayPoint.position - parent.transform.position).normalized;
        parentRigidbody.velocity = dir * speed;
    }

    void SetTransfrom()
    {
        parent.transform.position = wayPoint.position;
        transform.rotation = Quaternion.Euler(0, -90 * pointIndex, 0);
    }

    [SerializeField] int enemyNumber = 0;
    public int GetEnemyNumber { get { return enemyNumber;  } }

    public override void Dead()
    {
        base.Dead();

        parent.gameObject.SetActive(false);
        parent.position = new Vector3(500, 500, 500);
        enemySpawn.currentEnemyList.Remove(this.gameObject);
        ResetVariable();
    }

    void ResetVariable()
    {
        pointIndex = -1;
        transform.rotation = Quaternion.identity;
        ChangeMat(originMat);
        ChangeColor(new Color32(255, 255, 255, 255));
        sternEffect.SetActive(false);
    }

    public virtual void Passive() {}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "WayPoint")
        {
            SetTransfrom();

            pointIndex++;
            ChaseToPoint();
        }
    }
}
