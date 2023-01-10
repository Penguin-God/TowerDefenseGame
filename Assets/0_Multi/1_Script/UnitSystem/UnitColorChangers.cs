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

    [PunRPC]
    public void ChangeUnitColor(int id, UnitFlags unitFlag)
    {
        if (PhotonNetwork.IsMasterClient)
            new UnitColorChanger().ChangeUnitColor(Multi_UnitManager.Instance.FindUnit(id, unitFlag.UnitClass));
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

    public void ChangeUnitColor(Multi_TeamSoldier target)
    {
        Debug.Log(target.UnitFlags.ColorNumber);

        Util.GetRangeList(0, MAX_COLOR_NUMBER)
            .Where(x => x != target.UnitFlags.ColorNumber) // 애초에 flag로 찾으면 안 됨
            .ToList().ForEach(x => Debug.Log(x));

        Multi_SpawnManagers.NormalUnit.Spawn(GetRandomColor(target.UnitFlags.ColorNumber), (int)target.unitClass, target.transform.position, target.transform.rotation, target.UsingID);
        Multi_UnitManager.Instance.KillUnit(target);
    }
}
