using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnerController : MonoBehaviourPun
{
    protected Multi_GameManager _game;
    public void Init(Multi_GameManager game)
    {
        _game = game;
    }

    [PunRPC]
    protected virtual void DrawUnit(byte id) { }

    public bool TryDrawUnit()
    {
        if (CanDrawUnit() == false) return false;
        if(_game.TryUseGold(5))
            photonView.RPC(nameof(DrawUnit), RpcTarget.MasterClient, PlayerIdManager.Id);
        return true;
    }

    protected bool CanDrawUnit() => _game.UnitOver == false && _game.HasGold(5);
}


public class ServerSpawnerController : SpawnerController
{
    [PunRPC]
    protected override void DrawUnit(byte id)
    {
        Multi_SpawnManagers.NormalUnit.RPCSpawn(new UnitFlags(SummonUnitColor(), UnitClass.Swordman), id);
    }

    UnitColor SummonUnitColor() => (UnitColor)Random.Range(0, (int)(_game.BattleData.UnitSummonData.SummonMaxColor + 1));
}
