using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class SkillColorChanger : MonoBehaviourPun
{
    readonly UnitColorChangeTextPresenter _textPresenter = new UnitColorChangeTextPresenter();
    public void ColorChangeSkill(UnitClass targetClass)
        => photonView.RPC(nameof(ColorChangeSkill), RpcTarget.MasterClient, PlayerIdManager.EnemyId, targetClass);

    [PunRPC]
    void ColorChangeSkill(byte targetID, UnitClass targetClass)
    {
        var target = MultiServiceMidiator.Server.GetUnits(targetID).Where(x => x.UnitClass == targetClass).FirstOrDefault();
        if (target == null)
        {
            RPCFaildText(targetID);
            return;
        }

        var resultFlag = UnitColorChangerRpcHandler.ChangeUnitColor(targetID, target.UnitFlags);
        ShowColorChageResultText(targetID, target.UnitFlags, resultFlag);
    }

    void ShowColorChageResultText(byte targetID, UnitFlags before, UnitFlags after)
    {
        string changerText = _textPresenter.GenerateChangerText(before, after);
        string affectedText = _textPresenter.GenerateAffectedText(before, after);

        if (PlayerIdManager.IsMasterId(targetID))
            RPC_ColorChangeResultText(affectedText, changerText);
        else
            RPC_ColorChangeResultText(changerText, affectedText);
    }

    void RPC_ColorChangeResultText(string toMaster, string toClient)
    {
        PopupText(toMaster);
        photonView.RPC(nameof(PopupText), RpcTarget.Others, toClient);
    }

    // 인자로 넘겨준건 스킬을 적용시킬 타겟 ID라서 텍스트 띄우는 건 반대로 생각해야 됨
    void RPCFaildText(byte targetID)
    {
        if (PlayerIdManager.IsMasterId(targetID))
            photonView.RPC(nameof(ShowFaildText), RpcTarget.Others);
        else
            ShowFaildText();
    }

    [PunRPC] void ShowFaildText() => PopupText(_textPresenter.ChangeFaildText);

    [PunRPC]
    void PopupText(string text) => Managers.UI.ShowDefualtUI<UI_PopupText>().ShowTextForTime(text);
}
