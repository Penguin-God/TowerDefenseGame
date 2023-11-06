using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UnitTooltipController
{
    readonly UnitDamageInfoManager _damageInfoManager;
    readonly MouseroverTooltipHandler _tooltipHandler = new();
    public UnitTooltipController(UnitDamageInfoManager damageInfoManager) => _damageInfoManager = damageInfoManager;

    public void SetMouseOverAction(UI_UnitTracker tracker)
    {
        _tooltipHandler.SetMouseOverAction(tracker.GetComponent<RectTransform>(), BuildUnitDescrtion);

        string BuildUnitDescrtion()
        {
            UnitFlags flag = tracker.UnitFlags;
            var result = new StringBuilder();
            string unitDescription = Managers.Resources.LoadCsv<UI_UnitWindowData>("UIData/UI_UnitWindowData").First(x => x.UnitFlags == flag).Description;
            result.Append(TextUtility.UnitKeyToValue(unitDescription, flag));
            result.AppendLine();
            result.AppendLine();
            UnitDamageInfo damageInfo = _damageInfoManager.GetDamageInfo(flag);
            result.Append($"일반 몬스터 공격력 : {damageInfo.ApplyDamage}");
            result.AppendLine();
            result.Append($"보스 몬스터 공격력 : {damageInfo.ApplyBossDamage}");
            result.AppendLine();
            result.AppendLine();
            result.Append("적용된 강화");
            result.AppendLine();
            UnitDamageInfo upgradeInfo = _damageInfoManager.GetUpgradeInfo(flag);
            result.Append($"공격력 {upgradeInfo.BaseDamage} 증가 및 {Mathf.RoundToInt(upgradeInfo.DamageRate * 100)}% 증가.");
            result.AppendLine();
            result.Append($"보스 공격력 {upgradeInfo.BaseBossDamage} 증가 및 {Mathf.RoundToInt(upgradeInfo.BossDamageRate * 100)}% 증가");
            return result.ToString();
        }
    }
}

public class UnitJobTooltipController
{
    readonly MouseroverTooltipHandler _tooltipHandler = new();
    public void SetMouseOverAction(UI_UnitTracker tracker)
    {
        _tooltipHandler.SetMouseOverAction(tracker.GetComponent<RectTransform>(), BuildUnitDescrtion);

        string BuildUnitDescrtion() => Managers.Resources.LoadCsv<UnitJobTooltipData>("UIData/UI_UnitJobTooltipData").First(x => x.UnitClass == tracker.UnitFlags.UnitClass).Text;
    }
}

public class MouseroverTooltipHandler
{
    readonly float OFFSET_X = 150f;
    readonly float DELAY_TIME = 0.2f;
    readonly int FONT_SIZE = 16;
    readonly Vector2 WINDOW_SIZE = new(250, 200);

    public void SetMouseOverAction(RectTransform ui, Func<string> textBuilder)
    {
        var mouseOverHandler = ui.gameObject.GetOrAddComponent<MouseOverHandler>();
        mouseOverHandler.SetDelayTime(DELAY_TIME);
        mouseOverHandler.OnPointerEnterDelayedEvent += () => ShowUnitTooltip(ui, textBuilder?.Invoke());
        mouseOverHandler.OnPointerExitEvent += CloseWindow;
    }

    BackGround _currentWindow;
    void ShowUnitTooltip(RectTransform ui, string text)
    {
        _currentWindow = Managers.UI.ShowDefualtUI<BackGround>();
        float screenWidthScaleFactor = Screen.width / Managers.UI.UIScreenWidth; // 플레이어 스크린 크기 대비 설정한 UI 비율
        _currentWindow.SetPosition(ui.position + new Vector3(OFFSET_X * screenWidthScaleFactor, 0, 0));
        _currentWindow.SetFontSize(FONT_SIZE);
        _currentWindow.SetSize(WINDOW_SIZE);
        _currentWindow.SetAnchor(TMPro.TextAlignmentOptions.MidlineLeft);
        _currentWindow.SetLineSpace(11f);
        _currentWindow.SetText(text);
        _currentWindow.gameObject.SetActive(true);
    }

    void CloseWindow()
    {
        if (_currentWindow != null)
            Managers.Resources.Destroy(_currentWindow.gameObject);
    }
}
