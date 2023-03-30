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
            return new UnitColorChanger().ChangeUnitColor(MultiServiceMidiator.Server.GetUnits(id).Where(x => x.UnitClass == unitFlag.UnitClass).FirstOrDefault());
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

    // MasterOnly
    public UnitFlags ChangeUnitColor(Multi_TeamSoldier target)
    {
        var newFlag = new UnitFlags(GetRandomColor(target.UnitFlags.ColorNumber), (int)target.UnitClass);
        Multi_SpawnManagers.NormalUnit.Spawn(newFlag, target.transform.position, target.transform.rotation, target.UsingID);
        if(target.EnterStroyWorld) // 스폰된 얘가 맨 뒤에 있을 거니까 Last()의 월드를 바꿈. 좋은 코드는 아님
            MultiServiceMidiator.Server.GetUnits(target.UsingID).Where(x => x.UnitFlags == newFlag).Last().State.ChangedWorld();
        target.Dead();
        return newFlag;
    }
}
