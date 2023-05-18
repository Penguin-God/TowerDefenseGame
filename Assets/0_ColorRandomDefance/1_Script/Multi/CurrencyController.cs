using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CurrencyControllerProxy : IBattleCurrencyController
{
    CurrencyManager _currencyManager;
    MultiCurrencyController _multiCurrencyController;
    public CurrencyControllerProxy(CurrencyManager currencyManager, MultiCurrencyController multiCurrencyController)
    {
        _currencyManager = currencyManager;
        _multiCurrencyController = multiCurrencyController;
    }

    public int CurrentGold => _currencyManager.CurrentGold;
    public void AddGold(int amount)
    {
        _currencyManager.AddGold(amount);
        _multiCurrencyController.AddGold(amount);
    }
    public void UseGold(int amount)
    {
        _currencyManager.UseGold(amount);
        _multiCurrencyController.UseGold(amount);
    }


    public int CurrentFood => _currencyManager.CurrentFood;
    public void UseFood(int amount)
    {
        _currencyManager.UseFood(amount);
        _multiCurrencyController.UseFood(amount);
    }

    public void AddFood(int amount)
    {
        _currencyManager.AddFood(amount);
        _multiCurrencyController.AddFood(amount);
    }
}

public class MultiCurrencyController : MonoBehaviourPun
{
    ServerManager _server;
    public void SetInfo(ServerManager server) => _server = server;

    public void AddGold(int amount) => photonView.RPC(nameof(AddGold), RpcTarget.MasterClient, amount, PlayerIdManager.Id);
    public void UseGold(int amount) => photonView.RPC(nameof(UseGold), RpcTarget.MasterClient, amount, PlayerIdManager.Id);
    public void AddFood(int amount) => photonView.RPC(nameof(AddFood), RpcTarget.MasterClient, amount, PlayerIdManager.Id);
    public void UseFood(int amount) => photonView.RPC(nameof(UseFood), RpcTarget.MasterClient, amount, PlayerIdManager.Id);

    public void AddGold(byte amount, byte id) => _server.GetBattleData(id).Gold += amount;
    public void UseGold(byte amount, byte id)
    {
        if(_server.GetBattleData(id).Gold >= amount)
            _server.GetBattleData(id).Gold -= amount;
    }

    public void UseFood(byte amount, byte id) => _server.GetBattleData(id).Food += amount;

    public void AddFood(byte amount, byte id)
    {
        if (_server.GetBattleData(id).Food >= amount)
            _server.GetBattleData(id).Food -= amount;
    }
}
