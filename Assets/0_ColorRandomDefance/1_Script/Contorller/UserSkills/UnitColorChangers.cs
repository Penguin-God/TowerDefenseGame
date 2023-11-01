using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class UnitColorChangerRpcHandler : MonoBehaviourPun
{
    Multi_NormalUnitSpawner _unitSpawner;
    UnitManagerController _unitManagerController;
    public void DependencyInject(Multi_NormalUnitSpawner unitSpawner, UnitManagerController unitManagerController)
    {
        _unitSpawner = unitSpawner;
        _unitManagerController = unitManagerController;
    }

    [PunRPC]
    public UnitFlags ChangeUnitColorWithViewId(int viewID)
    {
        if (PhotonNetwork.IsMasterClient)
            return ChangeUnitColor(PhotonView.Find(viewID).GetComponent<Multi_TeamSoldier>());
        else
            photonView.RPC(nameof(ChangeUnitColorWithViewId), RpcTarget.MasterClient, viewID);
        return new UnitFlags(0, 0);
    }

    [PunRPC]
    public UnitFlags ChangeUnitColor(byte id, UnitFlags unitFlag)
    {
        if (PhotonNetwork.IsMasterClient)
            return ChangeUnitColor(_unitManagerController.GetUnit(id, unitFlag));
        else
            photonView.RPC(nameof(ChangeUnitColor), RpcTarget.MasterClient, id, unitFlag);
        return new UnitFlags(0, 0);
    }

    UnitFlags GetChangeFlag(UnitFlags origin) => new UnitFlags(UnitFlags.NormalColors.Where(x => x != origin.UnitColor).ToList().GetRandom(), origin.UnitClass);
    UnitFlags ChangeUnitColor(Multi_TeamSoldier target)
    {
        var newUnit = _unitSpawner.RPCSpawn(GetChangeFlag(target.UnitFlags), target.transform.position, target.transform.rotation, target.UsingID);
        if (target.IsInDefenseWorld == false) newUnit.ChangeWorldStateToAll();
        target.Dead();
        return newUnit.UnitFlags;
    }
}
