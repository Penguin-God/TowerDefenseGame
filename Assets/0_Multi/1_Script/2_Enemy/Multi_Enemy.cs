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

    protected Rigidbody Rigidbody;
    protected List<MeshRenderer> meshList;
    [SerializeField] protected Material originMat;
    private void Start()
    {
        // 타워랑 일반 적들 구조가 달라서 나중에 if문으로 나눠야됨
        // originMat = GetComponentInChildren<MeshRenderer>().material;
        IsNonePassive = (GetComponent<MageEnemy>() != null);
        meshList = new List<MeshRenderer>();
        MeshRenderer[] addMeshs = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < addMeshs.Length; i++) meshList.Add(addMeshs[i]);
        gameObject.SetActive(false);
    }

    [PunRPC]
    public virtual void Setup(int _hp, float _speed)
    {
        maxHp = _hp;
        currentHp = _hp;
        hpSlider.maxValue = _hp;
        hpSlider.value = _hp;
        maxSpeed = _speed;
        speed = _speed;
        isDead = false;
    }


    [PunRPC]
    public void OnDamage(int damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            currentHp -= damage;
            hpSlider.value = currentHp;

            photonView.RPC("UpdateHealth", RpcTarget.Others, currentHp);

            // 조건문 밖의 Dead부분 실행을 위한 코드
            photonView.RPC("OnDamage", RpcTarget.Others, 0);
        }

        // Dead는 보상 등 개인적으로 실행되어야 하는 기능이 포함되어 있으므로 모두 실행
        if (currentHp <= 0 && !isDead) Dead();
    }

    [PunRPC]
    public void UpdateHealth(int _newHp)
    {
        currentHp = _newHp;
        hpSlider.value = currentHp;
    }

    public Action OnDeath = null;
    public virtual void Dead()
    {
        ResetValue();

        if (OnDeath != null) OnDeath();
    }

    void ResetValue()
    {
        sternEffect.SetActive(false);
        queue_GetSturn.Clear();
        queue_HoldingPoison.Clear();

        maxSpeed = 0;
        speed = 0;
        isDead = true;
        maxHp = 0;
        currentHp = 0;
        hpSlider.maxValue = 0;
        hpSlider.value = 0;
    }




    // 메이지는 패시브가 스킬 무효화
    // 패시브는 호스트에서 적용 후 다른 플레이어에게 동기화하는 방식
    // 패시브 테스트하는데 꼴받아서 임시로 MageEnemy로 함
    private bool IsNonePassive;

    [PunRPC]
    public void OnSlow(float slowPercent, float slowTime)
    {
        if (IsNonePassive || isDead) return;

        if (PhotonNetwork.IsMasterClient)
        {
            // 슬로우를 적용했을 때 현재 속도보다 느려져야만 슬로우 적용
            if (maxSpeed - maxSpeed * (slowPercent / 100) <= speed)
            {
                speed = maxSpeed - maxSpeed * (slowPercent / 100);
                Rigidbody.velocity = dir * speed;
                photonView.RPC("SyncSpeed", RpcTarget.Others, speed);
                photonView.RPC("ChangeColor", RpcTarget.All, 50, 175, 222, 1);

                // 슬로우 시간 갱신 위한 코드
                // 더 강하거나 비슷한 슬로우가 들어오면 작동 준비중이던 슬로우 탈출 코루틴은 나가리 되고 새로운 탈출 코루틴이 돌아감
                if (exitSlowCoroutine != null && slowTime > 0) // 법사 패시브 때문에 slowTime > 0 조건 추가함
                {
                    StopCoroutine(exitSlowCoroutine);
                }
                if (slowTime > 0) exitSlowCoroutine = StartCoroutine(Co_ExitSlow(slowTime));
            }
        }
    }

    Coroutine exitSlowCoroutine = null;
    IEnumerator Co_ExitSlow(float slowTime)
    {
        yield return new WaitForSeconds(slowTime);
        photonView.RPC("ExitSlow", RpcTarget.All);
    }

    [PunRPC]
    public void ExitSlow()
    {
        ChangeMat(originMat);
        ChangeColor(255, 255, 255, 255);

        // 스턴 상태가 아니라면 속도 복구
        if (queue_GetSturn.Count <= 0 && photonView.IsMine) Set_OriginSpeed_ToAllPlayer();
    }

    [SerializeField] private Material freezeMat;
    [PunRPC] // 얼리는 스킬
    public void OnFreeze(float slowTime)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            speed = 0;
            Rigidbody.velocity = Vector3.zero;
            photonView.RPC("SyncSpeed", RpcTarget.Others, speed);

            if (exitSlowCoroutine != null) StopCoroutine(exitSlowCoroutine);
            exitSlowCoroutine = StartCoroutine(Co_ExitSlow(slowTime));

            photonView.RPC("OnFreeze", RpcTarget.Others, 0f); // if문 외의 코드 실행이 목적이므로 인자값은 의미없음
        }

        ChangeMat(freezeMat);
    }




    [PunRPC]
    public void OnStern(int sternPercent, float sternTime)
    {
        if (IsNonePassive || isDead || !PhotonNetwork.IsMasterClient) return;

        int random = UnityEngine.Random.Range(0, 100);
        if (random < sternPercent) StartCoroutine(SternCoroutine(sternTime));
    }

    Queue<int> queue_GetSturn = new Queue<int>();
    [SerializeField] private GameObject sternEffect;
    IEnumerator SternCoroutine(float sternTime)
    {
        queue_GetSturn.Enqueue(-1);
        speed = 0;
        Rigidbody.velocity = dir * speed;

        photonView.RPC("SyncSpeed", RpcTarget.Others, 0f);
        photonView.RPC("ShowSturnEffetc", RpcTarget.All);
        yield return new WaitForSeconds(sternTime);

        if (queue_GetSturn.Count != 0) queue_GetSturn.Dequeue();
        if (queue_GetSturn.Count == 0) photonView.RPC("ExitSturn", RpcTarget.All);
    }

    [PunRPC]
    public void ExitSturn()
    {
        sternEffect.SetActive(false);
        Set_OriginSpeed_ToAllPlayer();
    }

    [PunRPC]
    public void ShowSturnEffetc()
    {
        sternEffect.SetActive(true);
    }

    [PunRPC] // sync : 동기화한다는 뜻의 동사
    public void SyncSpeed(float _speed)
    {
        speed = _speed;
        Rigidbody.velocity = dir * _speed;
    }

    // 나중에 이동 tralslate로 바꿔서 스턴이랑 이속 다르게 처리하는거 시도해보기
    void Set_OriginSpeed_ToAllPlayer() => photonView.RPC("SyncSpeed", RpcTarget.All, maxSpeed);



    [PunRPC]
    public void OnPoison(int poisonPercent, int poisonCount, float poisonDelay, int maxDamage)
    {
        if (IsNonePassive || isDead || !PhotonNetwork.IsMasterClient) return;

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
            OnDamage(poisonDamage); // 포이즌 자체가 호스트에서만 돌아가기 때문에 그냥 써도 됨
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