using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;

public interface IEventClear
{
    void Clear();
}

public static class EventIdManager
{
    static byte id = 0;
    static List<IEventClear> clears= new List<IEventClear>();
    public static byte UseID(IEventClear clear)
    {
        clears.Add(clear);
        id++;
        return id;
    }
    public static byte UseID() => id++;
    public static void Clear()
    {
        clears.ForEach(x => x.Clear());
        clears.Clear();
        id = 0;
    }
}

class RPCAciontBase
{
    public byte Constructor(Action<EventData> recevieEvent, IEventClear clear)
    {
        PhotonNetwork.NetworkingClient.EventReceived -= recevieEvent;
        PhotonNetwork.NetworkingClient.EventReceived += recevieEvent;
        return EventIdManager.UseID(clear);
    }
}

// 마스터한테 요청해야 하지만 개별로 적용되어야 하는 이벤트들
public class RPCAction : IEventClear
{
    byte _eventId;
    event Action OnEvent = null;

    public RPCAction() => _eventId = new RPCAciontBase().Constructor(RecevieEvent, this);
    public void Clear() => PhotonNetwork.NetworkingClient.EventReceived -= RecevieEvent;

    void RecevieEvent(EventData data)
    {
        if (data.Code != _eventId) return;
        OnEvent?.Invoke();
    }

    public void RaiseEvent(int id)
    {
        Debug.Assert(PhotonNetwork.IsMasterClient, "마스터가 아닌데 이벤트를 전달하려 함");

        if (Multi_Data.instance.CheckIdSame(id)) // 내가 맞으면 실행하고 아니면 전달
        {
            OnEvent?.Invoke();
            return;
        }
        RaiseEvent(ReceiverGroup.Others);
    }

    void RaiseEvent(ReceiverGroup receiverGroup) 
        => PhotonNetwork.RaiseEvent(_eventId, null, new RaiseEventOptions() { Receivers = receiverGroup }, SendOptions.SendReliable);
    public void RaiseEventToOther(int id) => RaiseEvent((id == 0) ? 1 : 0);

    public static RPCAction operator +(RPCAction me, Action action)
    {
        me.OnEvent += action;
        return me;
    }

    public static RPCAction operator -(RPCAction me, Action action)
    {
        me.OnEvent -= action;
        return me;
    }
}

public class RPCAction<T> : IEventClear
{
    byte _eventId;
    event Action<T> OnEvent = null;

    public RPCAction() => _eventId = new RPCAciontBase().Constructor(RecevieEvent, this);
    public void Clear() => PhotonNetwork.NetworkingClient.EventReceived -= RecevieEvent;

    void RecevieEvent(EventData data)
    {
        if (data.Code != _eventId) return;

        T value = (T)((object[])data.CustomData)[0];
        OnEvent?.Invoke(value);
    }

    public void RaiseEvent(int id, T value)
    {
        Debug.Assert(PhotonNetwork.IsMasterClient, "마스터가 아닌데 이벤트를 전달하려 함");

        if (Multi_Data.instance.CheckIdSame(id)) // 내가 맞으면 실행하고 아니면 전달
        {
            OnEvent?.Invoke(value);
            return;
        }
        RaiseEvent(value, ReceiverGroup.Others);
    }

    public void RaiseAll(T value) => RaiseEvent(value, ReceiverGroup.All);
    void RaiseEvent(T value, ReceiverGroup receiverGroup) 
        => PhotonNetwork.RaiseEvent(_eventId, new object[] { value }, new RaiseEventOptions() { Receivers = receiverGroup }, SendOptions.SendReliable);
    public void RaiseEventToOther(int id, T value) => RaiseEvent((id == 0) ? 1 : 0, value);

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

public class RPCAction<T, T2> : IEventClear
{
    byte _eventId;
    event Action<T, T2> OnEvent = null;

