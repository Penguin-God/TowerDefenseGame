using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CurrencyControllerProxy : MonoBehaviourPun, IBattleCurrencyManager
{
    IBattleCurrencyManager _currencyManager;
    public void Init(IBattleCurrencyManager currencyManager) => _currencyManager = currencyManager;

    public int CurrentGold => _currencyManager.CurrentGold;
    public void AddGold(int amount)
    {
        _currencyManager.AddGold(amount);
        RPC_To_Master(nameof(AddGold), amount);
    }
    public void UseGold(int amount)
    {
        _currencyManager.UseGold(amount);
        RPC_To_Master(nameof(UseGold), amount);
    }

    public int CurrentFood => _currencyManager.CurrentFood;
    public void UseFood(int amount)
    {
        _currencyManager.UseFood(amount);
        RPC_To_Master(nameof(UseFood), amount);
    }

    public void AddFood(int amount)
    {
        _currencyManager.AddFood(amount);
        RPC_To_Master(nameof(AddFood), amount);
    }

    void RPC_To_Master(string methodName, int amount) => photonView.RPC(methodName, RpcTarget.MasterClient, (byte)amount, PlayerIdManager.Id);

    [PunRPC]
    public virtual void AddGold(byte amount, byte id) { }
    [PunRPC]
    public virtual void UseGold(byte amount, byte id) { }
    [PunRPC]
    public virtual void AddFood(byte amount, byte id) { }
    [PunRPC]
    public virtual void UseFood(byte amount, byte id) { }
}

public class MultiCurrencyController : CurrencyControllerProxy
{
    ServerManager _server;
    public void SetInfo(ServerManager server) => _server = server;

    public override void AddGold(byte amount, byte id) => _server.GetBattleData(id).Gold += amount;
    public override void UseGold(byte amount, byte id)
    {
        if(_server.GetBattleData(id).Gold >= amount)
            _server.GetBattleData(id).Gold -= amount;
    }

    public override void UseFood(byte amount, byte id) => _server.GetBattleData(id).Food += amount;

    public override void AddFood(byte amount, byte id)
    {
        if (_server.GetBattleData(id).Food >= amount)
            _server.GetBattleData(id).Food -= amount;
    }
}
