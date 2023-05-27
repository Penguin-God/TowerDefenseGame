using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CurrencyManagerProxy : MonoBehaviourPun, IBattleCurrencyManager
{
    IBattleCurrencyManager _currencyManager;
    public void Init(IBattleCurrencyManager currencyManager) => _currencyManager = currencyManager;

    public int Gold => _currencyManager.Gold;
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

    public int Food => _currencyManager.Food;
    public void AddFood(int amount)
    {
        _currencyManager.AddFood(amount);
        RPC_To_Master(nameof(AddFood), amount);
    }
    public void UseFood(int amount)
    {
        _currencyManager.UseFood(amount);
        RPC_To_Master(nameof(UseFood), amount);
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

public class MasterCurrencyManager : CurrencyManagerProxy
{
    ServerManager _server;
    public void Init(ServerManager server) => _server = server;

    [PunRPC]
    public override void AddGold(byte amount, byte id) => _server.GetBattleData(id).Gold += amount;
    [PunRPC]
    public override void UseGold(byte amount, byte id)
    {
        if(HasGold(amount, id))
            _server.GetBattleData(id).Gold -= amount;
    }
    public bool HasGold(int amount, byte id) => _server.GetBattleData(id).Gold >= amount;
    public bool TryUseGold(int amount, byte id)
    {
        var result = HasGold(amount, id);
        UseGold((byte)amount, id);
        return result;
    }

    [PunRPC]
    public override void AddFood(byte amount, byte id) => _server.GetBattleData(id).Food += amount;
    [PunRPC]
    public override void UseFood(byte amount, byte id)
    {
        if (HasFood(amount, id))
            _server.GetBattleData(id).Food -= amount;
    }
    public bool HasFood(int amount, byte id) => _server.GetBattleData(id).Food >= amount;
}


public class CurrencyManagerMediator : MonoBehaviourPun, IBattleCurrencyManager
{
    MasterCurrencyManager _masterCurrencyManager;
    ServerManager _server;

    public int Gold => 0;
    public void AddGold(int amount) => RPC_To_Master(nameof(AddGold), amount);
    public void UseGold(int amount) => RPC_To_Master(nameof(UseGold), amount);

    public int Food => 0;
    public void AddFood(int amount) => RPC_To_Master(nameof(AddFood), amount);
    public void UseFood(int amount) => RPC_To_Master(nameof(UseFood), amount);

    void RPC_To_Master(string methodName, int amount) => photonView.RPC(methodName, RpcTarget.MasterClient, (byte)amount, PlayerIdManager.Id);

    [PunRPC]
    public virtual void AddGold(byte amount, byte id)
    {
        _masterCurrencyManager.AddGold(amount, id);
        SyncGold(id);
    }

    [PunRPC]
    public virtual void UseGold(byte amount, byte id) 
    {
        _masterCurrencyManager.UseGold(amount, id);
        SyncGold(id);
    }
    [PunRPC]
    public virtual void AddFood(byte amount, byte id) 
    {
        _masterCurrencyManager.AddFood(amount, id);
        SyncFood(id);
    }
    [PunRPC]
    public virtual void UseFood(byte amount, byte id) 
    {
        _masterCurrencyManager.UseFood(amount, id);
        SyncFood(id);
    }

    void SyncGold(byte id)
    {
        if (PlayerIdManager.MasterId == id)
            OverrideGold(_server.GetBattleData(id).Gold);
        else
            photonView.RPC(nameof(OverrideGold), RpcTarget.Others, _server.GetBattleData(id).Gold);
    }

    void SyncFood(byte id)
    {
        if (PlayerIdManager.MasterId == id)
            OverrideFood(_server.GetBattleData(id).Food);
        else
            photonView.RPC(nameof(OverrideFood), RpcTarget.Others, _server.GetBattleData(id).Food);
    }

    [PunRPC] void OverrideGold(int amount) { }
    [PunRPC] void OverrideFood(int amount) { }
}