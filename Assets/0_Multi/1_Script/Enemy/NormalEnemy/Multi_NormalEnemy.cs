using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Multi_NormalEnemy : Multi_Enemy, IPunObservable
{
    // 이동, 회전 관련 변수
    private Transform wayPoint;
    private int pointIndex = -1;

    private void Awake()
    {
        nomalEnemy = GetComponent<Multi_NormalEnemy>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    void OnEnable() // 리스폰 시 상태 초기화
    {
        if (TurnPoints == null || !PhotonNetwork.IsMasterClient)
        {
            return;
        }

        pointIndex = 0;
        isDead = false;
        ChaseToPoint();
        //Passive();
    }

    [PunRPC]
    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }

    [PunRPC]
    public void SetStatus(int hp, float speed)
    {
        maxHp = hp;
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        this.maxSpeed = speed;
        this.speed = maxSpeed;
        gameObject.SetActive(true);
        currentPos = transform.position;
    }

    // 빠르게 자살하는 방법 : [PunRPC] 함수 private로 구현하기
    [PunRPC]
    public void SetVelocity(Vector3 _dir, float _speed)
    {
        Rigidbody.velocity = _dir * _speed;
    }

    [SerializeField] Transform[] asuwasfweuaki = null;
    public Transform[] TurnPoints { get; set; } = null;
    void ChaseToPoint()
    {
        if (pointIndex >= TurnPoints.Length) pointIndex = 0; // 무한반복을 위한 조건

        // 실제 이동을 위한 속도 설정
        wayPoint = TurnPoints[pointIndex];
        dir = (wayPoint.position - transform.position).normalized;
        photonView.RPC("SetVelocity", RpcTarget.All, dir, speed);
    }

    [PunRPC]
    public void SetTransfrom(Vector3 _pos, int _pointIndex)
    {
        transform.position = _pos;
        transform.rotation = Quaternion.Euler(0, -90 * _pointIndex, 0);
    }

    [SerializeField] int enemyNumber = 0;
    public int GetEnemyNumber { get { return enemyNumber; } }

    public override void Dead()
    {
        base.Dead();

        gameObject.SetActive(false);
        transform.position = new Vector3(500, 500, 500);
        ResetVariable();
    }

    void ResetVariable()
    {
        pointIndex = -1;
        transform.rotation = Quaternion.identity;
        ChangeMat(originMat);
        ChangeColor(new Color32(255, 255, 255, 255));
        sternEffect.SetActive(false);
    }

    public virtual void Passive() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WayPoint" && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetTransfrom", RpcTarget.All, wayPoint.position, pointIndex);

            pointIndex++;
            ChaseToPoint();
        }
    }

    Vector3 currentPos;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            currentPos = (Vector3)stream.ReceiveNext();
            if ((transform.position - currentPos).sqrMagnitude <= 100) transform.position = currentPos;
            else transform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime * 10);
        }
    }
}
