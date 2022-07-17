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
    public float maxSpeed = 0;
    public float speed = 0;
    public int maxHp = 0;
    public int currentHp = 0;
    public bool isDead = true;
    public Slider hpSlider = null;
    public EnemyType enemyType;

    public Vector3 dir = Vector3.zero;

    protected RPCable rpcable;
    protected List<MeshRenderer> meshList;
    [SerializeField] protected Material originMat;

    PhotonView _PV;
    public PhotonView PV
    {
        get
        {
            if (_PV == null) return photonView;
            else return _PV;
        }
    }

    public event Action OnDeath = null;

    private void Start()
    {
        rpcable = GetComponent<RPCable>();
        // originMat = GetComponentInChildren<MeshRenderer>().material;
        _PV = GetComponent<PhotonView>();
        meshList = new List<MeshRenderer>();
        MeshRenderer[] addMeshs = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < addMeshs.Length; i++) meshList.Add(addMeshs[i]);
    }


    public void SetStatus_RPC(int _hp, float _speed, bool _isDead)
    {
        PV.RPC("SetStatus", RpcTarget.All, _hp, _speed, _isDead);
    }

    [PunRPC]
    protected virtual void SetStatus(int _hp, float _speed, bool _isDead)
    {
        maxHp = _hp;
        currentHp = _hp;
        hpSlider.maxValue = _hp;
        hpSlider.value = _hp;
        maxSpeed = _speed;
        speed = _speed;
        isDead = _isDead;
        gameObject.SetActive(!_isDead);
    }

    public void OnDamage(int damage) => _PV.RPC("RPC_OnDamage", RpcTarget.MasterClient, damage);
    [PunRPC]
    public void RPC_OnDamage(int damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            currentHp -= damage;
            hpSlider.value = currentHp;

            photonView.RPC("RPC_UpdateHealth", RpcTarget.Others, currentHp);

            // 게스트에서 조건문 밖의 Dead부분을 실행시키게 하기 위한 코드
            photonView.RPC("RPC_OnDamage", RpcTarget.Others, 0);
        }

        // Dead는 보상 등 개인적으로 실행되어야 하는 기능이 포함되어 있으므로 모두 실행
        if (currentHp <= 0 && !isDead) Dead();
    }

    [PunRPC]
    public void RPC_UpdateHealth(int _newHp)
    {
        currentHp = _newHp;
        hpSlider.value = currentHp;
    }

    public virtual void Dead()
    {
        ResetValue();
        OnDeath?.Invoke();
    }

    protected virtual void ResetValue()
    {
        SetStatus(0, 0, true);
        queue_HoldingPoison.Clear();
        ChangeColor(255, 255, 255, 255);
        ChangeMat(originMat);
    }

    // 상태 이상은 호스트에서 적용 후 다른 플레이어에게 동기화하는 방식
    public void OnSlow(RpcTarget _target, float slowPercent, float slowTime) => _PV.RPC("OnSlow", _target, slowPercent, slowTime);
    [PunRPC] protected virtual void OnSlow(float slowPercent, float slowTime) { }

    public void ExitSlow(RpcTarget _target) => _PV.RPC("ExitSlow", _target);
    [PunRPC] protected virtual void ExitSlow() { }

    public void OnFreeze_RPC(float _freezeTime) => _PV.RPC("OnFreeze", RpcTarget.MasterClient, _freezeTime);
    [PunRPC] protected virtual void OnFreeze(float slowTime) { } // 얼리는 스킬

    public void OnStun(RpcTarget _target, int _stunPercent, float _stunTime) => _PV.RPC("OnStun", _target, _stunPercent, _stunTime);
    [PunRPC] protected virtual void OnStun(int stunPercent, float stunTime) { }

    public void OnPoison(RpcTarget _target, int poisonPercent, int poisonCount, float poisonDelay, int maxDamage) 
        => _PV.RPC("OnPoison", _target, poisonPercent, poisonCount, poisonDelay, maxDamage);
    [PunRPC]
    protected virtual void OnPoison(int poisonPercent, int poisonCount, float poisonDelay, int maxDamage)
    {
        if (isDead || !PhotonNetwork.IsMasterClient) return;

        StartCoroutine(Co_OnPoison(poisonPercent, poisonCount, poisonDelay, maxDamage));
    }

    // Queue를 사용해서 현재 코루틴이 중복으로 돌아가고 있지 않으면 색깔 복귀하기
    Queue<int> queue_HoldingPoison = new Queue<int>();
    IEnumerator Co_OnPoison(int poisonPercent, int poisonCount, float poisonDelay, int maxDamage)
    {
        queue_HoldingPoison.Enqueue(-1);
        photonView.RPC("ChangeColor", RpcTarget.All, 141, 49, 231, 255);

        int poisonDamage = GetPoisonDamage(poisonPercent, maxDamage);
        for (int i = 0; i < poisonCount; i++)
        {
            yield return new WaitForSeconds(poisonDelay);
            RPC_OnDamage(poisonDamage); // 포이즌 자체가 호스트에서만 돌아가기 때문에 그냥 써도 됨
        }

        if (queue_HoldingPoison.Count != 0) queue_HoldingPoison.Dequeue();
        if (queue_HoldingPoison.Count == 0) photonView.RPC("ChangeColor", RpcTarget.All, 255, 255, 255, 255);
    }

    int GetPoisonDamage(int poisonPercent, int maxDamage)
    {
        int poisonDamage = Mathf.RoundToInt(currentHp * poisonPercent / 100);
        if (poisonDamage <= 0) poisonDamage = 1; // 독 최소뎀
        if (poisonDamage >= maxDamage) poisonDamage = maxDamage; // 독 최대뎀
        return poisonDamage;
    }


    [PunRPC]
    public void ChangeColor(int r, int g, int b, int a )
    {
        Color32 _newColor = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
        foreach (MeshRenderer mesh in meshList) 
            mesh.material.color = _newColor;
    }

    protected void ChangeMat(Material mat)
    {
        foreach (MeshRenderer mesh in meshList) 
            mesh.material = mat;
    }
}