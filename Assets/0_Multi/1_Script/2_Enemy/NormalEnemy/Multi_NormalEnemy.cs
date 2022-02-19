using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Multi_NormalEnemy : Multi_Enemy, IPunObservable
{

    [SerializeField] int enemyNumber = 0;
    public int GetEnemyNumber { get { return enemyNumber; } }

    // 이동, 회전 관련 변수
    public Transform[] TurnPoints { get; set; } = null;
    private Transform WayPoint => TurnPoints[pointIndex];
    private int pointIndex = -1;

    private void Awake()
    {
        enemyType = EnemyType.Normal;
        Rigidbody = GetComponent<Rigidbody>();
    }


    void OnEnable() // 리스폰 시 상태 초기화
    {
        if (TurnPoints != null && photonView.IsMine)
        {
            photonView.RPC("OnEnemy", RpcTarget.All); 
            ChaseToPoint();
        }
    }

    [PunRPC]
    public void OnEnemy()
    {
        pointIndex = 0;
        isDead = false;
    }

    public virtual void Passive() { }


    [PunRPC]
    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }

    [PunRPC]
    public override void Setup(int _hp, float _speed)
    {
        maxHp = _hp;
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        this.maxSpeed = _speed;
        this.speed = maxSpeed;
        gameObject.SetActive(true);
        currentPos = transform.position;
    }

    [PunRPC]
    public void Turn(int _pointIndex, Vector3 _wayPos)
    {
        transform.position = _wayPos;
        transform.rotation = Quaternion.Euler(0, -90 * _pointIndex, 0);
        pointIndex++;
    }

    public void ChaseToPoint()
    {
        if (pointIndex >= TurnPoints.Length) pointIndex = 0; // 무한반복을 위한 조건

        // 실제 이동을 위한 속도 설정
        dir = (WayPoint.position - transform.position).normalized;
        photonView.RPC("SetVelocity", RpcTarget.All, dir, speed);
    }

    // 빠르게 자살하는 방법 : [PunRPC] 함수 private로 구현하기
    [PunRPC]
    public void SetVelocity(Vector3 _dir, float _speed)
    {
        Rigidbody.velocity = _dir * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WayPoint" && photonView.IsMine)
        {
            int _pointIndex = pointIndex;
            Vector3 _wayPoint = WayPoint.position;
            photonView.RPC("Turn", RpcTarget.All, _pointIndex, _wayPoint);

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
    }
}
