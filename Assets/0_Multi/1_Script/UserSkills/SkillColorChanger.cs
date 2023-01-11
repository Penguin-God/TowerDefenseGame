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
    void ColorChangeSkill(int targetID, UnitClass targetClass)
    {
        var target = Multi_UnitManager.Instance.FindUnit(targetID, targetClass);
        if (target == null)
        {
            ShowFaildText(targetID);
            return;
        }

        var resultFlag = Multi_UnitManager.Instance.ColorChangeHandler.ChangeUnitColor(targetID, target.UnitFlags);
        ShowColorChageResultText(targetID, target.UnitFlags, resultFlag);
    }

    // 인자로 넘겨준건 상대방 ID라서 텍스트 띄우는 건 반대로 생각해야 됨
    void ShowFaildText(int targetID)
    {
        if (targetID == 0)
            photonView.RPC(nameof(ShowFaildText), RpcTarget.Others);
        else
            ShowFaildText();
    }

    void ShowColorChageResultText(int targetID, UnitFlags before, UnitFlags after)
    {
        (string textToShowOnMaster, string textToShowOnClinet) showTexts = 
            (targetID == 0) ? (textPresenter.GenerateTextShowToVictim(before, after), textPresenter.GenerateTextShowToDisruptor(before, after))
            : (textPresenter.GenerateTextShowToDisruptor(before, after), textPresenter.GenerateTextShowToVictim(before, after));
        PopupText(showTexts.textToShowOnMaster);
        photonView.RPC(nameof(PopupText), RpcTarget.Others, showTexts.textToShowOnClinet);
    }

    [PunRPC] // UI 클릭할 때마다 RPC 호출이 맞는 걸까? 벗 이걸 막으려면 클라이언트에서도 Unit보유 숫자 정도는 알고 있어야 함.
    void ShowFaildText() => PopupText(textPresenter.ChangeFaildText);

    [PunRPC]
    void PopupText(string text) => TextPopup.PopupText(text);
}
