﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;

public enum EnemyType
{
    Normal,
    Boss,
    Tower,
}

public class Multi_Enemy : MonoBehaviourPun
{
    // 상태 변수
    [SerializeField] protected int maxHp = 0;
    protected void ChangeMaxHp(int newMaxHp)
    {
        maxHp = newMaxHp;
        hpSlider.maxValue = newMaxHp;
        ChangeHp(newMaxHp); // maxHp 갱신되면 currentHp도 같이 풀로 채워짐
    }
    public int currentHp = 0;
    void ChangeHp(int newHp)
    {
        if (newHp > maxHp) newHp = maxHp;
        currentHp = newHp;
        hpSlider.value = newHp;
    }

    [SerializeField] protected float maxSpeed = 0;
    protected void ChangeMaxSpeed(float newMaxSpeed)
    {
        maxSpeed = newMaxSpeed;
        ChangeSpeed(maxSpeed);
    }
    [SerializeField] protected float speed = 0;
    public float Speed => speed;
    protected virtual void ChangeSpeed(float newSpeed)
    {
        if (newSpeed > maxSpeed) newSpeed = maxSpeed;
        speed = newSpeed;
    }
    

    bool isDead = true;
    public bool IsDead => isDead;
    public Slider hpSlider = null;
    public EnemyType enemyType;

    public Vector3 dir = Vector3.zero;

    RPCable rpcable;
    public int UsingId => rpcable.UsingId;
    protected MeshRenderer[] meshList;
    [SerializeField] protected Material originMat;

    public event Action OnDeath = null;
    public event Action<Multi_Enemy> OnDead = null;

    private void Awake()
    {
        rpcable = GetComponent<RPCable>();
        meshList = GetComponentsInChildren<MeshRenderer>();
        Init();
    }

    protected virtual void Init() { }

    public void SetStatus_RPC(int _hp, float _speed, bool _isDead) => photonView.RPC(nameof(SetStatus), RpcTarget.All, _hp, _speed, _isDead);

    [PunRPC]
    protected virtual void SetStatus(int _hp, float _speed, bool _isDead)
    {
        gameObject.SetActive(!_isDead);
        ChangeMaxHp(_hp);
        ChangeMaxSpeed(_speed);
        isDead = _isDead;
    }

    public void OnDamage(int damage, bool isSkill = false) => photonView.RPC(nameof(RPC_OnDamage), RpcTarget.MasterClient, damage, isSkill);
    [PunRPC]
    protected virtual void RPC_OnDamage(int damage, bool isSkill)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ChangeHp(currentHp - damage);
            photonView.RPC(nameof(RPC_UpdateHealth), RpcTarget.Others, currentHp);

            // 게스트에서 조건문 밖의 Dead부분을 실행시키게 하기 위한 코드
            photonView.RPC(nameof(RPC_OnDamage), RpcTarget.Others, 0, false);
        }

        // Dead는 보상 등 개인적으로 실행되어야 하는 기능이 포함되어 있으므로 모두 실행
        if (currentHp <= 0 && !isDead) Dead();
    }

    [PunRPC]
    protected void RPC_UpdateHealth(int _newHp) => ChangeHp(_newHp);

    public virtual void Dead()
    {
        OnDeath?.Invoke();
        OnDead?.Invoke(this);
        OnDead = null;
        ResetValue();
        Managers.Multi.Instantiater.PhotonDestroy(gameObject);
    }

    protected virtual void ResetValue()
    {
        SetStatus(0, 0, true);
        queue_HoldingPoison.Clear();
    }

    protected void ResetColor()
    {
        ChangeColorToOrigin();
        ChangeMat(originMat);
    }

    // 상태 이상은 호스트에서 적용 후 다른 플레이어에게 동기화하는 방식
    public void OnSlow_RPC(float slowPercent, float slowTime)
    {
        if (isDead) return;
        ChangeColorToSlow();
        photonView.RPC(nameof(OnSlow), RpcTarget.MasterClient, slowPercent, slowTime);
    }
    [PunRPC] protected virtual void OnSlow(float slowPercent, float slowTime) { }

    public void ExitSlow(RpcTarget _target) => photonView.RPC(nameof(ExitSlow), _target);
    [PunRPC] protected virtual void ExitSlow() { }

    public void OnFreeze_RPC(float _freezeTime) => photonView.RPC(nameof(OnFreeze), RpcTarget.MasterClient, _freezeTime);
    [PunRPC] protected virtual void OnFreeze(float slowTime) { } // 얼리는 스킬

    public void OnStun_RPC(int _stunPercent, float _stunTime) => photonView.RPC(nameof(OnStun), RpcTarget.MasterClient, _stunPercent, _stunTime);
    [PunRPC] protected virtual void OnStun(int stunPercent, float stunTime) { }

    public void OnPoison_RPC(int poisonPercent, int poisonCount, float poisonDelay, int maxDamage, bool isSkill = false)
    {
        if (isDead) return;

        photonView.RPC(nameof(OnPoison), RpcTarget.MasterClient, poisonPercent, poisonCount, poisonDelay, maxDamage, isSkill);
    }
    [PunRPC]
    protected virtual void OnPoison(int poisonPercent, int poisonCount, float poisonDelay, int maxDamage, bool isSkill)
    {
        if (isDead || !PhotonNetwork.IsMasterClient) return;
        ChangeColorToPoison();
        StartCoroutine(Co_OnPoison(poisonPercent, poisonCount, poisonDelay, maxDamage, isSkill));
    }

    // Queue를 사용해서 현재 코루틴이 중복으로 돌아가고 있지 않으면 색깔 복귀하기
    Queue<int> queue_HoldingPoison = new Queue<int>();
    IEnumerator Co_OnPoison(int poisonPercent, int poisonCount, float poisonDelay, int maxDamage, bool isSkill)
    {
        queue_HoldingPoison.Enqueue(-1);
        
        int poisonDamage = GetPoisonDamage(poisonPercent, maxDamage);
        for (int i = 0; i < poisonCount; i++)
        {
            yield return new WaitForSeconds(poisonDelay);
            RPC_OnDamage(poisonDamage, isSkill); // 포이즌 자체가 마스터에서만 돌아가기 때문에 그냥 써도 됨
        }

        if (queue_HoldingPoison.Count != 0) queue_HoldingPoison.Dequeue();
        if (queue_HoldingPoison.Count == 0) photonView.RPC(nameof(ChangeColorToOrigin), RpcTarget.All);
    }

    int GetPoisonDamage(int poisonPercent, int maxDamage)
    {
        int poisonDamage = Mathf.RoundToInt(currentHp * poisonPercent / 100);
        poisonDamage = Mathf.Clamp(poisonDamage, 1, maxDamage);
        return poisonDamage;
    }

    void ChangeColor(byte r, byte g, byte b, byte a)
    {
        Color32 _newColor = new Color32(r, g, b, a);
        foreach (MeshRenderer mesh in meshList)
            mesh.material.color = _newColor;
    }

    protected void ChangeColorToPoison() => ChangeColor(141, 49, 231, 255);
    protected void ChangeColorToSlow() => ChangeColor(50, 175, 222, 1);
    [PunRPC]
    protected void ChangeColorToOrigin() => ChangeColor(255, 255, 255, 255);

    protected void ChangeMat(Material mat)
    {
        foreach (MeshRenderer mesh in meshList) 
            mesh.material = mat;
    }
}