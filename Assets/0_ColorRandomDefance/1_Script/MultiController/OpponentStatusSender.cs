using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
