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
        _speedManager = new SpeedManager(_speed);
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
        ChangeVelocity(dir);
    }

    void ChangeVelocity(Vector3 direction)
    {
        if (IsStun) direction = Vector3.zero;
        Rigidbody.velocity = direction * _speedManager.CurrentSpeed;
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
        _speedManager = null;
        isResurrection = false;
        spawnStage = 0;
        sternEffect.SetActive(false);
        ResetColor();
        pointIndex = -1;
        transform.rotation = Quaternion.identity;
        _slowSystem = null;
        _stunCount = 0;
    }


    protected SpeedManager _speedManager;
    public float Speed => IsStun ? 0 : _speedManager.CurrentSpeed;
    bool IsStun => _stunCount > 0;
    #region 상태이상 구현

    SlowSystem _slowSystem = null;
    public bool IsSlow => _slowSystem != null;
    [SerializeField] private Material freezeMat;

    int _stunCount = 0;
    [SerializeField] private GameObject sternEffect;

    public override void OnSlow(float slowPercent, float slowTime)
    {
        if (IsDead || PhotonNetwork.IsMasterClient == false) return;
        OnSlow(new SlowSystem(slowPercent, slowTime));
    }

    // 여기부터 시작하는 Slow도매인 로직 빼고 의존성 주입받게 한 다음에 스킬 추가되면 바꾸면 되는거 아님?
    void OnSlow(SlowSystem slowSystem)
    {
        if ((IsSlow && _slowSystem.SlowPercent >= slowSystem.SlowPercent) || 0 >= slowSystem.SlowTime) return;

        _slowSystem = slowSystem;
        ApplySlowToAll(slowSystem.SlowData);
    }

    protected void ApplySlowToAll(SlowData slowData) => photonView.RPC(nameof(ApplySlow), RpcTarget.All, (byte)slowData.SlowPercent, slowData.SlowTime);

    [PunRPC]
    protected void ApplySlow(byte slowRate, float slowTime)
    {
        _speedManager.OnSlow(slowRate);
        ChangeVelocity(dir);
        ChangeColorToSlow();
        ApplySlowTime(slowTime);
    }

    void ApplySlowTime(float slowTime)
    {
        StopCoroutine(nameof(Co_RestoreSpeed));
        StartCoroutine(nameof(Co_RestoreSpeed), slowTime);
    }

    IEnumerator Co_RestoreSpeed(float slowTime)
    {
        yield return new WaitForSeconds(slowTime);
        photonView.RPC(nameof(RestoreSpeed), RpcTarget.All);
    }

    [PunRPC]
    protected override void RestoreSpeed()
    {
        ChangeMat(originMat);
        ChangeColorToOrigin();
        _slowSystem = null;
        _speedManager.RestoreSpeed();
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
