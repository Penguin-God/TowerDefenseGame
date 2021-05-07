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
        StartCoroutine("NavCoroutine");
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

    //public virtual void EenmyChase() // 추적
    //{
        
    //}

    protected float enemyDistance;
    protected bool rayHit;
    protected RaycastHit rayHitObject;
    [SerializeField]
    protected bool enemyIsForward;
    IEnumerator NavCoroutine() // 적을 추적하는 무한반복 코루틴
    {
        while (true)
        {
            if(target != null) enemyDistance = Vector3.Distance(this.transform.position, target.position);
            if (target == null || enemyDistance > chaseRange)
            {
                UpdateTarget();
                yield return null; // 튕김 방지
                continue;
            }

            if (unitType == Type.archer || unitType == Type.mage) 
            {
                Vector3 enemySpeed = target.GetComponent<Enemy>().dir * 5f;
                nav.SetDestination(target.position + enemySpeed);
            } 
            else nav.SetDestination(target.position);

            if (CanAttack() && !isAttack) // Attack가능하고 쿨타임이 아니면 공격
                NormalAttack();
            yield return null;
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

    // 타워 때리는 무한반복 코루틴
    IEnumerator TowerNavCoroutine() 
    {
        while (true)
        {
            if(target != null)
            {
                nav.SetDestination(target.position);
                if (rayHit)
                {
                    if (rayHitObject.transform.gameObject.CompareTag("Tower") && !isAttack) NormalAttack();
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Tower")
        {
            StopCoroutine("NavCoroutine");
            nav.isStopped = false;
            StartCoroutine("TowerNavCoroutine");
        }
    }

    protected Enemy GetEnemyScript()
    {
        Enemy enemy = null;
        if (target != null) enemy = target.gameObject.GetComponentInChildren<Enemy>();
        return enemy;
    }

    private void OnMouseDown()
    {
        GameManager.instance.Chilk();
        Combine.ButtonOn();
    }
}