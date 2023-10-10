﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UnitTooltipController
{
    readonly UnitStatController _unitStatController;
    public UnitTooltipController(UnitStatController unitStatController) => _unitStatController = unitStatController;

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
        var damInfo = _unitStatController.GetDamageInfo(flag, PlayerIdManager.Id); new UnitDamageInfo();
        result.Append($"일반 몬스터 공격력 : {damInfo.ApplyDamage}");
        result.AppendLine();
        result.Append($"보스 몬스터 공격력 : {damInfo.ApplyBossDamage}");
        result.AppendLine();
        result.Append($"적용된 상점 강화 : 대미지 {_unitStatController.GetUnitUpgradeValue(flag)} 증가 및 대미지 {_unitStatController.GetUnitUpgradeScale(flag)}% 증가");
        return result.ToString();
    }

    void CloseWindow()
    {
        if(_currentWindow != null )
            Managers.Resources.Destroy(_currentWindow.gameObject);
    }
}
