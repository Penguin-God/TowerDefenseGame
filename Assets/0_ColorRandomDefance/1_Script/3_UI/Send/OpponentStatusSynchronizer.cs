using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentStatusSynchronizer
{
    public RPCAction<byte, UnitFlags, byte> OnOtherUnitCountChanged = new RPCAction<byte, UnitFlags, byte>();
    public RPCAction<byte> OnOtherUnitMaxCountChanged = new RPCAction<byte>();

    public OpponentStatusSynchronizer()
    {
        Multi_UnitManager.Instance.OnUnitCountChangeByFlag += SendUnitCountDataToOpponent;
        Multi_GameManager.Instance.BattleData.OnMaxUnitChanged += SendUnitMaxCountDataToOpponent;
    }

    void SendUnitCountDataToOpponent(UnitFlags flag, int count)
        => OnOtherUnitCountChanged?.RaiseToOther((byte)Multi_UnitManager.Instance.CurrentUnitCount, flag, (byte)count);
    void SendUnitMaxCountDataToOpponent(int count) => OnOtherUnitMaxCountChanged?.RaiseToOther((byte)count);
}
