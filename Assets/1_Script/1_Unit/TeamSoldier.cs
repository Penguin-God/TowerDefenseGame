﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TeamSoldier : MonoBehaviour
{
    // 원거리 유닛은 target이 움직이는 방향에 가중치를 준 값 추적하기

    Transform parentTransform;
    public enum Type { sowrdman, archer, spearman, mage } // rangeUnit = 원거리 공격 유닛,  meleeUnit = 근거리 공격 유닛
    public Type unitType;

    public float speed;
    public float attackDelayTime;
    public float attackRange;
    public bool isAttack;
    public float chaseRange;
    public int damage;

    public NavMeshAgent nav;
    public Transform target;

    private EnemySpawn enemySpawn;
    public CombineSoldier Combine;

    private void Start()
    {
        chaseRange = 150f;
        Combine = FindObjectOfType<CombineSoldier>();
        parentTransform = GetComponentInParent<Transform>();
        nav = GetComponentInParent<NavMeshAgent>();
        enemySpawn = FindObjectOfType<EnemySpawn>();
        UpdateTarget();
        nav.speed = this.speed;
        StartCoroutine(NavCoroutine());
    }

    public virtual void NormalAttack()
    {
        isAttack = true;
        Invoke("ReadyAttack", attackDelayTime);
    }
    void ReadyAttack()
    {
        isAttack = false;
    }

    IEnumerator NavCoroutine() // 적을 추적하는 무한반복 코루틴
    {
        while (true)
        {
            if (target != null && Vector3.Distance(target.position, this.transform.position) < chaseRange) // target이 추적범위 안에 있으면
            {
                float dir = Vector3.Distance(target.position, this.transform.position);

                // 유닛 타입에 따른 움직임
                if ((unitType == Type.spearman || unitType == Type.sowrdman) && dir < 15f) MeleeMove();
                else RangeChaseMove(dir);

                if (dir < attackRange && !isAttack) // 적이 사정거리 안에 있을때
                {
                    NormalAttack();
                }
                nav.SetDestination(target.position);
            }
            else UpdateTarget();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void UpdateTarget() // 가장 짧은 거리에 있는 적으로 타겟을 바꿈
    {
        float shortDistance = chaseRange;
        GameObject targetObject = null;
        if (enemySpawn.currentEnemyList.Count > 0)
        {
            foreach (GameObject enemyObject in enemySpawn.currentEnemyList)
            {
                if (enemyObject != null)
                { 
                    float distanceToEnemy = Vector3.Distance(this.transform.position, enemyObject.transform.position);
                    if (distanceToEnemy < shortDistance)
                    {
                        shortDistance = distanceToEnemy;
                        targetObject = enemyObject;
                    }
                }
            }
        }
        // 위에서 업데이트된 targetObject의 정보를 가지고 nav 및 변수 설정
        SetChaseSetting(targetObject); 
    }

    void SetChaseSetting(GameObject targetObject) // 추적 관련 변수 설정
    {
        if (targetObject != null)
        {
            nav.isStopped = false;
            target = targetObject.transform;
        }
        else
        {
            nav.isStopped = true;
            target = null;
        }
    }

    void RangeChaseMove(float distance)
    {
        if (distance < attackRange) nav.speed = 0.1f;
        else nav.speed = this.speed;
    }

    protected void LookEnemy()
    {
        parentTransform.LookAt(target.position);
        transform.Rotate(Vector3.up * 180);
    }

    protected Enemy GetEnemyScript()
    {
        Enemy enemy = null;
        if (target != null) enemy = target.gameObject.GetComponentInChildren<Enemy>();
        return enemy;
    }

    protected GameObject CreateBullte(GameObject instantObject, Transform createPositon)
    {
        Vector3 instantPosition = new Vector3(createPositon.position.x, 2f, createPositon.position.z);
        GameObject instantBullet = Instantiate(instantObject, instantPosition, Quaternion.identity);
        AttackWeapon attackWeapon = instantBullet.GetComponent<AttackWeapon>();
        attackWeapon.attackUnit = this.gameObject; // 화살과 적의 충돌감지를 위한 대입
        return instantBullet;
    }

    protected void ShotBullet(GameObject bullet, float weightRate, float velocity) // 원거리 유닛 총알 발사
    {
        Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
        Enemy enemy = GetEnemyScript();

        Vector3 dir = target.position - bullet.transform.position;
        float enemyWeightDir = Mathf.Lerp(0, enemy.speed, (weightRate * Vector3.Distance(target.position, this.transform.position)) / 100);
        dir += enemy.dir * enemyWeightDir;
        bulletRigid.velocity = dir.normalized * velocity;
    }

    private void MeleeMove() // 근접 공격 시 상대방이 유닛 쪽으로 움직이고 있으면 정지 아니면 이동
    {
        Enemy enemy = GetEnemyScript();
        float enemyDot = Vector3.Dot(enemy.dir.normalized, (target.position - this.transform.position).normalized);
        if (enemyDot < -0.7f) nav.speed = 1.5f;
        else nav.speed = this.speed;
    }

    protected float Check_EnemyToUnit_Deg()
    {
        Enemy enemy = GetEnemyScript();
        float enemyDot = Vector3.Dot(enemy.dir.normalized, (target.position - this.transform.position).normalized);
        return enemyDot;
    }

    protected void HitMeeleAttack() // 근접공격 타겟팅
    {
        Enemy enemy = GetEnemyScript();
        if (enemy != null && Vector3.Distance(enemy.transform.position, this.transform.position) < attackRange) 
            enemy.OnDamage(this.damage);
    }

    bool enemyIsForward = true;
    private void FixedUpdate()
    {
        //Debug.DrawRay(transform.position + Vector3.up * 2, transform.forward * -attackRange, Color.green);
        //enemyIsForward = Physics.Raycast(transform.position + Vector3.up * 2, transform.forward, -attackRange, LayerMask.GetMask("Enemy"));
        if(CanAttack()) Debug.Log(CanAttack());
        CanAttack();
    }

    float enemyDistance;
    bool CanAttack()
    {
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * -3, Color.green);
        Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit hit, -3);
        if(target != null) enemyDistance = Vector3.Distance(this.transform.position, target.position);

        if (hit.transform != null) Debug.Log("aa");
        if (hit.transform != null && hit.transform.gameObject.layer == 8 && enemyDistance < attackRange) return true;
        else return false;
    }

    private void OnMouseDown()
    {
        GameManager.instance.Chilk();
        Combine.ButtonOn();
    }
}