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

    public float stopDistanc;
    public bool isAttack; // 공격 중에 true인 변수
    public bool isAttackDelayTime; // 공격 못할 때 true 변수

    protected NavMeshAgent nav;
    public Transform target;
    protected NomalEnemy nomalEnemy;
    protected EnemySpawn enemySpawn;

    protected AudioSource unitAudioSource;
    public AudioClip normalAttackClip;
    public float normalAttakc_AudioDelay;
    public AudioClip tpAudioClip;

    private float chaseRange; // 풀링할 때 멀리 풀에 있는 놈들 충돌 안하게 하기위한 추적 최소거리
    private void Start()
    {
        UnitManager.instance.currentUnitList.Add(gameObject);
        // 인스턴스
        unitAudioSource = GetComponentInParent<AudioSource>();
        enemySpawn = FindObjectOfType<EnemySpawn>();
        nav = GetComponentInParent<NavMeshAgent>();
        // 변수 선언
        SetPassive();
        chaseRange = 150f;
        nav.speed = this.speed;
        layerMask = 1 << LayerMask.NameToLayer("Enemy"); // Ray가 Enemy 레이어만 충돌 체크함
        // 적 추적
        UpdateTarget();
        StartCoroutine("NavCoroutine");
    }

    public virtual void SetPassive() {} // 나중에 Action 인자 받아서 깔끔하게 바꿀수도

    public int specialAttackPercent;
    void UnitAttack()
    {
        int random = Random.Range(0, 100);
        if(random < specialAttackPercent)
        {
            SpecialAttack();
        }
        else
        {
            NormalAttack();
            Invoke("NormalAttackAudioPlay", normalAttakc_AudioDelay);
        }
    }

    void NormalAttackAudioPlay()
    {
        if(enterStoryWorld == GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(normalAttackClip);
    }
    public virtual void NormalAttack()
    {
        Invoke("ReadyAttack", attackDelayTime);
    }
    void ReadyAttack()
    {
        isAttackDelayTime = false;
    }

    public virtual void SpecialAttack() // 유닛마다 다른 스킬공격 (기사, 법사는 없음)
    {

    }

    public virtual void EenmyChase() // 추적
    {

    }

    protected int layerMask; // Ray 감지용
    [SerializeField]
    protected float enemyDistance;
    protected bool rayHit;
    protected RaycastHit rayHitObject;

    [SerializeField]
    protected bool enemyIsForward;

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
            //Debug.Log(rayHitObject);
            if (rayHitObject.transform.gameObject.CompareTag("Tower") || rayHitObject.transform.gameObject.CompareTag("Boss"))
                return true;
            else if (rayHitObject.transform.gameObject == target.parent.gameObject) return true;
            else return false;
        }
        else return false;
    }

    IEnumerator NavCoroutine() // 적을 추적하는 무한반복 코루틴
    {
        // boss 있으면 보스만 추격
        if (enemySpawn.bossRespawn && enemySpawn.currentBossList[0] != null) target = enemySpawn.currentBossList[0].transform;
        while (true)
        {
            if (target != null) enemyDistance = Vector3.Distance(this.transform.position, target.position);
            if (target == null || enemyDistance > chaseRange)
            {
                UpdateTarget();
                yield return null; // 튕김 방지
                continue;
            }

            if (unitType == Type.archer || unitType == Type.mage) 
            {
                Vector3 enemySpeed = target.GetComponent<NomalEnemy>().dir * target.GetComponent<NomalEnemy>().speed;
                nav.SetDestination(target.position + enemySpeed);
            } 
            else nav.SetDestination(target.position);

            if (enemyIsForward && !isAttackDelayTime) // Attack가능하고 쿨타임이 아니면 공격
            {
                //NormalAttack();
                UnitAttack();
            }
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
            nomalEnemy = target.gameObject.GetComponent<NomalEnemy>();
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
        Invoke("RangeNavStop", 4f);
        while (true)
        {
            if(target != null)
            {
                if (target.GetComponent<EnemyTower>().isDead)
                {
                    if(enemySpawn.currentTowerLevel < enemySpawn.towers.Length)
                        target = enemySpawn.towers[enemySpawn.currentTowerLevel].transform;
                    if (target == null) continue;
                    yield return null;
                }

                enemyDistance = Vector3.Distance(this.transform.position, target.position);
                nav.SetDestination(towerHit.point);
                if ((towerEnter || enemyIsForward) && !isAttackDelayTime) 
                {
                    UnitAttack();
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void RangeNavStop()
    {
        if (GetComponent<RangeUnit>() != null) nav.isStopped = true;
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
            transform.parent.position = SetRandomPosition(460, 540, -20, -30, true);
            enterStoryWorld = true;
            nav.enabled = true;
            StopCoroutine("NavCoroutine");
            target = GameObject.FindGameObjectWithTag("Tower").transform;
            StartCoroutine("TowerNavCoroutine");
        }
        else
        {
            transform.parent.position = SetRandomPosition(20, -20, 10, -10, false);
            towerEnter = false;
            enterStoryWorld = false;
            nav.enabled = true;
            StopCoroutine("TowerNavCoroutine");
            UpdateTarget();
            StartCoroutine("NavCoroutine");
        }
        unitAudioSource.PlayOneShot(tpAudioClip, 0.4f);
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

    protected Enemy GetEnemyScript()
    {
        Enemy enemy = null;
        if (target != null) enemy = target.gameObject.GetComponent<Enemy>();
        return enemy;
    }

    public void AttackEnemy(Enemy enemy)
    {
        if (enemy.gameObject.CompareTag("Tower") || enemy.gameObject.CompareTag("Boss"))
        {
            enemy.OnDamage(bossDamage);
            //Debug.Log("OnBossDamage" + bossDamage);
        }
        else enemy.OnDamage(damage);
    }

    public AudioClip getGoldClip;
    protected void Add_PassiveGold(int percent, int addGold)
    {
        int random = Random.Range(0, 100);
        if(random < percent)
        {
            unitAudioSource.PlayOneShot(getGoldClip, 1f);
            GameManager.instance.Gold += addGold;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
        }
    }

    //// 이벤트 관련 함수
    //public virtual void SkillPercentUp() { }
    //public virtual void SkillPercentDown() { }
    //public virtual void ReinforcePassive() { }
    //public virtual void WeakenPassive() { }



    //private void OnMouseDown()
    //{
    //    GameManager.instance.Chilk();
    //    //Combine.ButtonOn();
    //    UIManager.instance.SellSoldier.gameObject.SetActive(true);
    //}
}