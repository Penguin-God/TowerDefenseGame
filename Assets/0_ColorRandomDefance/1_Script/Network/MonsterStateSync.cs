using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MonsterStateSync : MonoBehaviourPun, IPunObservable
{
    Multi_NormalEnemy _monster;
    float _saveSpeed;
    void Awake()
    {
        photonView.ObservedComponents.Add(this);
        _monster = GetComponent<Multi_NormalEnemy>();
        _saveSpeed = _monster.Speed;
    }

    bool SpeedIsSame() => Mathf.Abs(_monster.Speed - _saveSpeed) <= 0.01f;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (_monster.IsDead || SpeedIsSame()) return;

        if (stream.IsWriting)
        {
            _saveSpeed = _monster.Speed;
            stream.SendNext(_saveSpeed);
        }
        else
        {
            print("속도 받음");
            _monster.ChangeVelocity((float)stream.ReceiveNext());
        }
    }
}
