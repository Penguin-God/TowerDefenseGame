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
        gameObject.AddComponent<MonsterStateSync>();
    }

    [PunRPC]
    protected override void RPC_OnDamage(int damage, bool isSkill)
    {
        if (IsSlow) // 슬로우 시 추뎀
            damage += Mathf.RoundToInt(damage * (SlowController.SlowIntensity / 100));
        base.RPC_OnDamage(damage, isSkill);
    }

    protected virtual void Passive() { }

    readonly Vector3[] _spawnPositons = new Vector3[]
    {
        new Vector3(-45, 0, 35),
        new Vector3(-45, 0, 535),
    };

    void SetStatus(int stage)
    {
        spawnStage = stage;
        SetStatus(Managers.Data.NormalEnemyDataByStage[stage].Hp, false);
        Passive();
    }

    protected void Go()
    {
        TurnPoints = MultiData.instance.GetEnemyTurnPoints(UsingId);
        pointIndex = 0;
        transform.position = _spawnPositons[UsingId];
        transform.rotation = Quaternion.identity;
        SetDirection();
    }

    MonsterSlowController _monsterSlowController;
    public SlowController SlowController { get; private set; }
    public void Inject(byte stage, MonsterSlowController monsterSlowController, SpeedManager speedManager)
    {
        spawnStage = stage;
        Inject(Managers.Data.NormalEnemyDataByStage[stage].Hp, monsterSlowController, speedManager);
    }

    public void Inject(int hp, MonsterSlowController monsterSlowController, SpeedManager speedManager)
    {
        _monsterSlowController = monsterSlowController;
        SpeedManager = speedManager;

        SlowController = GetComponent<SlowController>();
        SlowController.DependencyInject(SpeedManager);
        SlowController.OnChangeSpped -= ApplySlowEffect;
        SlowController.OnChangeSpped += ApplySlowEffect;

        SetStatus(hp, false);
        Passive();
        Go();
    }

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

    public void ChangeVelocity(float speed)
    {
        SpeedManager.ChangeSpeed(speed);
        Rigidbody.velocity = dir * speed;
    }

    void ChangeVelocity(Vector3 direction)
    {
        Rigidbody.velocity = direction * Speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WayPoint")
        {
            Turn();
            SetDirection();
        }
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        isResurrection = false;
        spawnStage = 0;
        sternEffect.SetActive(false);
        pointIndex = -1;
        transform.rotation = Quaternion.identity;
        _stunCount = 0;
        SpeedManager = null;
        ResetColor();
        StopAllCoroutines();
    }

    protected SpeedManager SpeedManager;
    public float Speed => IsStun || IsDead || SpeedManager == null ? 0 : SpeedManager.CurrentSpeed;

    #region 상태이상 구현
    [SerializeField] private Material freezeMat;
    public bool IsSlow => SlowController.IsSlow;

    int _stunCount = 0;
    bool IsStun => _stunCount > 0;
    [SerializeField] private GameObject sternEffect;

    public void OnSlow(float slowRate)
    {
        if (IsDead) return;
        ChangeColorToSlow();

        if (PhotonNetwork.IsMasterClient == false) return;
        _monsterSlowController.Slow(Slow.CreateInfinitySlow(slowRate));
        ChangeVelocity(dir);
    }

    public void OnSlowWithTime(float slowRate, float slowTime, UnitFlags flag)
    {
        if(IsDead) return;
        ChangeColorToSlow();

        if (PhotonNetwork.IsMasterClient == false) return;
        _monsterSlowController.Slow(Slow.CreateDurationSlow(slowRate, slowTime), flag);
        ChangeVelocity(dir);
    }

    void ChangeColorToSlow() => ChangeColor(50, 175, 222, 1);

    [PunRPC] protected void RestoreColor() => ResetColor();

    void ApplySlowEffect(bool isSlow)
    {
        if(SpeedManager.CurrentSpeed > 0) ResetColor();

        if (isSlow) ChangeColorToSlow();
        else
        {
            ResetColor();
            if (PhotonNetwork.IsMasterClient)
                photonView.RPC(nameof(RestoreColor), RpcTarget.Others);
        }
        ChangeVelocity(dir);
    }

    public void OnFreeze(float slowTime, UnitFlags flag)
    {
        if (IsDead) return;
        ChangeMat(freezeMat);

        if (PhotonNetwork.IsMasterClient == false) return;
        OnSlowWithTime(100f, slowTime, flag);
    }

    public void OnStunToAll(float stunTime)
    {
        if (IsDead || PhotonNetwork.IsMasterClient == false) return;
        photonView.RPC(nameof(OnStun), RpcTarget.All, stunTime);
    }

    [PunRPC]
    public void OnStun(float stunTime)
    {
        if (IsDead) return;
        StartCoroutine(Co_Stun(stunTime));
    }

    IEnumerator Co_Stun(float stunTime)
    {
        _stunCount++;
        Stun();
        yield return new WaitForSeconds(stunTime);

        _stunCount--;
        if (IsStun == false) ExitStun();
    }

    void ExitStun()
    {
        sternEffect.SetActive(false);
        ChangeVelocity(dir);
    }

    void Stun()
    {
        ChangeVelocity(Vector3.zero);
        sternEffect.SetActive(true);
    }
    #endregion
}
