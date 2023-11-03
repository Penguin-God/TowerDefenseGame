using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Paint : UI_Scene
{
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
    }

    [SerializeField] GameObject _currentUnitTracker;
    public GameObject CurrentUnitTracker { get => _currentUnitTracker; set => _currentUnitTracker = value; }

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
        GetButton((int)Buttons.ClassButton).onClick.AddListener(ShowClassButtons);

        ClearTarckers();

        foreach (Transform item in GetObject((int)GameObjects.ColorButtons).transform)
            item.GetComponent<Button>().onClick.AddListener(InActiveColorButtons);

        for (int i = 0; i < UnitFlags.NormalColors.Count(); i++)
            SetSortAction(GameObjects.ColorButtons, i, SortByColor);
        for (int i = 0; i < UnitFlags.AllClass.Count(); i++)
            SetSortAction(GameObjects.UnitByDefault, i, SortByClass);
    }

    void ShowClassButtons()
    {
        ClearTarckers();
        GetObject((int)GameObjects.UnitByDefault).SetActive(true);
    }

    void ClearTarckers()
    {
        foreach (Transform item in _trackerParent)
            Destroy(item.gameObject);
    }

    void SortTracker()
    {
        ClearTarckers();
        GetObject((int)GameObjects.UnitByDefault).gameObject.SetActive(false);
    }

    void SetSortAction(GameObjects ojects, int childIndex, UnityAction<int> action)
    {
        GetObject((int)ojects).transform.GetChild(childIndex).GetComponent<Button>().onClick.AddListener(() => action(childIndex));
    }

    void InActiveColorButtons() => GetObject((int)GameObjects.ColorButtons).SetActive(false);

    void ChangePaintRootActive()
    {
        GetObject((int)GameObjects.ColorButtons).SetActive(!GetObject((int)GameObjects.ColorButtons).activeSelf);
        Managers.Sound.PlayEffect(EffectSoundType.PopSound_2);
    }

    void SortByColor(int colorNumber)
    {
        _layoutGroup.constraint = GridLayoutGroup.Constraint.Flexible;
        _layoutGroup.padding.top = 0;
        SortTracker();

        foreach (var unitClass in UnitFlags.AllClass)
            Managers.UI.MakeSubItem<UI_UnitTracker>(_trackerParent).SetInfo(new UnitFlags((UnitColor)colorNumber, unitClass));
    }

    void SortByClass(int classNumber)
    {
        _layoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        _layoutGroup.constraintCount = 3;
        _layoutGroup.padding.top = 70;
        SortTracker();

        foreach (var unitColor in UnitFlags.NormalColors)
            Managers.UI.MakeSubItem<UI_UnitTracker>(_trackerParent).SetInfo(new UnitFlags(unitColor, (UnitClass)classNumber));
    }
}
