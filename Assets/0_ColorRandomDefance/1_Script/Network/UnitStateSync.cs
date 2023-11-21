using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateSync : MonoBehaviourPun, IPunObservable
{
    void Awake()
    {
        photonView.ObservedComponents.Add(this);
        _unitChaseSystem = GetComponent<UnitChaseSystem>();
    }

    void OnEnable()
    {
        _masterRotationY = transform.eulerAngles.y;
        _masterPos = transform.position;
    }

    UnitChaseSystem _unitChaseSystem;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((byte)_unitChaseSystem.ChaseState);
            stream.SendNext(transform.eulerAngles.y);
            stream.SendNext(transform.position.x);
            stream.SendNext(transform.position.z);
        }
        else
        {
            _unitChaseSystem.ChangeState((ChaseState)(byte)stream.ReceiveNext());
            _masterRotationY = (float)stream.ReceiveNext();
            float x = (float)stream.ReceiveNext();
            float z = (float)stream.ReceiveNext();
            _masterPos = new Vector3(x, 0, z);
        }
    }

    const float RotationLerpSpeed = 5.0f;
    const float PositionLerpSpeed = 10f;
    const float Delta = 10f;
    float _masterRotationY;
    Vector3 _masterPos;
    void Update()
    {
        if (PhotonNetwork.IsMasterClient) return;

        // 마스터 클라이언트로부터 받은 회전값 로컬 회전값을 비교 및 보간
        float currentRotationY = transform.eulerAngles.y;
        if (Mathf.Abs(_masterRotationY - currentRotationY) > Delta)
        {
            float newY = Mathf.LerpAngle(currentRotationY, _masterRotationY, Time.deltaTime * RotationLerpSpeed);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newY, transform.eulerAngles.z);
        }

        if (Vector3.Distance(transform.position, _masterPos) > 1)
            transform.position = Vector3.Lerp(transform.position, _masterPos, Time.deltaTime * PositionLerpSpeed);
    }
}
