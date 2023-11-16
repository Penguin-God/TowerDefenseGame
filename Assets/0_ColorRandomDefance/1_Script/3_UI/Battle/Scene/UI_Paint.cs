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
    BattleEventDispatcher _dispatcher;
    WorldUnitManager _worldUnitManager;
    public void DependencyInject(UnitStatController unitStatController, BattleEventDispatcher dispatcher, WorldUnitManager worldUnitManager)
    {
        _unitStatController = unitStatController;
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
        GetObject((int)GameObjects.PaintBackGround).SetActive(false);

        foreach (var unitClass in UnitFlags.AllClass)
        {
            var tracker = CreateTracker(new UnitFlags(UnitColor.Black, unitClass));
            tracker.UpdateUnitCountText(_worldUnitManager.GetUnitCount(PlayerIdManager.Id, unit => unit.UnitFlags.UnitClass == tracker.UnitFlags.UnitClass));
            tracker.GetComponent<Button>().onClick.AddListener(() => SortByClass(unitClass));
            new UnitJobTooltipController().SetMouseOverAction(tracker);
        }
    }


    public void SortByClass(UnitClass unitClass)
    {
        SwitchSortType(SortType.Class);
        GetObject((int)GameObjects.PaintBackGround).SetActive(true);
        _layoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        _layoutGroup.constraintCount = 3;

        foreach (var unitColor in UnitFlags.NormalColors)
        {
            var tracker = CreateTracker(new UnitFlags(unitColor, unitClass));
            tracker.GetComponent<Button>().onClick.AddListener(() => ShowUnitControlButtons(tracker));
        }
    }

    public void ClickTracker(UnitFlags flag) => ShowUnitControlButtons(_currentTrackers.FirstOrDefault(x => x.UnitFlags == flag));

    void ShowUnitControlButtons(UI_UnitTracker tracker)
    {
        if (tracker == null) return;

        Managers.UI.ClosePopupUI();
        var buttons = Managers.UI.ShowPopupUI<UI_UnitContolWindow>();
        buttons.SetButtonAction(tracker.UnitFlags);
        float screenWidthScaleFactor = Screen.width / Managers.UI.UIScreenWidth; // 플레이어 스크린 크기 대비 설정한 UI 비율
        buttons.SetPositioin(tracker.GetComponent<RectTransform>().position + new Vector3(80f * screenWidthScaleFactor, 0, 0));
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
