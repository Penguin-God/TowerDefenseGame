using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UnitMaxCountController : MonoBehaviourPun
{
    ServerManager _server;
    Multi_GameManager _game;

    public void Init(ServerManager server, Multi_GameManager game)
    {
        _server = server;
        _game = game;
    }

    public void IncreasedMaxUnitCount(int amount)
    {
        photonView.RPC(nameof(IncreasedMaxUnitCount), RpcTarget.MasterClient, (byte)amount, PlayerIdManager.Id);
    }

    [PunRPC]
    void IncreasedMaxUnitCount(byte amout, byte id)
    {
        _server.GetUnitstData(id).MaxUnitCount += amout;
        if (id == PlayerIdManager.MasterId)
            OverrideMaxCount((byte)_server.GetUnitstData(id).MaxUnitCount);
        else
            photonView.RPC(nameof(OverrideMaxCount), RpcTarget.Others, (byte)_server.GetUnitstData(id).MaxUnitCount);
    }

    [PunRPC] void OverrideMaxCount(byte count) => _game.BattleData.MaxUnit = count;
}
