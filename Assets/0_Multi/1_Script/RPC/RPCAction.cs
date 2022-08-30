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

// 마스터한테 요청해야 하지만 개별로 적용되어야 하는 이벤트들

public class RPCAction<T> : IEventClear
{
    byte _eventId;
    event Action<T> OnEvent = null;

    public RPCAction()
    {
        _eventId = EventIdManager.UseID(this);
        PhotonNetwork.NetworkingClient.EventReceived -= RecevieEvent;
        PhotonNetwork.NetworkingClient.EventReceived += RecevieEvent;
    }

    public void Clear()
    {
        Debug.Log("wwwwwwwwwwwwwwww");
        PhotonNetwork.NetworkingClient.EventReceived -= RecevieEvent;
    }

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
        PhotonNetwork.RaiseEvent(_eventId, new object[] { value }, new RaiseEventOptions() { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
    }

    public void RaiseEvent(T value) => PhotonNetwork.RaiseEvent(_eventId, new object[] { value }, new RaiseEventOptions() { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);

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

    public RPCAction()
    {
        _eventId = EventIdManager.UseID(this);
        PhotonNetwork.NetworkingClient.EventReceived -= RecevieEvent;
        PhotonNetwork.NetworkingClient.EventReceived += RecevieEvent;
    }

    public void Clear()
    {
        Debug.Log("wwwwwwwwwwwwwwww");
        PhotonNetwork.NetworkingClient.EventReceived -= RecevieEvent;
    }

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
        PhotonNetwork.RaiseEvent
            (_eventId, new object[] { value, value2 }, new RaiseEventOptions() { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
    }

    public void RaiseEvent(T value, T2 value2) 
        => PhotonNetwork.RaiseEvent(_eventId, new object[] { value, value2 }, new RaiseEventOptions() { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);

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