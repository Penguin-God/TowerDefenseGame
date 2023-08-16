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
        if (IsSlow) // 슬로우 시 추뎀
            damage += Mathf.RoundToInt(damage * (SpeedManager.ApplySlowRate / 100));
        base.RPC_OnDamage(damage, isSkill);
    }

    protected virtual void Passive() { }
    [SerializeField] float _speed;
    [SerializeField] float _originSpeed;
    [PunRPC]
    protected override void SetStatus(int _hp, float speed, bool _isDead)
    {
        base.SetStatus(_hp, speed, _isDead);
        spawnStage = StageManager.Instance.CurrentStage;
        TurnPoints = Multi_Data.instance.GetEnemyTurnPoints(UsingId);
        if(pointIndex == -1) pointIndex = 0;
        transform.position = _spawnPositons[UsingId];
        transform.rotation = Quaternion.identity;
        if (PhotonNetwork.IsMasterClient)
        {
            Passive();
            _speed = SpeedManager.OriginSpeed;
            photonView.RPC(nameof(SetInfo), RpcTarget.All, maxHp, _speed);
        }
    }

    [PunRPC] 
    protected void SetInfo(int hp, float speed)
    {
        ChangeMaxHp(hp);
        _speed = speed;
        _originSpeed = speed;
        SetDirection();
    }

    public void Inject(SpeedManager speedManager, int hp)
    {
        SpeedManager = speedManager;
        photonView.RPC(nameof(SetStatus), RpcTarget.All, hp, SpeedManager.OriginSpeed, false);
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
        if(IsDead) return;
        dir = (WayPoint.position - transform.position).normalized;
        ChangeVelocity(dir);
    }

    void ChangeVelocity(Vector3 direction) => Rigidbody.velocity = direction * Speed;

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
        SpeedManager = null;
        isResurrection = false;
        spawnStage = 0;
        sternEffect.SetActive(false);
        ResetColor();
        pointIndex = -1;
        transform.rotation = Quaternion.identity;
        _stunCount = 0;
        _speed = 0;
        StopCoroutine(nameof(Co_RestoreSpeed));
    }

    protected SpeedManager SpeedManager { get; private set; }
    protected MonsterSpeedManager MonsterSpeedManager { get; private set; }

    public float Speed => IsStun ? 0 : _speed;
    public bool IsSlow => SpeedManager == null ? false : SpeedManager.IsSlow;
    bool IsStun => _stunCount > 0;
    bool RPCSendable => IsDead == false && PhotonNetwork.IsMasterClient;
    #region 상태이상 구현

    [SerializeField] private Material freezeMat;

    int _stunCount = 0;
    [SerializeField] private GameObject sternEffect;

    public override void OnSlow(float slowRate, float slowTime)
    {
        if (RPCSendable == false) return;
        SpeedManager.OnSlow(slowRate);
        ApplySlowToAll(slowTime);
    }

    void ApplySlowToAll(float slowTime)
    {
        if (RPCSendable == false) return;
        photonView.RPC(nameof(ApplySlow), RpcTarget.All, SpeedManager.CurrentSpeed);
        ApplySlowTime(slowTime);
    }
    [PunRPC]
    protected void ApplySlow(float newSpeed)
    {
        _speed = newSpeed;
        ChangeVelocity(dir);
        ChangeColorToSlow();
    }

    void ApplySlowTime(float slowTime)
    {
        StopCoroutine(nameof(Co_RestoreSpeed));
        StartCoroutine(nameof(Co_RestoreSpeed), slowTime);
    }

    IEnumerator Co_RestoreSpeed(float slowTime)
    {
        yield return new WaitForSeconds(slowTime);
        RestoreSpeedToAll();
    }

    public void RestoreSpeedToAll()
    {
        if (RPCSendable == false) return;
        SpeedManager.RestoreSpeed();
        photonView.RPC(nameof(RestoreSpeed), RpcTarget.All);
    }

    [PunRPC] // rpc용 proteted
    protected void RestoreSpeed()
    {
        ChangeMat(originMat);
        ChangeColorToOrigin();
        _speed = _originSpeed;
        ChangeVelocity(dir);
    }


    public void OnFreeze(float slowTime)
    {
        if (RPCSendable == false) return;

        OnSlow(100f, slowTime);
        photonView.RPC(nameof(Mat_To_Freeze), RpcTarget.All);
    }

    [PunRPC] protected void Mat_To_Freeze() => ChangeMat(freezeMat);

    [PunRPC]
    protected override void OnStun(int stunPercent, float stunTime)
    {
        if (RPCSendable == false) return;
        int random = UnityEngine.Random.Range(0, 100);
        if (random < stunPercent) StartCoroutine(Co_Stun(stunTime));
    }

    IEnumerator Co_Stun(float stunTime)
    {
        _stunCount++;
        photonView.RPC(nameof(Stun), RpcTarget.All);
        yield return new WaitForSeconds(stunTime);

        _stunCount--;
        if(IsStun == false) photonView.RPC(nameof(ExitStun), RpcTarget.All);
    }

    [PunRPC]
    protected void ExitStun()
    {
        sternEffect.SetActive(false);
        ChangeVelocity(dir);
    }

    [PunRPC]
    protected void Stun()
    {
        ChangeVelocity(Vector3.zero);
        sternEffect.SetActive(true);
    }
    #endregion
}
