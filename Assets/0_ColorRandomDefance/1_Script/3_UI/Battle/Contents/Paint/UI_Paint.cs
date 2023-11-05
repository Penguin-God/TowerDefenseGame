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
        UnitByDefault,
    }

    enum Buttons
    {
        ClassButton,
        CombineableButton,
    }

    [SerializeField] GameObject _currentUnitTracker;
    public GameObject CurrentUnitTracker { get => _currentUnitTracker; set => _currentUnitTracker = value; }

    Transform _trackerParent;
    GridLayoutGroup _layoutGroup;
    UnitTrakerSortByCombineable _combineSorter;
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
        for (int i = 0; i < UnitFlags.AllClass.Count(); i++)
            SetSortAction(GameObjects.UnitByDefault, i, SortByClass);

        _combineSorter = GetComponentInChildren<UnitTrakerSortByCombineable>();
        GetButton((int)Buttons.CombineableButton).onClick.AddListener(SortByCombineable);

        UpdateUI(SortType.Default);
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

    public void SortByColor(int colorNumber)
    {
        _layoutGroup.constraint = GridLayoutGroup.Constraint.Flexible;
        _layoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
        _layoutGroup.padding.top = 0;
        UpdateUI(SortType.Color);

        foreach (var unitClass in UnitFlags.AllClass)
            Managers.UI.MakeSubItem<UI_UnitTracker>(_trackerParent).SetInfo(new UnitFlags((UnitColor)colorNumber, unitClass));
    }

    void SortByClass(int classNumber)
    {
        _layoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        _layoutGroup.constraintCount = 3;
        _layoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
        _layoutGroup.padding.top = 70;
        UpdateUI(SortType.Class);

        foreach (var unitColor in UnitFlags.NormalColors)
            Managers.UI.MakeSubItem<UI_UnitTracker>(_trackerParent).SetInfo(new UnitFlags(unitColor, (UnitClass)classNumber));
    }

    void UpdateUI(SortType type)
    {
        foreach (Transform item in _trackerParent)
            Destroy(item.gameObject);

        GetObject((int)GameObjects.ColorButtons).SetActive(false);
        _combineSorter.enabled = false;
        GetObject((int)GameObjects.UnitByDefault).gameObject.SetActive(false);
        switch (type)
        {
            case SortType.Default: GetObject((int)GameObjects.UnitByDefault).gameObject.SetActive(true); break;
            case SortType.Color: break;
            case SortType.Class: break;
            case SortType.Combineable: _layoutGroup.startCorner = GridLayoutGroup.Corner.LowerLeft; _layoutGroup.padding.top = 230; _combineSorter.enabled = true; break;
        }
    }
}
