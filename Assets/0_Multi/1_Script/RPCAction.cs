using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

// 마스터한테 요청해야 하지만 개별로 적용되어야 하는 이벤트들
public class RPCAction
{
    PhotonView _pv;
    public RPCAction(PhotonView pv)
    {
        _pv = pv;
    }

    Action action;

    public void Invoke_RPC(int id) => _pv?.RPC("Invoke", RpcTarget.All, id);

    [PunRPC]
    void Invoke(int id)
    {
        if (Multi_Data.instance.CheckIdSame(id) == false) return;
        action?.Invoke();
    }
}

public class RPCActionInt
{
    PhotonView _pv;
    public RPCActionInt(PhotonView pv)
    {
        _pv = pv;
    }

    Action<int> action;

    public void Invoke_RPC(int id, int value) => _pv?.RPC("Invoke", RpcTarget.All, id, value);

    [PunRPC]
    void Invoke(int id, int value)
    {
        if (Multi_Data.instance.CheckIdSame(id) == false) return;
        action?.Invoke(value);
    }
}

public class RPCActionUnitInt
{

}
