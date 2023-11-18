using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleButtons : UI_Scene
{
    enum Buttons
    {
        SummonUnitButton,
        StoryWolrd_EnterButton,
        WorldChangeButton,
        // CombinButton,
    }

    enum Texts
    {
        StoryWorldText,
        WorldDestinationText,
    }

    //enum GameObjects
    //{
    //    CombineObject,
    //    CombineButtonsParent,
    //    CombineImpossibleText,
    //}

    protected override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        // Bind<GameObject>(typeof(GameObjects));

        Managers.Camera.OnIsLookMyWolrd += (isLookMy) => GetButton((int)Buttons.SummonUnitButton).gameObject.SetActive(isLookMy);
        GetButton((int)Buttons.StoryWolrd_EnterButton).onClick.AddListener(CameraPositionChanged);
        GetButton((int)Buttons.SummonUnitButton).onClick.AddListener(SommonUnit);
        GetButton((int)Buttons.WorldChangeButton).onClick.AddListener(ChangeWorld);

        // GetButton((int)Buttons.CombinButton).onClick.AddListener(ClickCombineButton);

        // uI_UnitOperater = GetComponentInChildren<UI_UnitOperater>();
        // uI_UnitOperater.DependencyInject(_unitCombineHandler, _worldUnitManager);
        // _dispatcher.OnUnitCountChange += _ => uI_UnitOperater.ShowOperableUnits(); // SortByCombineables(_unitCombineHandler);
        // GetObject((int)GameObjects.CombineObject).SetActive(false);
    }

    public void DependencyInject(SwordmanGachaController swordmanGachaController, TextShowAndHideController textShowAndHideController, UnitManagerController unitManagerController, UnitCombineMultiController combineController, BattleEventDispatcher dispatcher)
    {
        _swordmanGachaController = swordmanGachaController;
        _textShowAndHideController = textShowAndHideController;
        GetComponentInChildren<UI_UnitOperationsHub>().DependencyInject(unitManagerController, combineController, dispatcher);
    }

    void ChangeText(Texts textType, string text) => GetTextMeshPro((int)textType).text = text;

    void CameraPositionChanged()
    {
        Managers.UI.CloseAllPopupUI();
        Managers.Sound.PlayEffect(EffectSoundType.PopSound);
        if (Managers.Camera.IsLookEnemyTower)
        {
            Managers.Camera.LookWorld();
            ChangeText((int)Texts.StoryWorldText, "적군의 성으로");
        }
        else
        {
            Managers.Camera.LookEnemyTower();
            ChangeText((int)Texts.StoryWorldText, "월드로");
        }
    }

    [SerializeField] Sprite lookMyWorldIcon;
    [SerializeField] Sprite lookEnemyWorldIcon;
    void ChangeWorld()
    {
        Managers.Camera.LookWorldChanged();
        if (Managers.Camera.CameraSpot.IsInDefenseWorld == false)
            Managers.Camera.LookWorld();
        Managers.Sound.PlayEffect(EffectSoundType.PopSound);
        if (Managers.Camera.LookWorld_Id == PlayerIdManager.Id)
        {
            GetButton((int)Buttons.WorldChangeButton).image.sprite = lookMyWorldIcon;
            ChangeText(Texts.WorldDestinationText, "상대 진영으로");
            ChangeText((int)Texts.StoryWorldText, "적군의 성으로");
        }
        else
        {
            GetButton((int)Buttons.WorldChangeButton).image.sprite = lookEnemyWorldIcon;
            ChangeText(Texts.WorldDestinationText, "아군 진영으로");
            ChangeText((int)Texts.StoryWorldText, "적군의 성으로");
        }
    }

    SwordmanGachaController _swordmanGachaController;
    TextShowAndHideController _textShowAndHideController;
    void SommonUnit()
    {
        if (Multi_GameManager.Instance.UnitOver)
        {
            _textShowAndHideController.ShowTextForTime("유닛 공간이 부족해 소환할 수 없습니다.", Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
            return;
        }

        GachaUnit();
    }

    void GachaUnit()
    {
        if (_swordmanGachaController.TryDrawUnit())
            Managers.Sound.PlayEffect(EffectSoundType.DrawSwordman);
    }

    //void ClickCombineButton()
    //{
    //    if (GetObject((int)GameObjects.CombineObject).activeSelf)
    //        GetObject((int)GameObjects.CombineObject).SetActive(false);
    //    else
    //    {
    //        GetObject((int)GameObjects.CombineObject).SetActive(true);
    //        // uI_UnitOperater.ShowOperableUnits();
    //    }
    //}

    //void SortByCombineables(IUnitOperationHandler operationHandler)
    //{
    //    foreach (Transform child in GetObject((int)GameObjects.CombineButtonsParent).transform)
    //        Managers.Resources.Destroy(child.gameObject);

    //    // _combineSystem.GetCombinableUnitFalgs(_worldUnitManager.GetUnitFlags(PlayerIdManager.Id));
    //    var combineableUnitFalgs = operationHandler.GetOperableUnits(_worldUnitManager.GetUnitFlags(PlayerIdManager.Id));
    //    if (combineableUnitFalgs.Count() > 0)
    //    {
    //        GetObject((int)GameObjects.CombineImpossibleText).SetActive(false);
    //        foreach (var unitFlag in SortUnitFlags(combineableUnitFalgs))
    //        {
    //            var icon = Managers.UI.MakeSubItem<UI_UnitIcon>(GetObject((int)GameObjects.CombineButtonsParent).transform);
    //            icon.SetUnitIcon(unitFlag);
    //            icon.BindClickEvent(Do);

    //            void Do() => operationHandler.Do(unitFlag);
    //        }
    //    }
    //    else
    //        GetObject((int)GameObjects.CombineImpossibleText).SetActive(true);
    //}

    //IEnumerable<UnitFlags> SortUnitFlags(IEnumerable<UnitFlags> flags)
    //    => flags
    //        .Where(x => UnitFlags.NormalFlags.Contains(x))
    //        .OrderByDescending(x => x.ClassNumber)
    //        .ThenByDescending(x => x.ColorNumber)
    //        .Reverse();
}
