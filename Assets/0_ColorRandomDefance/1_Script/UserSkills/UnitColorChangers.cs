using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class UnitColorChangerRpcHandler : MonoBehaviour
{
    static PhotonView photonView;
    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    public static void ChangeUnitColor(int viewID)
    {
        if (PhotonNetwork.IsMasterClient)
            new UnitColorChanger().ChangeUnitColor(PhotonView.Find(viewID).GetComponent<Multi_TeamSoldier>());
        else
            photonView.RPC(nameof(ChangeUnitColor), RpcTarget.MasterClient, viewID);
    }

    [PunRPC]
    public static UnitFlags ChangeUnitColor(byte id, UnitFlags unitFlag)
    {
        if (PhotonNetwork.IsMasterClient)
            return new UnitColorChanger().ChangeUnitColor(Multi_UnitManager.Instance.FindUnit(id, unitFlag.UnitClass));
        else
            photonView.RPC(nameof(ChangeUnitColor), RpcTarget.MasterClient, id, unitFlag);
        return new UnitFlags(0, 0);
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
        if(target.EnterStroyWorld) // 스폰된 얘가 맨 뒤에 있을 거니까 Last()의 월드를 바꿈. 좋은 코드는 아님
            Multi_UnitManager.Instance.Master.GetUnitList(target.UsingID, newFlag).Last().State.ChangedWorld();
        Multi_UnitManager.Instance.KillUnit(target);
        return newFlag;
    }
}
