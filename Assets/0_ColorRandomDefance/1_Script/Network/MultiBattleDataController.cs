using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBattleDataController : MonoBehaviourPun
{
    readonly MultiData<BattleData> _battleDatas = WorldDataFactory.CreateWorldData<BattleData>();
    public BattleData GetData(byte id) => _battleDatas.GetData(id);
    public void IncreasedMaxUnitCount(int amount)
    {
        GetData(PlayerIdManager.Id).MaxUnitCount += amount;
        Multi_GameManager.Instance.BattleData.ChangeMaxUnit(GetData(PlayerIdManager.Id).MaxUnitCount);
        if (PhotonNetwork.IsMasterClient == false)
            photonView.RPC(nameof(RPC_IncreasedMaxUnitCount), RpcTarget.MasterClient, (byte)amount, PlayerIdManager.Id);
    }

    [PunRPC]
    void RPC_IncreasedMaxUnitCount(byte amout, byte id) => GetData(id).MaxUnitCount += amout;
}