    public RPCAction() => _eventId = new RPCAciontBase().Constructor(RecevieEvent, this);
    public void Clear() => PhotonNetwork.NetworkingClient.EventReceived -= RecevieEvent;


    void RecevieEvent(EventData data)
    {
        if (data.Code != _eventId) return;

        T value = (T)((object[])data.CustomData)[0];
        T2 value2 = (T2)((object[])data.CustomData)[1];
        OnEvent?.Invoke(value, value2);
    }

    public void RaiseEvent(int id, T value, T2 value2)
    {
        Debug.Assert(PhotonNetwork.IsMasterClient, "마스터가 아닌데 이벤트를 전달하려 함");

        if (Multi_Data.instance.CheckIdSame(id)) // 내가 맞으면 실행하고 아니면 전달
        {
            OnEvent?.Invoke(value, value2);
            return;
        }
        RaiseEvent(value, value2, ReceiverGroup.Others);
    }

    public void RaiseEventToOther(int id, T value, T2 value2) => RaiseEvent((id == 0) ? 1 : 0, value, value2);
    public void RaiseAll(T value, T2 value2) => RaiseEvent(value, value2, ReceiverGroup.All);
    void RaiseEvent(T value, T2 value2, ReceiverGroup receiverGroup)
        => PhotonNetwork.RaiseEvent(_eventId, new object[] { value, value2 }, new RaiseEventOptions() { Receivers = receiverGroup }, SendOptions.SendReliable);

    public static RPCAction<T, T2> operator +(RPCAction<T, T2> me, Action<T, T2> action)
    {
        me.OnEvent += action;
        return me;
    }

    public static RPCAction<T, T2> operator -(RPCAction<T, T2> me, Action<T, T2> action)
    {
        me.OnEvent -= action;
        return me;
    }
}


public class RPCAction<T, T2, T3> : IEventClear
{
    byte _eventId;
    event Action<T, T2, T3> OnEvent = null;

    public RPCAction() => _eventId = new RPCAciontBase().Constructor(RecevieEvent, this);
    public void Clear() => PhotonNetwork.NetworkingClient.EventReceived -= RecevieEvent;


    void RecevieEvent(EventData data)
    {
        if (data.Code != _eventId) return;

        T value = (T)((object[])data.CustomData)[0];
        T2 value2 = (T2)((object[])data.CustomData)[1];
        T3 value3 = (T3)((object[])data.CustomData)[2];
        OnEvent?.Invoke(value, value2, value3);
    }

    public void RaiseEvent(int id, T value, T2 value2, T3 value3)
    {
        Debug.Assert(PhotonNetwork.IsMasterClient, "마스터가 아닌데 이벤트를 전달하려 함");

        if (Multi_Data.instance.CheckIdSame(id)) // 내가 맞으면 실행하고 아니면 전달
        {
            OnEvent?.Invoke(value, value2, value3);
            return;
        }
        RaiseEvent(value, value2, value3, ReceiverGroup.Others);
    }

    public void RaiseEventToOther(int id, T value, T2 value2, T3 value3) => RaiseEvent((id == 0) ? 1 : 0, value, value2, value3);
    public void RaiseAll(T value, T2 value2, T3 value3) => RaiseEvent(value, value2, value3, ReceiverGroup.All);
    void RaiseEvent(T value, T2 value2, T3 value3, ReceiverGroup receiverGroup)
        => PhotonNetwork.RaiseEvent(_eventId, new object[] { value, value2, value3 }, new RaiseEventOptions() { Receivers = receiverGroup }, SendOptions.SendReliable);

    public static RPCAction<T, T2, T3> operator +(RPCAction<T, T2, T3> me, Action<T, T2, T3> action)
    {
        me.OnEvent += action;
        return me;
    }

    public static RPCAction<T, T2, T3> operator -(RPCAction<T, T2, T3> me, Action<T, T2, T3> action)
    {
        me.OnEvent -= action;
        return me;
    }
}