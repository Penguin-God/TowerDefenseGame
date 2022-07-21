using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
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

public class RPCAction<T>
{
    static byte currentEventID = 0;
    byte _eventId;

    event Action<T> OnEvent = null;

    public RPCAction()
    {
        _eventId = currentEventID++;
        PhotonNetwork.NetworkingClient.EventReceived += RecevieEvent;
    }

    void RecevieEvent(EventData data)
    {
        if (data.Code != _eventId) return;
        T value = (T)((object[])data.CustomData)[0];
        OnEvent?.Invoke(value);
    }

    public void RaiseEvent(int id, T value) // 내가 맞으면 실행하고 아니면 전달
    {
        if (Multi_Data.instance.CheckIdSame(id))
        {
            PhotonNetwork.RaiseEvent(_eventId, new object[] { value }, new RaiseEventOptions(){ Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
            OnEvent?.Invoke(value);
            return;
        }
        //PhotonNetwork.RaiseEvent(_eventId, new object[] { value }, new RaiseEventOptions() { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
    }

    public static RPCAction<T> operator +(RPCAction<T> me, Action<T> action)
    {
        me.OnEvent += action;
        return me;
    }

    public static RPCAction<T> operator -(RPCAction<T> me, Action<T> action)
    {
        me.OnEvent -= action;
        return me;
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
