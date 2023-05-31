using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SwordmanGachaController : MonoBehaviourPun
{
    protected Multi_GameManager _game;
    
    protected IBattleCurrencyManager _currencyManager;
    protected readonly int DrawGold = 5;
    public void Init(Multi_GameManager game, IBattleCurrencyManager currencyManager)
    {
        _game = game;
        _currencyManager = currencyManager;
    }

    [PunRPC]
    protected virtual void DrawUnit(byte id) { }

    public bool TryDrawUnit()
    {
        if (CanDrawUnit() == false) return false;
        photonView.RPC(nameof(DrawUnit), RpcTarget.MasterClient, PlayerIdManager.Id);
        return true;
    }

    protected bool CanDrawUnit() => _game.UnitOver == false && _game.HasGold(DrawGold);
}

public class MasterSwordmanGachaController : SwordmanGachaController
{
    CurrencyManagerMediator _currencyManagerMediator;
    ServerManager _serverManager;
    public void Init(ServerManager serverManager, CurrencyManagerMediator currencyManagerMediator)
    {
        _serverManager = serverManager;
        _currencyManagerMediator = currencyManagerMediator;
    }

    [PunRPC]
    protected override void DrawUnit(byte id)
    {
        if (_serverManager.GetUnitstData(id).UnitOver() == false && _currencyManagerMediator.TryUseGold(DrawGold, id))
            Multi_SpawnManagers.NormalUnit.RPCSpawn(new UnitFlags(SummonUnitColor(), UnitClass.Swordman), id);
    }

    UnitColor SummonUnitColor() => (UnitColor)Random.Range(0, (int)(_game.BattleData.UnitSummonData.SummonMaxColor + 1));
}
