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
public abstract class RPCActionBase : IEventClear
{
    byte Constructor(Action<EventData> recevieEvent, IEventClear clear)
    {
        PhotonNetwork.NetworkingClient.EventReceived -= recevieEvent;
        PhotonNetwork.NetworkingClient.EventReceived += recevieEvent;
        return EventIdManager.UseID(clear);
    }

    byte _eventId;

    // 자식 생성자 호출 시 자동 실행
    public RPCActionBase() => _eventId = Constructor(RecevieEvent, this);
    public void Clear() => PhotonNetwork.NetworkingClient.EventReceived -= RecevieEvent;

    protected void RecevieEvent(EventData data)
    {
        if (data.Code != _eventId) return;
        RecevieEvent((object[])data.CustomData);
    }

    protected abstract void RecevieEvent(object[] values);

    protected void RaiseEvent(int id, params object[] values)
    {
        Debug.Assert(PhotonNetwork.IsMasterClient, "마스터가 아닌데 이벤트를 전달하려 함");

        if (Multi_Data.instance.CheckIdSame(id)) // 마스터가 맞으면 실행하고 아니면 전달
            RecevieEvent(values);
        else
            RaiseEvent(values, ReceiverGroup.Others);
    }

    void RaiseEvent(object[] values, ReceiverGroup receiverGroup)
    {
        if (PhotonNetwork.OfflineMode) // 오프라인 모드 전용
            RecevieEvent(values);
        PhotonNetwork.RaiseEvent(_eventId, values, new RaiseEventOptions() { Receivers = receiverGroup }, SendOptions.SendReliable);
    }
    protected void RaiseAll(params object[] values) => RaiseEvent(values, ReceiverGroup.All);
}

public class RPCAction : RPCActionBase
{
    event Action OnEvent = null;
    public void RaiseEvent(int id) => base.RaiseEvent(id);
    protected override void RecevieEvent(object[] values) => OnEvent?.Invoke();
    
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

public class RPCAction<T> : RPCActionBase
{
    event Action<T> OnEvent = null;
    public void RaiseEvent(int id, T value) => base.RaiseEvent(id, value);
    protected override void RecevieEvent(object[] values) => OnEvent?.Invoke((T)values[0]);

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

public class RPCAction<T, T2> : RPCActionBase
{
    event Action<T, T2> OnEvent = null;

    protected override void RecevieEvent(object[] values)
    {
        T value = (T)values[0];
        T2 value2 = (T2)values[1];
        OnEvent?.Invoke(value, value2);
    }
    public void RaiseAll(T value, T2 value2) => base.RaiseAll(value, value2);

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


public class RPCAction<T, T2, T3> : RPCActionBase
{
    event Action<T, T2, T3> OnEvent = null;
    protected override void RecevieEvent(object[] values)
    {
        T value = (T)values[0];
        T2 value2 = (T2)values[1];
        T3 value3 = (T3)values[2];
        OnEvent?.Invoke(value, value2, value3);
    }
    public void RaiseAll(T value, T2 value2, T3 value3) => base.RaiseAll(value, value2, value3);

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