using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkillColorChanger : MonoBehaviourPun
{
    RPCAction<UnitFlags, UnitFlags> OnChangeColor = new RPCAction<UnitFlags, UnitFlags>(); // 변하기 전 색깔, 후 색깔
    public void ColorChangeSkill(UnitFlags targetFlag)
        => photonView.RPC(nameof(ColorChangeSkill), RpcTarget.MasterClient, Multi_Data.instance.EnemyPlayerId, targetFlag);

    [PunRPC]
    void ColorChangeSkill(int id, UnitFlags targetFlag)
    {
        if(Multi_UnitManager.Instance.FindUnit(id, targetFlag.UnitClass) == null)
            return; // 실패했다는 텍스트 띄우고 return하기

        var afterFlag = Multi_UnitManager.Instance.ColorChangeHandler.ChangeUnitColor(id, targetFlag);
        // 텍스트 띄우
    }
}
