using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_UnitOperater : UI_Base
{
    enum Buttons
    {
        OperateControlButton,
    }

    enum GameObjects
    {
        UnitIconsParent,
        OperateImpossibleText,
    }

    void Awake()
    {
        Bind<Button>(typeof(Buttons));
    }

    protected override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        BindOperateEvent(ToggleUnitIcons);
        HideIcons();
    }
    
    public void BindOperateEvent(UnityAction action) => GetButton((int)Buttons.OperateControlButton).onClick.AddListener(action);

    IUnitOperationHandler _operationHandler;
    WorldUnitManager _worldUnitManager;
    public bool IsActiveIcons { get; private set; }
    public void DependencyInject(IUnitOperationHandler operationHandler, WorldUnitManager worldUnitManager)
    {
        _operationHandler = operationHandler;
        _worldUnitManager = worldUnitManager;
    }

    void ToggleUnitIcons()
    {
        if(IsActiveIcons)
            HideIcons(); 
        else
            ShowOperableUnits();
    }

    public void HideIcons()
    {
        GetObject((int)GameObjects.OperateImpossibleText).SetActive(false);
        GetObject((int)GameObjects.UnitIconsParent).SetActive(false);
        IsActiveIcons = false;
    }

    public void ShowOperableUnits()
    {
        GetObject((int)GameObjects.UnitIconsParent).SetActive(true);
        IsActiveIcons = true;
        foreach (Transform child in GetObject((int)GameObjects.UnitIconsParent).transform)
            Managers.Resources.Destroy(child.gameObject);

        var combineableUnitFalgs = _operationHandler.GetOperableUnits(_worldUnitManager.GetUnitFlags(PlayerIdManager.Id));
        if (combineableUnitFalgs.Count() > 0)
        {
            GetObject((int)GameObjects.OperateImpossibleText).SetActive(false);
            foreach (var unitFlag in SortUnitFlags(combineableUnitFalgs))
            {
                var icon = Managers.UI.MakeSubItem<UI_UnitIcon>(GetObject((int)GameObjects.UnitIconsParent).transform);
                icon.SetUnitIcon(unitFlag);
                icon.BindClickEvent(Do);
                icon.BindClickEvent(ShowOperableUnits);

                void Do() => _operationHandler.Do(unitFlag);
            }
        }
        else
            GetObject((int)GameObjects.OperateImpossibleText).SetActive(true);
    }

    IEnumerable<UnitFlags> SortUnitFlags(IEnumerable<UnitFlags> flags)
        => flags
            .Where(x => UnitFlags.NormalFlags.Contains(x))
            .OrderByDescending(x => x.ClassNumber)
            .ThenByDescending(x => x.ColorNumber);
}
