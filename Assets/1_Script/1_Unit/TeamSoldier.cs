using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TeamSoldier : MonoBehaviour
{
    // 원거리 유닛은 target이 움직이는 방향에 가중치를 준 값 추적하기

    
    public enum Type { sowrdman, archer, spearman, mage } // rangeUnit = 원거리 공격 유닛,  meleeUnit = 근거리 공격 유닛
    public Type unitType;

    public float speed;
    public float attackDelayTime;
    public float attackRange;
    public int damage;

    public float stopDistanc;
    public bool isAttack; // 공격 쿨타임 중에 true인 함수

    protected int layerMask; // Ray 감지용

    protected NavMeshAgent nav;
    public Transform target;

    private EnemySpawn enemySpawn;
    private CombineSoldier Combine;

    private float chaseRange; // 풀링할 때 멀리 풀에 있는 놈들 충돌 안하게 하기위한 추적 최소거리
    private void Start()
    {
        chaseRange = 150f;
        Combine = FindObjectOfType<CombineSoldier>();
        nav = GetComponentInParent<NavMeshAgent>();
        enemySpawn = FindObjectOfType<EnemySpawn>();
        UpdateTarget();
        nav.speed = this.speed;
        layerMask = 1 << LayerMask.NameToLayer("Enemy"); // Ray가 Enemy 레이어만 충돌 체크함
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

    
    public virtual bool CanAttack() // 자식들이 Attack가능 여부 판단
    {
        return false;
    }

    protected float enemyDistance;
    IEnumerator NavCoroutine() // 적을 추적하는 무한반복 코루틴
    {
        while (true)
        {
            //if (target != null && Vector3.Distance(target.position, this.transform.position) < chaseRange) // target이 추적범위 안에 있으면
            //{
            //    float dir = Vector3.Distance(target.position, this.transform.position);

            //    // 유닛 타입에 따른 움직임
            //    if ((unitType == Type.spearman || unitType == Type.sowrdman) && dir < 15f) MeleeMove();
            //    else RangeChaseMove(dir);

            //    if (dir < attackRange && !isAttack) // 적이 사정거리 안에 있을때
            //    {
            //        NormalAttack();
            //    }
            //    nav.SetDestination(target.position);
            //}

            if(target != null) enemyDistance = Vector3.Distance(this.transform.position, target.position);
            if (target == null || enemyDistance > chaseRange)
            {
                UpdateTarget();
                yield return null; // 튕김 방지
                continue;
            }

            nav.SetDestination(target.position + new Vector3(12f, 0, 12f));
            if (CanAttack() && !isAttack) // Attack가능하고 쿨타임이 아니면 공격
                NormalAttack();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void UpdateTarget() // 가장 가까운 거리에 있는 적으로 타겟을 바꿈
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

    protected Enemy GetEnemyScript()
    {
        Enemy enemy = null;
        if (target != null) enemy = target.gameObject.GetComponentInChildren<Enemy>();
        return enemy;
    }

    //private void MeleeMove() // 근접 공격 시 상대방이 유닛 쪽으로 움직이고 있으면 정지 아니면 이동
    //{
    //    Enemy enemy = GetEnemyScript();
    //    float enemyDot = Vector3.Dot(enemy.dir.normalized, (target.position - this.transform.position).normalized);
    //    if (enemyDot < -0.7f) nav.speed = 1.5f;
    //    else nav.speed = this.speed;
    //}

    private void OnMouseDown()
    {
        GameManager.instance.Chilk();
        Combine.ButtonOn();
    }
}