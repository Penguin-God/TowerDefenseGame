using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OpponentStatusSynchronizer
{
    public RPCAction<byte, UnitClass, byte> OnOtherUnitCountChanged = new RPCAction<byte, UnitClass, byte>();
    public RPCAction<byte> OnOtherUnitMaxCountChanged = new RPCAction<byte>();

    public OpponentStatusSynchronizer()
    {
        Managers.Unit.OnUnitCountChangeByClass += SendUnitCountDataToOpponent;
        Multi_GameManager.Instance.BattleData.OnMaxUnitChanged += SendUnitMaxCountDataToOpponent;
    }

    void SendUnitCountDataToOpponent(UnitClass unitClass, int count)
        => OnOtherUnitCountChanged?.RaiseToOther((byte)Managers.Unit.CurrentUnitCount, unitClass, (byte)count);
    void SendUnitMaxCountDataToOpponent(int count) => OnOtherUnitMaxCountChanged?.RaiseToOther((byte)count);
}

public class OpponentStatusSender : MonoBehaviourPun
{
    BattleEventDispatcher _dispatcher;
    public void Init(BattleEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        Managers.Unit.OnUnitCountChangeByClass += SendUnitCountDataToOpponent;
        Multi_GameManager.Instance.BattleData.OnMaxUnitChanged += SendUnitMaxCountDataToOpponent;
    }

    void SendUnitCountDataToOpponent(UnitClass unitClass, int classCount)
        => photonView.RPC(nameof(OnOpponentUnitCountChange), RpcTarget.Others, (byte)Managers.Unit.CurrentUnitCount, (byte)unitClass, (byte)classCount);
    void SendUnitMaxCountDataToOpponent(int count) => photonView.RPC(nameof(OnOpponentUnitMaxCountChange), RpcTarget.Others, (byte)count);

    [PunRPC]
    void OnOpponentUnitCountChange(byte count, byte unitClass, byte classCount) => _dispatcher.NotifyOpponentUnitCountChanged(count, (UnitClass)unitClass, classCount);
    [PunRPC]
    void OnOpponentUnitMaxCountChange(byte count) => _dispatcher.NotifyOpponentUnitMaxCountChanged(count);
}
