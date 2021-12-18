using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeamSoldier : MonoBehaviour
{    
    public enum Type { sowrdman, archer, spearman, mage }
    public Type unitType;

    public enum UnitColor { red, blue, yellow, green, orange, violet, white, black};
    public UnitColor unitColor;

    // 아무 버프도 받지 않은 상태 변수 
    public int originDamage;
    public int originBossDamage;
    public float originAttackDelayTime;

    public float speed;
    public float attackDelayTime;
    public float attackRange;
    public int damage;
    public int bossDamage;
    public int skillDamage;
    public float stopDistanc;

    // 상태 변수
    public bool isAttack; // 공격 중에 true
    public bool isAttackDelayTime; // 공격 쿨타임 중에 true
    // 나중에 유닛별 공격 조건 만들면서 없애기
    public bool isSkillAttack; // 스킬 공격 중에 true

    protected NavMeshAgent nav;
    public Transform target;
    protected NomalEnemy nomalEnemy;
    protected EnemySpawn enemySpawn;

    protected AudioSource unitAudioSource;
    public AudioClip normalAttackClip;
    public float normalAttakc_AudioDelay;

    public GameObject reinforceEffect;
    protected float chaseRange; // 풀링할 때 멀리 풀에 있는 놈들 충돌 안하게 하기위한 추적 최소거리

    private void Start()
    {
        UnitManager.instance.currentUnitList.Add(gameObject);
        // 인스턴스
        unitAudioSource = GetComponentInParent<AudioSource>();
        enemySpawn = FindObjectOfType<EnemySpawn>();
        nav = GetComponentInParent<NavMeshAgent>();
        // 변수 선언

        chaseRange = 150f;
        enemyDistance = 150f;
        nav.speed = this.speed;
        // 평타 설정
        delegate_OnHit += (Enemy enemy) => AttackEnemy(enemy);
        delegate_OnHit += delegate_OnPassive;
        // 스킬 설정
        delegate_OnSkile += (Enemy enemy) => enemy.OnDamage(skillDamage);
        delegate_OnSkile += delegate_OnPassive;

        // 적 추적
        UpdateTarget();
        StartCoroutine("NavCoroutine");
    }

    [HideInInspector]
    public bool passiveReinForce;

    // 적에게 대미지 입히기, 패시브 적용 등의 역할을 하는 델리게이트
    public delegate void Delegate_OnHit(Enemy enemy);
    protected Delegate_OnHit delegate_OnHit; // 평타
    protected Delegate_OnHit delegate_OnSkile; // 스킬
    public Delegate_OnHit delegate_OnPassive; // 패시브

    public int specialAttackPercent;
    void UnitAttack()
    {
        int random = Random.Range(0, 100);
        if(random < specialAttackPercent) SpecialAttack();
        else
        {
            NormalAttack();
            PlayNormalAttackClip();
        }
    }

    protected void PlayNormalAttackClip()
    {
        StartCoroutine(Co_NormalAttackClipPlay());
    }

    IEnumerator Co_NormalAttackClipPlay()
    {
        yield return new WaitForSeconds(normalAttakc_AudioDelay);
        if (enterStoryWorld == GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(normalAttackClip);
    }

    protected void StartAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;
    }

    public virtual void NormalAttack()
    {
        // override 코루틴 마지막 부분에서 실행하는 코드
        StartCoroutine(Co_ResetAttactStatus());
        if (target != null && TragetIsNormalEnemy) UpdateTarget();
    }

    IEnumerator Co_ResetAttactStatus()
    {
        isAttack = false;
        yield return new WaitForSeconds(attackDelayTime);
        isAttackDelayTime = false;
    }

    public virtual void SpecialAttack() { } // 유닛마다 다른 스킬공격 (기사, 법사는 없음)

    protected int layerMask; // Ray 감지용
    [SerializeField]
    protected float enemyDistance;
    protected bool rayHit;
    protected RaycastHit rayHitObject;

    public bool enemyIsForward;

    private void Update()
    {
        if (target == null) return;

        UnitTypeMove();
        enemyIsForward = Set_EnemyIsForword();
    }

    public virtual void UnitTypeMove() {} // 유닛에 따른 이동
    public Transform rayHitTransform;
    bool Set_EnemyIsForword()
    {
        if (rayHit)
        {
            rayHitTransform = rayHitObject.transform;
            if (rayHitTransform == null) return false;

            if (CheckObjectIsBoss(rayHitTransform.gameObject) || rayHitTransform == target.parent) return true;
            else if(ReturnLayerMask(rayHitTransform.GetChild(0).gameObject) == layerMask)
            {
                // ray에 맞은 적이 target은 아니지만 target과 같은 layer라면 두 enemy가 겹친 것으로 판단해 target을 바꾸고 true를 리턴
                target = rayHitTransform.GetChild(0);
                return true;
            }
        }

        return false;
    }

    bool CheckObjectIsBoss(GameObject enemy)
    {
        return enemy.CompareTag("Tower") || enemy.CompareTag("Boss");
    }

    int ReturnLayerMask(GameObject targetObject) // 인자의 layer를 반환하는 함수
    {
        int layer = targetObject.layer;
        string layerName = LayerMask.LayerToName(layer);
        return 1 << LayerMask.NameToLayer(layerName);
    }

    IEnumerator NavCoroutine() // 적을 추적하는 무한반복 코루틴
    {
        // 적군의 성에서 돌아올 때 boss 있으면 보스만 추격
        if (enemySpawn.bossRespawn && enemySpawn.currentBossList[0] != null) SetChaseSetting(enemySpawn.currentBossList[0].gameObject);
        while (true)
        {
            if (target != null) enemyDistance = Vector3.Distance(this.transform.position, target.position);
            if (target == null || enemyDistance > chaseRange)
            {
                UpdateTarget();
                yield return null; // 튕김 방지
                continue;
            }

            if (GetComponent<RangeUnit>() != null) 
            {
                Enemy enemy = target.GetComponent<Enemy>();
                Vector3 enemySpeed = enemy.dir * enemy.speed;
                nav.SetDestination(target.position + enemySpeed);
            } 
            else nav.SetDestination(target.position);

            if ( (enemyIsForward || contactEnemy) && !isAttackDelayTime && !isSkillAttack && !isAttack) // Attack가능하고 쿨타임이 아니면 공격
            {
                UnitAttack();
            }
            yield return null;
        }
    }
    public bool contactEnemy = false;

    public void UpdateTarget() // 가장 가까운 거리에 있는 적으로 타겟을 바꿈
    {
        if (enemySpawn.bossRespawn) // 보스 있으면 보스가 타겟
        {
            SetChaseSetting(enemySpawn.currentBossList[0].gameObject);
            return;
        }

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

    public void SetChaseSetting(GameObject targetObject) // 추적 관련 변수 설정
    {
        if (targetObject != null)
        {
            nav.isStopped = false;
            target = targetObject.transform;
            nomalEnemy = target.gameObject.GetComponent<NomalEnemy>();
            layerMask = ReturnLayerMask(target.gameObject);
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
        Physics.Raycast(transform.parent.position + Vector3.up, target.position - transform.position, out RaycastHit towerHit, 100f, layerMask);
        if (target != null) enemyDistance = Vector3.Distance(this.transform.position, towerHit.point);

        Invoke("RangeNavStop", 3f); // 원거리 타워에 다가가는거 막기
        while (true)
        {
            if (target == null || enemyDistance > chaseRange)
            {
                EnemyTower currentTower = enemySpawn.currentTower;
                if (currentTower.isRespawn) target = currentTower.transform;
                else
                {
                    yield return null;
                    continue;
                }
            }

            nav.SetDestination(towerHit.point);
            enemyDistance = Vector3.Distance(this.transform.position, towerHit.point);
            
            if ((towerEnter || enemyIsForward) && !isAttackDelayTime && !isSkillAttack && !isAttack)
                UnitAttack();

            yield return new WaitForSeconds(0.5f);
        }
    }

    void RangeNavStop()
    {
        if (GetComponent<RangeUnit>() != null) 
        {
            nav.isStopped = true;
            nav.speed = 0f;
        }
    }

    [SerializeField]
    protected bool towerEnter; // 타워 충돌감지
    public bool enterStoryWorld;

    public void Unit_WorldChange()
    {
        StopCoroutine("Unit_WorldChange_Coroutine");
        StartCoroutine("Unit_WorldChange_Coroutine");
    }

    IEnumerator Unit_WorldChange_Coroutine() // 월드 바꾸는 함수
    {
        yield return new WaitUntil(() => !isAttack);
        nav.enabled = false;
        UnitManager.instance.ShowTpEffect(transform);
        
        if (!enterStoryWorld)
        {
            // 적군의 성 때 겹치는 버그 방지
            nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            transform.parent.position = UnitManager.instance.Set_StroyModePosition();
            StopCoroutine("NavCoroutine");
            target = GameObject.FindGameObjectWithTag("Tower").transform;
            layerMask = ReturnLayerMask(target.gameObject);
            StartCoroutine("TowerNavCoroutine");
        }
        else
        {
            nav.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
            transform.parent.position = SetRandomPosition(20, -20, 10, -10, false);
            towerEnter = false;
            StopCoroutine("TowerNavCoroutine");
            UpdateTarget();
            StartCoroutine("NavCoroutine");
        }

        nav.enabled = true;
        enterStoryWorld = !enterStoryWorld;
        SoundManager.instance.PlayEffectSound_ByName("TP_Unit");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tower")
        {
            towerEnter = true;
        }
    }

    Vector3 SetRandomPosition(float maxX, float minX, float maxZ, float minZ, bool isTower)
    {
        float randomX;
        if (isTower)
        {
            float randomX_1 = Random.Range(minX, 480);
            float randomX_2 = Random.Range(520, maxX);
            int xArea = Random.Range(0, 2);
            randomX = (xArea == 0) ? randomX_1 : randomX_2;
        }
        else randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        return new Vector3(randomX, 0, randomZ);
    }

    // 현재 타겟이 노말인지 아닌지 나타내는 프로퍼티
    protected bool TragetIsNormalEnemy { get { return (target != null && target.GetComponent<NomalEnemy>() ); } }

    void AttackEnemy(Enemy enemy) // Boss enemy랑 쫄병 구분
    {
        if (TragetIsNormalEnemy) enemy.OnDamage(damage);
        else enemy.OnDamage(bossDamage);
    }
}