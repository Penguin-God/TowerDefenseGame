using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class UnitColorChangerRpcHandler : MonoBehaviourPun
{
    [PunRPC]
    public void ChangeUnitColor(int viewID)
    {
        if (PhotonNetwork.IsMasterClient)
            new UnitColorChanger().ChangeUnitColor(PhotonView.Find(viewID).GetComponent<Multi_TeamSoldier>());
        else
            photonView.RPC(nameof(ChangeUnitColor), RpcTarget.MasterClient, viewID);
    }

    // 이거 여따 두지 말고 그냥 스킬에 둬
    RPCAction<UnitFlags, UnitFlags> OnChangeColor = new RPCAction<UnitFlags, UnitFlags>(); // 변하기 전 색깔, 후 색깔
    [PunRPC]
    public void ChangeUnitColor(int id, UnitFlags unitFlag)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var target = Multi_UnitManager.Instance.FindUnit(id, unitFlag.UnitClass);
            if (target == null) return;
            OnChangeColor.RaiseEvent(id, unitFlag, new UnitColorChanger().ChangeUnitColor(target));
        }
        else
            photonView.RPC(nameof(ChangeUnitColor), RpcTarget.MasterClient, id, unitFlag);
    }
}

public class UnitColorChanger
{
    readonly int MAX_COLOR_NUMBER = 6;
    int GetRandomColor(int colorNum) => Util.GetRangeList(0, MAX_COLOR_NUMBER)
        .Where(x => x != colorNum)
        .ToList()
        .GetRandom();

    public UnitFlags ChangeUnitColor(Multi_TeamSoldier target)
    {
        var newFlag = new UnitFlags(GetRandomColor(target.UnitFlags.ColorNumber), (int)target.unitClass);
        Multi_SpawnManagers.NormalUnit.Spawn(newFlag, target.transform.position, target.transform.rotation, target.UsingID);
        Multi_UnitManager.Instance.KillUnit(target);
        return newFlag;
    }
}
