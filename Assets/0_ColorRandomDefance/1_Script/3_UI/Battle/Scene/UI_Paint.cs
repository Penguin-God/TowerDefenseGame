using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Paint : UI_Scene
{
    enum SortType
    {
        Default,
        Class,
    }

    enum GameObjects
    {
        TrackerParent,
        PaintBackGround,
    }

    enum Buttons
    {
        PaintButton,
    }

    UnitStatController _unitStatController;
    BattleUI_Mediator _uiMediator;
    BattleEventDispatcher _dispatcher;
    WorldUnitManager _worldUnitManager;
    public void DependencyInject(UnitStatController unitStatController, BattleUI_Mediator uiMediator, BattleEventDispatcher dispatcher, WorldUnitManager worldUnitManager)
    {
        _unitStatController = unitStatController;
        _uiMediator = uiMediator;
        _dispatcher = dispatcher;
        _worldUnitManager = worldUnitManager;
    }

    SortType _currentSortType;
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
        
        Managers.Camera.OnIsLookMyWolrd += (isLookMy) => gameObject.SetActive(isLookMy);
        GetButton((int)Buttons.PaintButton).onClick.AddListener(SortDefault);
        _dispatcher.OnUnitCountChangeByFlag += (flag, _) => OnChangeUnitCount(flag);

        SortDefault();

        // 중첩 함수
        void OnChangeUnitCount(UnitFlags flag)
        {
            switch (_currentSortType)
            {
                case SortType.Default: UpdateDefaultCount(); break;
                case SortType.Class: UpdateTrackersCount(flag); break;
            }

            void UpdateDefaultCount()
            {
                foreach (var tracker in _currentTrackers)
                    tracker.UpdateUnitCountText(_worldUnitManager.GetUnitCount(PlayerIdManager.Id, unit => unit.UnitFlags.UnitClass == tracker.UnitFlags.UnitClass));
            }

            void UpdateTrackersCount(UnitFlags flag) => _currentTrackers.FirstOrDefault(x => x.UnitFlags == flag)?.UpdateUnitCountText();
        }
    }


    void SortDefault()
    {
        Managers.UI.ClosePopupUI();
        SwitchSortType(SortType.Default);
        _layoutGroup.constraint = GridLayoutGroup.Constraint.Flexible;
        _layoutGroup.padding.right = 0;
        GetObject((int)GameObjects.PaintBackGround).SetActive(false);

        foreach (var unitClass in UnitFlags.AllClass)
        {
            var tracker = CreateTracker(new UnitFlags(UnitColor.Black, unitClass));
            tracker.UpdateUnitCountText(_worldUnitManager.GetUnitCount(PlayerIdManager.Id, unit => unit.UnitFlags.UnitClass == tracker.UnitFlags.UnitClass));
            tracker.GetComponent<Button>().onClick.AddListener(() => SortByClass(unitClass));
            new UnitJobTooltipController().SetMouseOverAction(tracker);
        }
    }

    void SortByClass(UnitClass unitClass)
    {
        SwitchSortType(SortType.Class);
        GetObject((int)GameObjects.PaintBackGround).SetActive(true);
        _layoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        _layoutGroup.constraintCount = 3;
        _layoutGroup.padding.right = 80;

        foreach (var unitColor in UnitFlags.NormalColors)
        {
            var tracker = CreateTracker(new UnitFlags(unitColor, unitClass));
            new UnitTooltipController(_unitStatController.GetInfoManager(PlayerIdManager.Id)).SetMouseOverAction(tracker);
        }
    }

    UI_UnitTracker CreateTracker(UnitFlags flag)
    {
        var tracker = Managers.UI.MakeSubItem<UI_UnitTracker>(_trackerParent);
        tracker.SetInfo(flag, _worldUnitManager);
        _currentTrackers.Add(tracker);
        return tracker;
    }

    void ClearTrackers()
    {
        foreach (Transform item in _trackerParent)
            Destroy(item.gameObject);
        _currentTrackers.Clear();
    }

    void SwitchSortType(SortType type)
    {
        ClearTrackers();

        if(type != _currentSortType)
            _currentSortType = type;
    }
}
