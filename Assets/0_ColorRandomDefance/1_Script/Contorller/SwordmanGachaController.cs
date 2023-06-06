using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SwordmanGachaController : MonoBehaviourPun
{
    Multi_GameManager _game;
    UnitSummonData _unitSummonData;
    public void Init(Multi_GameManager game, UnitSummonData unitSummonData)
    {
        _game = game;
        _unitSummonData = unitSummonData;
    }

    [PunRPC] protected virtual void DrawUnit(byte id) { }

    public bool TryDrawUnit()
    {
        if (CanDrawUnit())
        {
            photonView.RPC(nameof(DrawUnit), RpcTarget.MasterClient, PlayerIdManager.Id);
            return true;
        }
        else
            return false;
    }

    protected bool CanDrawUnit() => _game.UnitOver == false && _game.HasGold(_unitSummonData.SummonPrice);

    public void ChangeUnitSummonData(int price, UnitColor maxColor)
    {
        _unitSummonData = new UnitSummonData(price, maxColor);
        photonView.RPC(nameof(ChangeUnitSummonData), RpcTarget.MasterClient, PlayerIdManager.Id, (byte)price, (byte)maxColor);
    }

    [PunRPC]
    public virtual void ChangeUnitSummonData(byte id, byte price, byte maxColor) { }
}

public class MasterSwordmanGachaController : SwordmanGachaController
{
    CurrencyManagerMediator _currencyManagerMediator;
    ServerManager _serverManager;
    MultiData<UnitSummonData> _multiUnitSummonData;

    public void Init(ServerManager serverManager, CurrencyManagerMediator currencyManagerMediator, UnitSummonData unitSummonData)
    {
        _multiUnitSummonData = new MultiData<UnitSummonData>(() => unitSummonData);
        _serverManager = serverManager;
        _currencyManagerMediator = currencyManagerMediator;
    }

    [PunRPC]
    protected override void DrawUnit(byte id)
    {
        if (_serverManager.GetUnitstData(id).UnitOver() == false && _currencyManagerMediator.TryUseGold(_multiUnitSummonData.GetData(id).SummonPrice, id))
            Multi_SpawnManagers.NormalUnit.RPCSpawn(new UnitFlags(SummonUnitColor(id), UnitClass.Swordman), id);
    }

    UnitColor SummonUnitColor(byte id) => _multiUnitSummonData.GetData(id).SelectColor();

    [PunRPC]
    public override void ChangeUnitSummonData(byte id, byte price, byte maxColor)
    {
        var newSummonData = new UnitSummonData(price, (UnitColor)maxColor);
        _multiUnitSummonData.SetData(id, newSummonData);
    }
}
