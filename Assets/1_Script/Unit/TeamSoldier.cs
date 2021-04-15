﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TeamSoldier : MonoBehaviour
{
    Transform parentTransform;
    //public string unitColor;
    public enum Type { rangeUnit, meleeUnit } // rangeUnit = 원거리 공격 유닛,  meleeUnit = 근거리 공격 유닛
    public Type unitType;
    public float speed;
    public float attackDelayTime;
    public float attackRange;
    public bool isAttack;

    public NavMeshAgent nav;
    public Transform target;

    private EnemySpawn enemySpawn;
    public CombineSoldier Combine;

    private void Start()
    {
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

    IEnumerator NavCoroutine()
    {
        while (true)
        {
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.position) < 150f)
                {
                    float dir = Vector3.Distance(target.position, this.transform.position);
                    if (unitType == Type.meleeUnit && dir < 15f) MeleeMove();
                    if (dir < attackRange)
                    {
                        if (unitType == Type.rangeUnit) nav.speed = 0.1f;

                        if (!isAttack)
                        {
                            NormalAttack();
                        }
                    }
                    else if(unitType == Type.rangeUnit)
                    {
                        nav.speed = speed;
                    } 
                    nav.SetDestination(target.position);
                }
                else
                {
                    UpdateTarget();
                }
            }
            else UpdateTarget();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void UpdateTarget()
    {
        float shortDistance = 150f;
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

    public void NextUpdateTarget()
    {
        bool hasRemoveEnemy = enemySpawn.currentEnemyList.Contains(target.gameObject);
        if (hasRemoveEnemy) enemySpawn.currentEnemyList.Remove(target.gameObject);
        UpdateTarget();
    }

    protected void LookEnemy()
    {
        parentTransform.LookAt(target.position);
        transform.Rotate(Vector3.up * 180);
    }

    protected GameObject CreateBullte(GameObject instantObject, Transform createPositon)
    {
        GameObject instantBullet = Instantiate(instantObject, createPositon.position, createPositon.rotation);
        AttackWeapon attackWeapon = instantBullet.GetComponent<AttackWeapon>();
        attackWeapon.attackUnit = this.gameObject; // 화살과 적의 충돌감지를 위한 대입
        return instantBullet;
    }

    protected void ShotBullet(GameObject bullet, float weightRate, float velocity) // 원거리 유닛 총알 발사
    {
        Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
        Vector3 dir = target.position - bullet.transform.position;
        Enemy enemy = target.gameObject.GetComponentInChildren<Enemy>();
        float enemyWeightDir = Mathf.Lerp(0, enemy.speed, (weightRate * Vector3.Distance(target.position, this.transform.position)) / 100);
        dir += enemy.dir * enemyWeightDir;
        bulletRigid.velocity = dir.normalized * velocity;
    }

    protected void MeleeMove() // 근접 공격 시 상대방이 유닛 쪽으로 움직이고 있으면 정지 아니면 이동
    {
        Enemy enemy = target.gameObject.GetComponentInChildren<Enemy>();
        float enemyDot = Vector3.Dot(enemy.dir.normalized, (target.position - this.transform.position).normalized);
        if (enemyDot < -0.7f) nav.speed = 0.1f;
        else nav.speed = this.speed;
        Debug.Log(enemyDot);
    }

    private void OnMouseDown()
    {
        Combine.ButtonOn();
        GameManager.instance.Chilk();
    }
}