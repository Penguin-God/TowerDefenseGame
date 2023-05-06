using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentStatusSynchronizer
{
    public RPCAction<byte, UnitClass, byte> OnOtherUnitCountChanged = new RPCAction<byte, UnitClass, byte>();
    public RPCAction<byte> OnOtherUnitMaxCountChanged = new RPCAction<byte>();

    public OpponentStatusSynchronizer()
    {
        Multi_UnitManager.Instance.OnUnitCountChangeByClass += SendUnitCountDataToOpponent;
        Multi_GameManager.Instance.BattleData.OnMaxUnitChanged += SendUnitMaxCountDataToOpponent;
    }

    void SendUnitCountDataToOpponent(UnitClass unitClass, int count)
        => OnOtherUnitCountChanged?.RaiseToOther((byte)Multi_UnitManager.Instance.CurrentUnitCount, unitClass, (byte)count);
    void SendUnitMaxCountDataToOpponent(int count) => OnOtherUnitMaxCountChanged?.RaiseToOther((byte)count);
}
