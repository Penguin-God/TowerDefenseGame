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

    protected override void Init()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void Passive() { }

    [PunRPC]
    protected override void SetStatus(int _hp, float _speed, bool _isDead)
    {
        base.SetStatus(_hp, _speed, _isDead);
        Passive();
        TurnPoints = Multi_Data.instance.GetEnemyTurnPoints(gameObject);
        if(pointIndex == -1) pointIndex = 0;
        SetDirection();
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

    [ContextMenu("으앙 주금")]
    public override void Dead()
    {
        base.Dead();
        gameObject.SetActive(false);
        transform.position = new Vector3(500, 500, 500);
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        resurrection.Reset();
        sternEffect.SetActive(false);
        queue_GetSturn.Clear();
        ResetColor();
        pointIndex = -1;
        transform.rotation = Quaternion.identity;
    }

    public ResurrectionSystem resurrection = new ResurrectionSystem();

    public void Resurrection_RPC() => photonView.RPC(nameof(Resurrection), RpcTarget.All);
    [PunRPC] protected void Resurrection() => resurrection.Resurrection();

    public class ResurrectionSystem
    {
        bool isResurrection = false;
        public bool IsResurrection => isResurrection;
        public void Resurrection() => isResurrection = true;

        int spawnStage;
        public int SpawnStage => spawnStage;
        public void SetSpawnStage(int stage) => spawnStage = stage;

        public void Reset()
        {
            isResurrection = false;
            spawnStage = 0;
        }
    }

    // TODO : 상태이상 구현 코드 줄일 방법 찾아보기
    /// <summary>
    /// 상태이상 구현 표시
    /// </summary>
    #region 상태이상 구현

    Coroutine exitSlowCoroutine = null;
    [SerializeField] private Material freezeMat;

    private Queue<int> queue_GetSturn = new Queue<int>();
    [SerializeField] private GameObject sternEffect;

    [PunRPC]
    protected override void OnSlow(float slowPercent, float slowTime)
    {
        if (IsDead || PhotonNetwork.IsMasterClient == false) return;

        // 슬로우를 적용했을 때 현재 속도보다 느려져야만 슬로우 적용
        if (maxSpeed - maxSpeed * (slowPercent / 100) <= speed)
        {
            speed = maxSpeed - maxSpeed * (slowPercent / 100);
            Rigidbody.velocity = dir * speed;
            photonView.RPC(nameof(SyncSpeed), RpcTarget.Others, speed);
            photonView.RPC(nameof(ChangeColor), RpcTarget.All, 50, 175, 222, 1);

            // 슬로우 시간 갱신 위한 코드
            // 더 강하거나 비슷한 슬로우가 들어오면 작동 준비중이던 슬로우 탈출 코루틴은 나가리 되고 새로운 탈출 코루틴이 돌아감
            if (exitSlowCoroutine != null && slowTime > 0) // 법사 패시브 때문에 slowTime > 0 조건 추가함
            {
                StopCoroutine(exitSlowCoroutine);
            }
            if (slowTime > 0) exitSlowCoroutine = StartCoroutine(Co_ExitSlow(slowTime));
        }
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
        ChangeColor(255, 255, 255, 255);

        // 스턴 상태가 아니라면 속도 복구
        if (queue_GetSturn.Count <= 0 && photonView.IsMine) Set_OriginSpeed_ToAllPlayer();
    }


    [PunRPC]
    protected override void OnFreeze(float slowTime)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(SyncSpeed), RpcTarget.All, 0f);

            if (exitSlowCoroutine != null) StopCoroutine(exitSlowCoroutine);
            exitSlowCoroutine = StartCoroutine(Co_ExitSlow(slowTime));

            photonView.RPC(nameof(OnFreeze), RpcTarget.Others, 0f); // 마스터 클라 외의 플레이어 입장에서 if문 바깥의 ChangeMat 코드 실행이 목적이므로 인자값은 의미없음
        }

        ChangeMat(freezeMat);
    }

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
        Set_OriginSpeed_ToAllPlayer();
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

    void SyncSpeedToOther(float speed) => photonView.RPC(nameof(SyncSpeed), RpcTarget.Others, speed);

    // 나중에 이동 tralslate로 바꿔서 스턴이랑 이속 다르게 처리하는거 시도해보기
    protected void Set_OriginSpeed_ToAllPlayer() => photonView.RPC(nameof(SyncSpeed), RpcTarget.All, maxSpeed);

    #endregion
}
