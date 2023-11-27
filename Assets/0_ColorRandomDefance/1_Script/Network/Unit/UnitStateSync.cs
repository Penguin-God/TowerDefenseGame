using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitStateSync : MonoBehaviourPun, IPunObservable
{
    void Awake()
    {
        photonView.ObservedComponents.Add(this);
        _unitChaseSystem = GetComponent<UnitChaseSystem>();
        _unit = GetComponent<Multi_TeamSoldier>();
        _agent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        _masterRotationY = transform.eulerAngles.y;
        _masterPos = transform.position;
    }

    UnitChaseSystem _unitChaseSystem;
    Multi_TeamSoldier _unit;
    NavMeshAgent _agent;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((byte)_unitChaseSystem.ChaseState);
            stream.SendNext(transform.eulerAngles.y);
            stream.SendNext(transform.position.x);
            stream.SendNext(transform.position.z);
            stream.SendNext(_unit.TargetEnemy == null ? 0 : _unit.TargetEnemy.GetComponent<PhotonView>().ViewID);
        }
        else
        {
            _unitChaseSystem.ChangeState((ChaseState)(byte)stream.ReceiveNext());
            _masterRotationY = (float)stream.ReceiveNext();
            float x = (float)stream.ReceiveNext();
            float z = (float)stream.ReceiveNext();
            _masterPos = new Vector3(x, 0, z);
            _targetId = (int)stream.ReceiveNext();
        }
    }

    const float RotationLerpSpeed = 0.8f;
    const float PositionLerpSpeed = 0.4f;
    const float AngleDelta = 5f;
    const float PositionDelta = 5f;
    float _masterRotationY;
    Vector3 _masterPos;
    int _targetId = -1;
    void Update()
    {
        if (PhotonNetwork.IsMasterClient) return;

        // 마스터 클라이언트로부터 받은 회전값 로컬 회전값을 비교 및 보간
        float currentRotationY = transform.eulerAngles.y;
        if (Mathf.Abs(_masterRotationY - currentRotationY) > AngleDelta)
        {
            // print("방향 보간");
            float newY = Mathf.LerpAngle(currentRotationY, _masterRotationY, Time.deltaTime * RotationLerpSpeed);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newY, transform.eulerAngles.z);
        }

        if (Vector3.Distance(transform.position, _masterPos) > PositionDelta)
            transform.position = Vector3.Lerp(transform.position, _masterPos, Time.deltaTime * PositionLerpSpeed);

        //if (Vector3.Distance(transform.position, _masterPos) > PositionDelta)
        //    print("위치 보간");

        // 이거 사용하려면 유닛에서 isStopped 건드리는 부분 고려해야 함.
        //if (Vector3.Distance(transform.position, _masterPos) > PositionDelta)
        //    _agent.isStopped = true;
        //else
        //    _agent.isStopped = false;

        if (_targetId > 0) _unit.ChangeTarget(_targetId);
        else _unit.SetNull();
    }
}
