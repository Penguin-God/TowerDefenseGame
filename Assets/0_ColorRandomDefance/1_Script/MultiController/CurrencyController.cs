using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CurrencyManagerMediator : MonoBehaviourPun, IBattleCurrencyManager
{
    Multi_GameManager _game;
    MultiData<CurrencyManager> _multiCurrencyManager;

    public void Init(Multi_GameManager game)
    {
        _multiCurrencyManager = MultiDataFactory.CreateMultiData<CurrencyManager>();
        _game = game;
    }

    public int Gold { get; set; }
    public void AddGold(int amount) => RPC_To_Master(nameof(AddGold), amount);
    public void UseGold(int amount) => RPC_To_Master(nameof(UseGold), amount);

    public int Food { get; set; }
    public void AddFood(int amount) => RPC_To_Master(nameof(AddFood), amount);
    public void UseFood(int amount) => RPC_To_Master(nameof(UseFood), amount);

    void RPC_To_Master(string methodName, int amount) => photonView.RPC(methodName, RpcTarget.MasterClient, (byte)amount, PlayerIdManager.Id);

    CurrencyManager GetCurrencyManager(byte id) => _multiCurrencyManager.GetData(id);

    [PunRPC]
    public virtual void AddGold(byte amount, byte id)
    {
        GetCurrencyManager(id).AddGold(amount);
        SyncGold(id);
    }

    [PunRPC]
    public virtual void UseGold(byte amount, byte id) 
    {
        GetCurrencyManager(id).UseGold(amount);
        SyncGold(id);
    }
    [PunRPC]
    public virtual void AddFood(byte amount, byte id) 
    {
        GetCurrencyManager(id).AddFood(amount);
        SyncFood(id);
    }
    [PunRPC]
    public virtual void UseFood(byte amount, byte id) 
    {
        GetCurrencyManager(id).UseFood(amount);
        SyncFood(id);
    }

    public bool TryUseGold(int amount, byte id)
    {
        if (GetCurrencyManager(id).Gold >= amount)
        {
            UseGold((byte)amount, id);
            return true;
        }
        else 
            return false;
    }

    void SyncGold(byte id)
    {
        if (PlayerIdManager.MasterId == id)
            OverrideGold(GetCurrencyManager(id).Gold);
        else
            photonView.RPC(nameof(OverrideGold), RpcTarget.Others, GetCurrencyManager(id).Gold);
    }

    void SyncFood(byte id)
    {
        if (PlayerIdManager.MasterId == id)
            OverrideFood(GetCurrencyManager(id).Food);
        else
            photonView.RPC(nameof(OverrideFood), RpcTarget.Others, GetCurrencyManager(id).Food);
    }

    [PunRPC] void OverrideGold(int amount) => _game.UpdateGold(amount);
    [PunRPC] void OverrideFood(int amount) => _game.UpdateFood(amount);
}