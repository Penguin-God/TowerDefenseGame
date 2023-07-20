using System.Collections;
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

    bool isDead = true;
    public bool IsDead => isDead;
    public Slider hpSlider = null;
    public EnemyType enemyType;

    public Vector3 dir = Vector3.zero;

    RPCable rpcable;
    public byte UsingId => rpcable == null ? (byte)GetComponent<RPCable>().UsingId : (byte)rpcable.UsingId;
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
        isDead = _isDead;
    }

    public void OnDamage(int damage, bool isSkill = false) => photonView.RPC(nameof(RPC_OnDamage), RpcTarget.MasterClient, damage, isSkill);
    [PunRPC]
    protected virtual void RPC_OnDamage(int damage, bool isSkill)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        ChangeHp(currentHp - damage);
        photonView.RPC(nameof(RPC_UpdateHealth), RpcTarget.Others, currentHp);

        if (currentHp <= 0 && isDead == false)
            photonView.RPC(nameof(RPC_Dead), RpcTarget.All);
    }

    [PunRPC] // RPC 매서드라 protected임
    protected void RPC_UpdateHealth(int _newHp) => ChangeHp(_newHp);

    [PunRPC]
    protected void RPC_Dead() => Dead();

    public virtual void Dead()
    {
        isDead = true;
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

    // 상태 이상은 마스트에서 적용 후 다른 플레이어에게 동기화하는 방식
    public virtual void OnSlow(float slowPercent, float slowTime) { }

    public void ExitSlow(RpcTarget _target) => photonView.RPC(nameof(RestoreSpeed), _target);
    [PunRPC] protected virtual void RestoreSpeed() { }

    public void OnFreeze_RPC(float _freezeTime) => photonView.RPC(nameof(OnFreeze), RpcTarget.MasterClient, _freezeTime);
    [PunRPC] protected virtual void OnFreeze(float slowTime) { } // 얼리는 스킬

    public void OnStun_RPC(int _stunPercent, float _stunTime) => photonView.RPC(nameof(OnStun), RpcTarget.MasterClient, _stunPercent, _stunTime);
    [PunRPC] protected virtual void OnStun(int stunPercent, float stunTime) { }

    public void OnPoison_RPC(int poisonCount, int damage, bool isSkill = false)
    {
        if (isDead) return;

        photonView.RPC(nameof(OnPoison), RpcTarget.MasterClient, (byte)poisonCount, damage, isSkill);
    }
    [PunRPC]
    protected virtual void OnPoison(byte poisonCount, int damage, bool isSkill)
    {
        if (isDead) return;
        photonView.RPC(nameof(ChangeColorToPoison), RpcTarget.All);
        StartCoroutine(Co_OnPoison(poisonCount, damage, isSkill));
    }

    // Queue를 사용해서 현재 코루틴이 중복으로 돌아가고 있지 않으면 색깔 복귀하기
    Queue<int> queue_HoldingPoison = new Queue<int>();
    const float PoisonTick = 0.25f;
    IEnumerator Co_OnPoison(int poisonCount, int damage, bool isSkill)
    {
        queue_HoldingPoison.Enqueue(-1);
        for (int i = 0; i < poisonCount; i++)
        {
            yield return new WaitForSeconds(PoisonTick);
            RPC_OnDamage(damage, isSkill); // 포이즌 자체가 마스터에서만 돌아가기 때문에 그냥 써도 됨
        }

        if (queue_HoldingPoison.Count != 0) queue_HoldingPoison.Dequeue();
        if (queue_HoldingPoison.Count == 0) photonView.RPC(nameof(ChangeColorToOrigin), RpcTarget.All);
    }

    void ChangeColor(byte r, byte g, byte b, byte a)
    {
        Color32 _newColor = new Color32(r, g, b, a);
        foreach (MeshRenderer mesh in meshList)
            mesh.material.color = _newColor;
    }

    [PunRPC]
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