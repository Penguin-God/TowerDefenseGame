using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UnitTooltipController
{
    readonly float OFFSET_X = 150f;
    readonly float DELAY_TIME = 0.5f;
    readonly int FONT_SIZE = 32;
    readonly Vector2 WINDOW_SIZE = new Vector2(250, 190);
    public void SetMouseOverAction(IEnumerable<UI_UnitTracker> uis)
    {
        uis.ToList().ForEach(x => {
            var mouseOverHandler = x.gameObject.AddComponent<MouseOverHandler>();
            mouseOverHandler.SetDelayTime(DELAY_TIME);
            mouseOverHandler.OnPointerEnterDelayedEvent += () => ShowUnitTooltip(x, OFFSET_X);
            mouseOverHandler.OnPointerExitEvent += CloseWindow;
        });
    }

    bool isShowWindow = false;
    void ShowUnitTooltip(UI_UnitTracker tracker, float offSetX)
    {
        BackGround window = Managers.UI.ShowPopupUI<BackGround>();
        float screenWidthScaleFactor = Screen.width / Managers.UI.UIScreenWidth;
        window.SetPosition(tracker.GetComponent<RectTransform>().position + new Vector3(offSetX * screenWidthScaleFactor, 0, 0));
        window.SetFontSize(FONT_SIZE);
        window.SetSize(WINDOW_SIZE);
        window.SetAnchor(TextAnchor.MiddleLeft);
        window.SetLineSpace(1.1f);
        window.SetText(BuildUnitDescrtion(tracker.UnitFlags));
        isShowWindow = true;
    }

    string BuildUnitDescrtion(UnitFlags flag)
    {
        var result = new StringBuilder();
        result.Append(Managers.Data.UnitWindowDataByUnitFlags[flag].Description);
        result.AppendLine();
        result.AppendLine();
        var damInfo = MultiServiceMidiator.UnitUpgrade.GetUnitDamageInfo(flag);
        result.Append($"일반 몬스터 공격력 : {damInfo.ApplyDamage}");
        result.AppendLine();
        result.Append($"보스 몬스터 공격력 : {damInfo.ApplyBossDamage}");
        result.AppendLine();
        var game = Multi_GameManager.Instance;
        result.Append($"적용된 상점 강화 : 대미지 {game.GetUnitUpgradeShopAddDamageValue(flag)} 증가 및 대미지 {game.GetUnitUpgradeShopUpScaleValue(flag)}% 증가");
        return result.ToString();
    }

    void CloseWindow()
    {
        if (isShowWindow)
        {
            isShowWindow = false;
            Managers.UI.ClosePopupUI();
        }
    }
}
