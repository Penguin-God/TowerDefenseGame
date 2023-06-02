using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentStatusSynchronizer
{
    public RPCAction<byte, UnitClass, byte> OnOtherUnitCountChanged = new RPCAction<byte, UnitClass, byte>();
    public RPCAction<byte> OnOtherUnitMaxCountChanged = new RPCAction<byte>();

    public OpponentStatusSynchronizer()
    {
        Managers.Unit.OnUnitCountChangeByClass += SendUnitCountDataToOpponent;
        Multi_GameManager.Instance.BattleData.OnMaxUnitChanged += SendUnitMaxCountDataToOpponent;
    }

    void SendUnitCountDataToOpponent(UnitClass unitClass, int count)
        => OnOtherUnitCountChanged?.RaiseToOther((byte)Managers.Unit.CurrentUnitCount, unitClass, (byte)count);
    void SendUnitMaxCountDataToOpponent(int count) => OnOtherUnitMaxCountChanged?.RaiseToOther((byte)count);
}
