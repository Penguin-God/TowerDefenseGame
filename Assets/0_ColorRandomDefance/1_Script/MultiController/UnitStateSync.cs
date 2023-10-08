using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateSync : MonoBehaviourPun, IPunObservable
{
    void Awake()
    {
        photonView.ObservedComponents.Add(this);
        _unitChaseSystem = gameObject.AddComponent<UnitChaseSystem>();
    }

    UnitChaseSystem _unitChaseSystem;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext((byte)_unitChaseSystem.ChaseState);
        else
            _unitChaseSystem.ChangeState((ChaseState)(byte)stream.ReceiveNext());
    }
}
