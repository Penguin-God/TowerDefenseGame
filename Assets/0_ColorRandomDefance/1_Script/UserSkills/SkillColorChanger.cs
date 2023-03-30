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
            ShowFaildText(targetID);
            return;
        }

        var resultFlag = UnitColorChangerRpcHandler.ChangeUnitColor(targetID, target.UnitFlags);
        ShowColorChageResultText(targetID, target.UnitFlags, resultFlag);
    }

    // 인자로 넘겨준건 스킬을 적용시킬 타겟 ID라서 텍스트 띄우는 건 반대로 생각해야 됨
    void ShowFaildText(byte targetID)
    {
        if (targetID == PlayerIdManager.MasterId)
            ShowFaildText();
        else
            photonView.RPC(nameof(ShowFaildText), RpcTarget.Others);
    }

    void ShowColorChageResultText(byte targetID, UnitFlags before, UnitFlags after)
    {
        (string textToShowOnMaster, string textToShowOnClinet) showTexts = 
            (targetID == 0) ? (_textPresenter.GenerateTextShowToVictim(before, after), _textPresenter.GenerateTextShowToDisruptor(before, after))
            : (_textPresenter.GenerateTextShowToDisruptor(before, after), _textPresenter.GenerateTextShowToVictim(before, after));
        PopupText(showTexts.textToShowOnMaster);
        photonView.RPC(nameof(PopupText), RpcTarget.Others, showTexts.textToShowOnClinet);
    }

    [PunRPC] // UI 클릭할 때마다 RPC 호출이 맞는 걸까? 벗 이걸 막으려면 클라이언트에서도 Unit보유 숫자 정도는 알고 있어야 함.
    void ShowFaildText() => PopupText(_textPresenter.ChangeFaildText);

    [PunRPC]
    void PopupText(string text)
    {
        var ui = Managers.UI.ShowDefualtUI<UI_PopupText>();
        ui.SetPosition(new Vector2(0, 120f));
        ui.Show(text, 2.5f);
    }
}
