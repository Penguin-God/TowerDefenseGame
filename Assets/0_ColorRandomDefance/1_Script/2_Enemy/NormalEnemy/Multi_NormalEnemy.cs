﻿using System.Collections;
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
            damage += Mathf.RoundToInt(damage * (MonsterSpeedManager.ApplySlowRate / 100));
        base.RPC_OnDamage(damage, isSkill);
    }

    protected virtual void Passive() { }
    [PunRPC]
    protected override void SetStatus(int _hp, float speed, bool _isDead)
    {
        base.SetStatus(_hp, speed, _isDead);
        spawnStage = StageManager.Instance.CurrentStage;
        TurnPoints = MultiData.instance.GetEnemyTurnPoints(UsingId);
        if(pointIndex == -1) pointIndex = 0;
        transform.position = _spawnPositons[UsingId];
        transform.rotation = Quaternion.identity;
        if (PhotonNetwork.IsMasterClient)
        {
            Passive();
            photonView.RPC(nameof(SetInfo), RpcTarget.All, maxHp);
        }
    }

    [PunRPC]
    protected void SetInfo(int hp)
    {
        ChangeMaxHp(hp);
        SetDirection();
    }

    public void Inject(int hp)
    {
        MonsterSpeedManager = gameObject.GetOrAddComponent<MonsterSpeedSystem>();
        MonsterSpeedManager.OnRestoreSpeed -= ExitSlow;
        MonsterSpeedManager.OnRestoreSpeed += ExitSlow;
        SetStatus(hp, SpeedManager.OriginSpeed, false);
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
        ResetColor();
        pointIndex = -1;
        transform.rotation = Quaternion.identity;
        _stunCount = 0;
        MonsterSpeedManager.RestoreSpeed();
        MonsterSpeedManager.OnRestoreSpeed -= ExitSlow;
    }

    protected SpeedManager SpeedManager => MonsterSpeedManager.SpeedManager;
    protected MonsterSpeedSystem MonsterSpeedManager { get; private set; }

    public float Speed => IsStun || IsDead || MonsterSpeedManager == null ? 0 : SpeedManager.CurrentSpeed;
    public bool IsSlow => MonsterSpeedManager == null ? false : MonsterSpeedManager.IsSlow;
    bool IsStun => _stunCount > 0;
    bool RPCSendable => IsDead == false && PhotonNetwork.IsMasterClient;
    #region 상태이상 구현

    [SerializeField] private Material freezeMat;

    int _stunCount = 0;
    [SerializeField] private GameObject sternEffect;

    public void OnSlow(float slowRate)
    {
        if (RPCSendable == false) return;
        photonView.RPC(nameof(ApplySlow), RpcTarget.All, slowRate);
    }

    [PunRPC]
    protected void ApplySlow(float slowRate)
    {
        ChangeColorToSlow();
        MonsterSpeedManager.OnSlow(slowRate);
        ChangeVelocity(dir);
    }

    public void OnSlowWithTime(float slowRate, float slowTime, UnitFlags flag)
    {
        if (RPCSendable == false) return;
        photonView.RPC(nameof(ApplySlowToAll), RpcTarget.All, slowRate, slowTime, flag);
    }

    [PunRPC]
    protected void ApplySlowToAll(float slowRate, float slowTime, UnitFlags flag)
    {
        ChangeColorToSlow(); // 서순 무조건 지켜야됨
        MonsterSpeedManager.OnSlowWithTime(slowRate, slowTime, flag);
        ChangeVelocity(dir);
    }

    void ChangeColorToSlow() => ChangeColor(50, 175, 222, 1);

    public void RestoreSpeedToAll()
    {
        if (RPCSendable == false) return;
        photonView.RPC(nameof(RestoreSpeed), RpcTarget.All);
    }

    [PunRPC] protected void RestoreSpeed() => MonsterSpeedManager.RestoreSpeed();

    protected void ExitSlow()
    {
        ResetColor();
        ChangeVelocity(dir);
    }

    public void OnFreeze(float slowTime, UnitFlags flag)
    {
        if (RPCSendable == false) return;

        OnSlowWithTime(100f, slowTime, flag);
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
