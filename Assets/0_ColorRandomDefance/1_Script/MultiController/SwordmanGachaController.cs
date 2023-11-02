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

    public void ChangeUnitSummonMaxColor(UnitColor maxColor)
    {
        _unitSummonData.SummonMaxColor = maxColor;
        photonView.RPC(nameof(ChangeUnitSummonMaxColor), RpcTarget.MasterClient, PlayerIdManager.Id, (byte)maxColor);
    }

    [PunRPC]
    public virtual void ChangeUnitSummonMaxColor(byte id, byte maxColor) { }
}

public class MasterSwordmanGachaController : SwordmanGachaController
{
    CurrencyManagerMediator _currencyManagerMediator;
    MultiBattleDataController _multiBattleDataController { get; set; }
    WorldUnitManager _worldUnitManager;
    MultiData<UnitSummonData> _multiUnitSummonData;
    Multi_NormalUnitSpawner _unitSpawner;

    public void Init(MultiBattleDataController multiBattleDataController, WorldUnitManager worldUnitManager, CurrencyManagerMediator currencyManagerMediator, UnitSummonData unitSummonData, Multi_NormalUnitSpawner unitSpawner)
    {
        _multiUnitSummonData = new MultiData<UnitSummonData>(() => unitSummonData);
        _worldUnitManager = worldUnitManager;
        _multiBattleDataController = multiBattleDataController;
        _currencyManagerMediator = currencyManagerMediator;
        _unitSpawner = unitSpawner;
    }

    [PunRPC]
    protected override void DrawUnit(byte id)
    {
        if (_multiBattleDataController.GetData(id).MaxUnitCount > _worldUnitManager.GetUnitCount(id) && _currencyManagerMediator.TryUseGold(SummonPrice(id), id))
            _unitSpawner.RPCSpawn(new UnitFlags(SummonUnitColor(id), UnitClass.Swordman), id);
    }

    UnitColor SummonUnitColor(byte id) => _multiUnitSummonData.GetData(id).SelectColor();

    int SummonPrice(byte id) => _multiUnitSummonData.GetData(id).SummonPrice;

    [PunRPC]
    public override void ChangeUnitSummonMaxColor(byte id, byte maxColor)
    {
        var newSummonData = new UnitSummonData(SummonPrice(id), (UnitColor)maxColor);
        _multiUnitSummonData.SetData(id, newSummonData);
    }
}