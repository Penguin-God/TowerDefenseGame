using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkillColorChanger : MonoBehaviourPun
{
    UnitColorChangeTextPresenter textPresenter = new UnitColorChangeTextPresenter();
    public void ColorChangeSkill(UnitClass targetClass)
        => photonView.RPC(nameof(ColorChangeSkill), RpcTarget.MasterClient, Multi_Data.instance.EnemyPlayerId, targetClass);

    [PunRPC]
    void ColorChangeSkill(int id, UnitClass targetClass)
    {
        var target = Multi_UnitManager.Instance.FindUnit(id, targetClass);
        if (target == null)
        {
            PopupText(textPresenter.ChangeFaildText);
            return;
        }

        var resultFlag = Multi_UnitManager.Instance.ColorChangeHandler.ChangeUnitColor(id, target.UnitFlags);
        ShowColorChageResultText(target.UnitFlags, resultFlag);
    }

    void ShowColorChageResultText(UnitFlags before, UnitFlags after)
    {
        PopupText(textPresenter.GenerateTextShowToDisruptor(before, after));
        photonView.RPC(nameof(PopupText), RpcTarget.Others, textPresenter.GenerateTextShowToVictim(before, after));
    }

    [PunRPC]
    void PopupText(string text) => TextPopup.PopupText(text);
}
