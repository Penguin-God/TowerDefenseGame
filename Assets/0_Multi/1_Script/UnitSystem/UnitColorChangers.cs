using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;


// 이거 모노비해비에서 멀티 담당하는 컴포넌트 만들어야 됨
public static class UnitColorChangerFactory
{
    public static UnitColorChangerByPhotonViewId CreateChangerByPhotonViewId(int viewID)
        => new UnitColorChangerByPhotonViewId(viewID);

    public static UnitColorChangerByUnitFlag CreateChangerByUnitFlag(UnitFlags unitFlags) 
        => new UnitColorChangerByUnitFlag(unitFlags);
}

public abstract class UnitColorChanger
{
    readonly int MAX_COLOR_NUMBER = 6;
    protected abstract Multi_TeamSoldier FindUnit();
    int GetRandomColor(int colorNum) => Util.GetRangeList(0, MAX_COLOR_NUMBER)
        .Where(x => x != colorNum)
        .ToList()
        .GetRandom();

    public void ChangeUnitColor()
    {
        var unit = FindUnit();
        Debug.Log(unit.UnitFlags.ColorNumber);
        Util.GetRangeList(0, MAX_COLOR_NUMBER)
        .Where(x => x != unit.UnitFlags.ColorNumber) // 애초에 flag로 찾으면 안 됨
        .ToList().ForEach(x => Debug.Log(x));
        Multi_UnitManager.Instance.UnitColorChanged_RPC(Multi_Data.instance.EnemyPlayerId, unit.UnitFlags, GetRandomColor(unit.UnitFlags.ColorNumber));
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
    protected override Multi_TeamSoldier FindUnit() => Multi_UnitManager.Instance.GetUnit(Multi_Data.instance.Id, _unitFlag);
}