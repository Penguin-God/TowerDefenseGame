using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UnitTooltipController
{
    readonly UnitDamageInfoManager _damageInfoManager;
    public UnitTooltipController(UnitDamageInfoManager damageInfoManager) => _damageInfoManager = damageInfoManager;

    readonly float OFFSET_X = 150f;
    readonly float DELAY_TIME = 0.5f;
    readonly int FONT_SIZE = 16;
    readonly Vector2 WINDOW_SIZE = new Vector2(250, 200);
    public void SetMouseOverAction(IEnumerable<UI_UnitTracker> uis)
    {
        uis.ToList().ForEach(x => {
            var mouseOverHandler = x.gameObject.AddComponent<MouseOverHandler>();
            mouseOverHandler.SetDelayTime(DELAY_TIME);
            mouseOverHandler.OnPointerEnterDelayedEvent += () => ShowUnitTooltip(x, OFFSET_X);
            mouseOverHandler.OnPointerExitEvent += CloseWindow;
        });
    }

    BackGround _currentWindow;
    void ShowUnitTooltip(UI_UnitTracker tracker, float offSetX)
    {
        _currentWindow = Managers.UI.ShowDefualtUI<BackGround>();
        float screenWidthScaleFactor = Screen.width / Managers.UI.UIScreenWidth; // 플레이어 스크린 크기 대비 설정한 UI 비율
        _currentWindow.SetPosition(tracker.GetComponent<RectTransform>().position + new Vector3(offSetX * screenWidthScaleFactor, 0, 0));
        _currentWindow.SetFontSize(FONT_SIZE);
        _currentWindow.SetSize(WINDOW_SIZE);
        _currentWindow.SetAnchor(TMPro.TextAlignmentOptions.MidlineLeft);
        _currentWindow.SetLineSpace(11f);
        _currentWindow.SetText(BuildUnitDescrtion(tracker.UnitFlags));
        _currentWindow.gameObject.SetActive(true);
    }

    string BuildUnitDescrtion(UnitFlags flag)
    {
        var result = new StringBuilder();
        result.Append(TextUtility.UnitKeyToValue(Managers.Data.UnitWindowDataByUnitFlags[flag].Description, flag));
        result.AppendLine();
        result.AppendLine();
        UnitDamageInfo upgradeInfo = _damageInfoManager.GetUpgradeInfo(flag);
        result.Append($"일반 몬스터 공격력 : {upgradeInfo.ApplyDamage}");
        result.AppendLine();
        result.Append($"보스 몬스터 공격력 : {upgradeInfo.ApplyBossDamage}");
        result.AppendLine();
        result.Append("적용된 강화");
        result.AppendLine();
        result.Append($"공격력 {upgradeInfo.BaseDamage} 증가 및 {Mathf.RoundToInt(upgradeInfo.DamageRate * 100)}% 증가.");
        result.AppendLine();
        result.Append($"보스 공격력 {upgradeInfo.BaseBossDamage} 증가 및 {Mathf.RoundToInt(upgradeInfo.BossDamageRate * 100)}% 증가");
        return result.ToString();
    }

    void CloseWindow()
    {
        if(_currentWindow != null )
            Managers.Resources.Destroy(_currentWindow.gameObject);
    }
}
