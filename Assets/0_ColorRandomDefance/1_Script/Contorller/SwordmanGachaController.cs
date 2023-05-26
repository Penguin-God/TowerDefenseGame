using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SwordmanGachaController : MonoBehaviourPun
{
    protected Multi_GameManager _game;
    public void Init(Multi_GameManager game)
    {
        _game = game;
    }

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
        if(_game.TryUseGold(DrawGold))
            photonView.RPC(nameof(DrawUnit), RpcTarget.MasterClient, PlayerIdManager.Id);
        return true;
    }

    protected bool CanDrawUnit() => _game.UnitOver == false && _game.HasGold(DrawGold);
}


public class MasterSwordmanGachaController : SwordmanGachaController
{
    [PunRPC]
    protected override void DrawUnit(byte id)
    {
        var countData = MultiServiceMidiator.Server.GetBattleData(id);
        if (countData.MaxUnitCount > countData.CurrentUnitCount && GetMasterCurrencyManager().TryUseGold(DrawGold, id))
            Multi_SpawnManagers.NormalUnit.RPCSpawn(new UnitFlags(SummonUnitColor(), UnitClass.Swordman), id);
    }

    MasterCurrencyManager GetMasterCurrencyManager()
    {
        var result = _currencyManager as MasterCurrencyManager;
        if (result == null)
            print("이게 왜 null일까?");
        return result;
    }
    UnitColor SummonUnitColor() => (UnitColor)Random.Range(0, (int)(_game.BattleData.UnitSummonData.SummonMaxColor + 1));
}
