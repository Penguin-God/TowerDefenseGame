using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class UnitColorChangerRpcHandler
{
    PhotonView photonView;
    public UnitColorChangerRpcHandler(PhotonView pv) => photonView = pv;

    [PunRPC]
    public void ChangeUnitColor(int viewID)
    {
        if (PhotonNetwork.IsMasterClient)
            new UnitColorChanger().ChangeUnitColor(PhotonView.Find(viewID).GetComponent<Multi_TeamSoldier>());
        else
            photonView.RPC("ChangeUnitColor", RpcTarget.MasterClient, viewID);
    }

    [PunRPC]
    public void ChangeUnitColor(UnitFlags unitFlag)
    {
        if (PhotonNetwork.IsMasterClient)
            new UnitColorChanger().ChangeUnitColor(Multi_UnitManager.Instance.FindUnit(Multi_Data.instance.EnemyPlayerId, unitFlag.UnitClass));
        else
            photonView.RPC("ChangeUnitColor", RpcTarget.MasterClient, unitFlag);
    }
}

public static class UnitColorChangerFactory
{
    public static UnitColorChangerByPhotonViewId CreateChangerByPhotonViewId(int viewID)
        => new UnitColorChangerByPhotonViewId(viewID);

    public static UnitColorChangerByUnitFlag CreateChangerByUnitFlag(UnitFlags unitFlags) 
        => new UnitColorChangerByUnitFlag(unitFlags);
}

public class UnitColorChanger
{
    readonly int MAX_COLOR_NUMBER = 6;
    protected virtual Multi_TeamSoldier FindUnit() { return null; }
    int GetRandomColor(int colorNum) => Util.GetRangeList(0, MAX_COLOR_NUMBER)
        .Where(x => x != colorNum)
        .ToList()
        .GetRandom();

    public void ChangeUnitColor() // 근데 여기서 유닛을 왜 찾음?
    {
        var unit = FindUnit();
        Debug.Log(unit.UnitFlags.ColorNumber);

        Util.GetRangeList(0, MAX_COLOR_NUMBER)
            .Where(x => x != unit.UnitFlags.ColorNumber) // 애초에 flag로 찾으면 안 됨
            .ToList().ForEach(x => Debug.Log(x));
        Multi_UnitManager.Instance.UnitColorChanged_RPC(Multi_Data.instance.EnemyPlayerId, unit.UnitFlags, GetRandomColor(unit.UnitFlags.ColorNumber));
    }

    public void ChangeUnitColor(Multi_TeamSoldier target)
    {
        Debug.Log(target.UnitFlags.ColorNumber);

        Util.GetRangeList(0, MAX_COLOR_NUMBER)
            .Where(x => x != target.UnitFlags.ColorNumber) // 애초에 flag로 찾으면 안 됨
            .ToList().ForEach(x => Debug.Log(x));

        Multi_UnitManager.Instance.UnitColorChanged_RPC(Multi_Data.instance.EnemyPlayerId, target.UnitFlags, GetRandomColor(target.UnitFlags.ColorNumber));
    }
}

public class UnitColorChangerByPhotonViewId : UnitColorChanger
{
    readonly int _viewID;
    public UnitColorChangerByPhotonViewId(int viewID) => _viewID = viewID;
    protected override Multi_TeamSoldier FindUnit() => PhotonView.Find(_viewID).GetComponent<Multi_TeamSoldier>();
}

public class UnitColorChangerByUnitFlag : UnitColorChanger
{
    readonly UnitFlags _unitFlag;
    public UnitColorChangerByUnitFlag(UnitFlags unitFlag) => _unitFlag = unitFlag;
    protected override Multi_TeamSoldier FindUnit() => Multi_UnitManager.Instance.FindUnit(Multi_Data.instance.Id, _unitFlag);
}