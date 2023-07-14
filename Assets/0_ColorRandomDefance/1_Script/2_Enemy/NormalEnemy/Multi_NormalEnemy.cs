using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_NormalEnemy : Multi_Enemy
{
    // 이동, 회전 관련 변수
    [SerializeField] Transform[] TurnPoints;
    private Transform WayPoint => TurnPoints[pointIndex];
    private int pointIndex = -1;

    protected Rigidbody Rigidbody;

    [SerializeField] bool isResurrection = false;
    public bool IsResurrection => isResurrection;
    public void Resurrection() => isResurrection = true;

    [SerializeField] int spawnStage;
    public int SpawnStage => spawnStage;

    protected override void Init()
    {
        Rigidbody = GetComponent<Rigidbody>();
        enemyType = EnemyType.Normal;
    }

    [PunRPC]
    protected override void RPC_OnDamage(int damage, bool isSkill)
    {
        if (IsSlow)
            damage += Mathf.RoundToInt(damage * (_slowSystem.SlowPercent / 100));
        base.RPC_OnDamage(damage, isSkill);
    }

    protected virtual void Passive() { }

    [PunRPC]
    protected override void SetStatus(int _hp, float _speed, bool _isDead)
    {
        base.SetStatus(_hp, _speed, _isDead);
        Passive();
        spawnStage = StageManager.Instance.CurrentStage;
        TurnPoints = Multi_Data.instance.GetEnemyTurnPoints(gameObject);
        if(pointIndex == -1) pointIndex = 0;
        transform.position = _spawnPositons[UsingId];
        transform.rotation = Quaternion.identity;
        SetDirection();
    }

    readonly Vector3[] _spawnPositons = new Vector3[]
    {
        new Vector3(-45, 0, 35),
        new Vector3(-45, 0, 535),
    };

    void Turn()
    {
        transform.position = WayPoint.position;
        transform.rotation = Quaternion.Euler(0, -90 * pointIndex, 0);
        pointIndex++;
        if (pointIndex >= TurnPoints.Length) pointIndex = 0;
    }

    void SetDirection() // 실제 이동을 위한 속도 설정
    {
        dir = (WayPoint.position - transform.position).normalized;
        Rigidbody.velocity = dir * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WayPoint")
        {
            Turn();
            SetDirection();
        }
    }

    public override void Dead()
    {
        base.Dead();
        gameObject.SetActive(false);
        transform.position = new Vector3(500, 500, 500);
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        isResurrection = false;
        spawnStage = 0;
        sternEffect.SetActive(false);
        queue_GetSturn.Clear();
        ResetColor();
        pointIndex = -1;
        transform.rotation = Quaternion.identity;
        _slowSystem = null;
    }

    // TODO : 상태이상 구현 코드 줄일 방법 찾아보기
    /// <summary>
    /// 상태이상 구현 표시
    /// </summary>
    #region 상태이상 구현

    SlowSystem _slowSystem = null;
    public bool IsSlow => _slowSystem != null;
    [SerializeField] private Material freezeMat;

    private Queue<int> queue_GetSturn = new Queue<int>();
    [SerializeField] private GameObject sternEffect;

    public override void OnSlow(float slowPercent, float slowTime)
    {
        if (IsDead || PhotonNetwork.IsMasterClient == false) return;
        OnSlow(new SlowSystem(slowPercent, slowTime));
    }

    void OnSlow(SlowSystem slowSystem)
    {
        if (IsSlow && _slowSystem.SlowPercent >= slowSystem.SlowPercent) return;

        _slowSystem = slowSystem;
        photonView.RPC(nameof(ApplySlow), RpcTarget.All, _slowSystem.ApplySlowToSpeed(maxSpeed));
        if (_slowSystem.SlowTime > 0)
            ApplySlowTime(_slowSystem.SlowTime);
    }

    void ApplySlowTime(float slowTime)
    {
        StopCoroutine(nameof(Co_ExitSlow));
        StartCoroutine(nameof(Co_ExitSlow), slowTime);
    }

    [PunRPC]
    protected void ApplySlow(float slowSpeed)
    {
        ChangeSpeed(slowSpeed);
        ChangeColorToSlow();
    }

    IEnumerator Co_ExitSlow(float slowTime)
    {
        yield return new WaitForSeconds(slowTime);
        photonView.RPC(nameof(ExitSlow), RpcTarget.All);
    }

    [PunRPC]
    protected override void ExitSlow()
    {
        ChangeMat(originMat);
        ChangeColorToOrigin();
        _slowSystem = null;

        // 스턴 상태가 아니라면 속도 복구
        if (queue_GetSturn.Count <= 0 && photonView.IsMine) Set_OriginSpeed_To_AllPlayer();
    }


    [PunRPC]
    protected override void OnFreeze(float slowTime)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        OnSlow(new SlowSystem(100f, slowTime));
        photonView.RPC(nameof(Mat_To_Freeze), RpcTarget.All);
    }

    [PunRPC] protected void Mat_To_Freeze() => ChangeMat(freezeMat);


    [PunRPC]
    protected override void OnStun(int stunPercent, float stunTime)
    {
        if (IsDead || PhotonNetwork.IsMasterClient == false) return;
        int random = UnityEngine.Random.Range(0, 100);
        if (random < stunPercent) StartCoroutine(SternCoroutine(stunTime));
    }


    IEnumerator SternCoroutine(float stunTime)
    {
        queue_GetSturn.Enqueue(-1);
        photonView.RPC(nameof(SyncSpeed), RpcTarget.All, 0f);
        photonView.RPC(nameof(ShowSturnEffetc), RpcTarget.All);
        yield return new WaitForSeconds(stunTime);

        if (queue_GetSturn.Count != 0) queue_GetSturn.Dequeue();
        if (queue_GetSturn.Count == 0) photonView.RPC(nameof(ExitStun), RpcTarget.All);
    }

    [PunRPC]
    protected void ExitStun()
    {
        sternEffect.SetActive(false);
        Set_OriginSpeed_To_AllPlayer();
    }

    [PunRPC]
    protected void ShowSturnEffetc()
    {
        sternEffect.SetActive(true);
    }

    [PunRPC] // sync : 동기화한다는 뜻의 동사
    protected void SyncSpeed(float _speed) => ChangeSpeed(_speed);

    protected override void ChangeSpeed(float newSpeed)
    {
        base.ChangeSpeed(newSpeed);
        Rigidbody.velocity = dir * Speed;
    }

    // 나중에 이동 tralslate로 바꿔서 스턴이랑 이속 다르게 처리하는거 시도해보기
    protected void Set_OriginSpeed_To_AllPlayer() => photonView.RPC(nameof(SyncSpeed), RpcTarget.All, maxSpeed);

    #endregion
}
