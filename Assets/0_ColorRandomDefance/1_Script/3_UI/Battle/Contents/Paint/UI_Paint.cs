using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Paint : UI_Scene
{
    enum SortType
    {
        Default,
        Color,
        Class,
        Combineable,
    }

    enum GameObjects
    {
        TrackerParent,
        ColorButtons,
        PaintButton,
    }

    enum Buttons
    {
        ClassButton,
        CombineableButton,
    }


    UnitStatController _unitStatController;
    BattleEventDispatcher _dispatcher;
    WorldUnitManager _worldUnitManager;
    UnitCombineSystem _combineSystem;
    public void DependencyInject(UnitStatController unitStatController, BattleEventDispatcher dispatcher, WorldUnitManager worldUnitManager, UnitCombineSystem combineSystem)
    {
        _unitStatController = unitStatController;
        _dispatcher = dispatcher;
        _worldUnitManager = worldUnitManager;
        _combineSystem = combineSystem;
    }

    SortType _currentSortType = SortType.Default;
    List<UI_UnitTracker> _currentTrackers = new List<UI_UnitTracker>();
    Transform _trackerParent;
    GridLayoutGroup _layoutGroup;
    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        _trackerParent = GetObject((int)GameObjects.TrackerParent).transform;
        _layoutGroup = _trackerParent.GetComponent<GridLayoutGroup>();

        GetObject((int)GameObjects.PaintButton).GetComponent<Button>().onClick.AddListener(ChangePaintRootActive);
        GetButton((int)Buttons.ClassButton).onClick.AddListener(SortDefault);

        for (int i = 0; i < UnitFlags.NormalColors.Count(); i++)
            SetSortAction(GameObjects.ColorButtons, i, SortByColor);

        GetButton((int)Buttons.CombineableButton).onClick.AddListener(SortByCombineable);

        _dispatcher.OnUnitCountChangeByFlag += (flag, _) => UpdateUI(flag);
        UpdateUI(_currentSortType);
    }

    void SetSortAction(GameObjects ojects, int childIndex, UnityAction<int> action)
    {
        GetObject((int)ojects).transform.GetChild(childIndex).GetComponent<Button>().onClick.AddListener(() => action(childIndex));
    }

    void ChangePaintRootActive()
    {
        GetObject((int)GameObjects.ColorButtons).SetActive(!GetObject((int)GameObjects.ColorButtons).activeSelf);
        Managers.Sound.PlayEffect(EffectSoundType.PopSound_2);
    }


    void SortByCombineable() => UpdateUI(SortType.Combineable);

    void SortDefault() => UpdateUI(SortType.Default);

    void __SortDefault()
    {
        _layoutGroup.constraint = GridLayoutGroup.Constraint.Flexible;
        _layoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
        _layoutGroup.padding.top = 0;
        foreach (var unitClass in UnitFlags.AllClass)
        {
            var tracker = CreateTracker(new UnitFlags(UnitColor.Black, unitClass));
            tracker.UpdateUnitCountText(_worldUnitManager.GetUnitCount(PlayerIdManager.Id, unit => unit.UnitFlags.UnitClass == tracker.UnitFlags.UnitClass));
            tracker.GetComponent<Button>().onClick.AddListener(() => SortByClass(unitClass));
        }
    }

    public void SortByColor(int colorNumber)
    {
        _layoutGroup.constraint = GridLayoutGroup.Constraint.Flexible;
        _layoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
        _layoutGroup.padding.top = 0;
        UpdateUI(SortType.Color);

        foreach (var unitClass in UnitFlags.AllClass)
            CreateTracker(new UnitFlags((UnitColor)colorNumber, unitClass));
    }

    void SortByClass(UnitClass unitClass)
    {
        _layoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        _layoutGroup.constraintCount = 3;
        _layoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
        _layoutGroup.padding.top = 70;
        UpdateUI(SortType.Class);

        foreach (var unitColor in UnitFlags.NormalColors)
            CreateTracker(new UnitFlags(unitColor, unitClass));
    }

    void SortByCombineables()
    {
        _layoutGroup.startCorner = GridLayoutGroup.Corner.LowerLeft;
        _layoutGroup.padding.top = 230;

        var combineableUnitFalgs = _combineSystem.GetCombinableUnitFalgs(_worldUnitManager.GetUnitFlags(PlayerIdManager.Id));
        foreach (var unitFlag in SortUnitFlags(combineableUnitFalgs))
            CreateTracker(unitFlag);
    }

    const int MAX_COMBINABLE_TRACKER_COUNT = 4;
    IEnumerable<UnitFlags> SortUnitFlags(IEnumerable<UnitFlags> flags)
        => flags
            .Where(x => UnitFlags.NormalFlags.Contains(x))
            .OrderByDescending(x => x.ClassNumber)
            .ThenByDescending(x => x.ColorNumber)
            .Take(MAX_COMBINABLE_TRACKER_COUNT)
            .Reverse();

    UI_UnitTracker CreateTracker(UnitFlags flag)
    {
        var tracker = Managers.UI.MakeSubItem<UI_UnitTracker>(_trackerParent);
        tracker.SetInfo(flag, _worldUnitManager);
        new UnitTooltipController(_unitStatController.GetInfoManager(PlayerIdManager.Id)).SetMouseOverAction(tracker);
        _currentTrackers.Add(tracker);
        return tracker;
    }

    void UpdateUI(UnitFlags flag)
    {
        switch (_currentSortType)
        {
            case SortType.Default: UpdateDefaultCount(); break;
            case SortType.Color: 
            case SortType.Class: UpdateTrackersCount(flag); break;
            case SortType.Combineable: UpdateUI(SortType.Combineable); break;
        }
    }

    void UpdateDefaultCount()
    {
        foreach (var tracker in _currentTrackers)
            tracker.UpdateUnitCountText(_worldUnitManager.GetUnitCount(PlayerIdManager.Id, unit => unit.UnitFlags.UnitClass == tracker.UnitFlags.UnitClass));
    }

    void UpdateTrackersCount(UnitFlags flag) => _currentTrackers.FirstOrDefault(x => x.UnitFlags == flag)?.UpdateUnitCountText();

    void UpdateUI(SortType type)
    {
        foreach (Transform item in _trackerParent)
            Destroy(item.gameObject);
        _currentTrackers.Clear();

        GetObject((int)GameObjects.ColorButtons).SetActive(false);
        switch (type)
        {
            case SortType.Default: __SortDefault(); break;
            case SortType.Color: break;
            case SortType.Class: break;
            case SortType.Combineable: SortByCombineables(); break;
        }
        if(type != _currentSortType)
            _currentSortType = type;
    }
}
