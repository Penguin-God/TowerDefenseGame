using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class SpawnerController : MonoBehaviourPun
{
    protected UnitSummoner _unitSummoner;
    protected Multi_GameManager _game;
    public void Init(Multi_GameManager game)
    {
        _unitSummoner = new UnitSummoner(game);
        _game = game;
    }

    [PunRPC]
    public bool TryDrawUnit(byte id)
    {
        if (_game.UnitOver == false)
            return false;

        DrawUnit(id);
        return true;
    }

    protected abstract void DrawUnit(byte id);
}

public class ClientSpawnerController : SpawnerController
{
    protected override void DrawUnit(byte id)
    {
        _game.TryUseGold(_game.BattleData.UnitSummonData.SummonPrice);
        photonView.RPC(nameof(TryDrawUnit), RpcTarget.MasterClient, id);
    }
}


public class ServerSpawnerController : SpawnerController
{
    protected override void DrawUnit(byte id)
    {
        if(id == PlayerIdManager.MasterId)
            _game.TryUseGold(_game.BattleData.UnitSummonData.SummonPrice);
        Multi_SpawnManagers.NormalUnit.Spawn(_unitSummoner.SummonUnitColor(), UnitClass.Swordman, id);
    }
}
