using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitManagedWindow : UI_Popup
{
    enum Buttons
    {
        UnitSellButton,
        Unit_World_Changed_Button,
    }

    enum Texts
    {
        Description,
        Unit_World_Changed_Text,
        UnitNameText,
    }

    enum GameObjects
    {
        CombineUIParent,
    }

    [SerializeField] UnitFlags _unitFlag;
    public UnitFlags UnitFlags => _unitFlag;
    UI_CombineButtonParent _combineButtonsParent;

    protected override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        GetButton((int)Buttons.UnitSellButton).onClick.AddListener(SellUnit);
        GetButton((int)Buttons.Unit_World_Changed_Button).onClick.AddListener(UnitWorldChanged);
        _combineButtonsParent = GetComponentInChildren<UI_CombineButtonParent>();
    }

    public void Show(UnitFlags flags)
    {
        CheckInit();
        SetInfo(flags);
        gameObject.SetActive(true);
    }

    void SetInfo(UnitFlags flag)
    {
        if (Managers.Data.UnitWindowDataByUnitFlags.TryGetValue(flag, out var windowData) == false)
            return;
        _unitFlag = flag;
        GetText((int)Texts.Unit_World_Changed_Text).text = (Managers.Camera.IsLookEnemyTower) ? "월드로" : "적군의 성으로";
        GetText((int)Texts.UnitNameText).text = Managers.Data.UnitNameDataByFlag[_unitFlag].KoearName;
        CreateCombineUI(flag);
        //GetText((int)Texts.Description).text = windowData.CombinationRecipe;
        //_combineButtonsParent.SettingCombineButtons(windowData.CombineUnitFlags);

    }

    void CreateCombineUI(UnitFlags flag)
    {
        Transform parent = GetObject((int)GameObjects.CombineUIParent).transform;
        foreach (Transform child in parent)
            Destroy(child.gameObject);

        var flags = new List<UnitFlags>();
        if(Managers.Data.CombineConditionByUnitFalg.ContainsKey(flag))
            flags.Add(flag);
        flags.AddRange(
            Managers.Data.CombineConditionByUnitFalg
            .Values
            .Where(x => x.NeedCountByFlag.Keys.Contains(flag))
            .Select(x => x.TargetUnitFlag)
            .Where(x => UnitFlags.NormalColors.Contains(x.UnitColor))
            );

        foreach (var combineTargetFlag in flags.OrderBy(x => x.ClassNumber))
            Managers.UI.MakeSubItem<UI_UnitCombineInfoItem>(parent).SetInfo(combineTargetFlag);
    }

    void SellUnit()
    {
        if (Managers.Unit.TryFindUnit((unit) => unit.UnitFlags == _unitFlag, out var findUnit))
        {
            findUnit.Dead();
            Multi_GameManager.Instance.AddGold(Multi_GameManager.Instance.BattleData.UnitSellRewardDatas[(int)findUnit.UnitClass].Amount);
        }
    }

    void UnitWorldChanged()
        => Managers.Unit.FindUnit((unit) => unit.UnitFlags == _unitFlag && unit.EnterStroyWorld == Managers.Camera.IsLookEnemyTower)?.ChangeWorldToMaster();
}
