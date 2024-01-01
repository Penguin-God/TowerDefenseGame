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

        BindOperateClickEvent(ToggleUnitIcons);
        HideIcons();
    }
    
    public void BindOperateClickEvent(UnityAction action) => GetButton((int)Buttons.OperateControlButton).onClick.AddListener(action);

    IUnitOperationHandler _operationHandler;
    WorldUnitManager _worldUnitManager;
    bool _isActiveIcons;
    public void DependencyInject(IUnitOperationHandler operationHandler, WorldUnitManager worldUnitManager)
    {
        _operationHandler = operationHandler;
        _worldUnitManager = worldUnitManager;
    }

    void ToggleUnitIcons()
    {
        if(_isActiveIcons)
            HideIcons(); 
        else
            ShowOperableUnits();
    }

    public void HideIcons()
    {
        GetObject((int)GameObjects.OperateImpossibleText).SetActive(false);
        GetObject((int)GameObjects.UnitIconsParent).SetActive(false);
        _isActiveIcons = false;
    }

    public void ShowOperableUnits()
    {
        GetObject((int)GameObjects.UnitIconsParent).SetActive(true);
        _isActiveIcons = true;
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
                icon.BindClickEvent(UpdateOperableUnits);

                void Do() => _operationHandler.Do(unitFlag);
            }
        }
        else
            GetObject((int)GameObjects.OperateImpossibleText).SetActive(true);
    }

    public void UpdateOperableUnits()
    {
        if (_isActiveIcons == false) return;
        StartCoroutine(Co_UpdateOperableUnits());
    }
    IEnumerator Co_UpdateOperableUnits()
    {
        HideIcons();
        yield return new WaitForSeconds(0.2f);
        ShowOperableUnits();
    }

    IEnumerable<UnitFlags> SortUnitFlags(IEnumerable<UnitFlags> flags)
        => flags
            .Where(x => UnitFlags.NormalFlags.Contains(x))
            .OrderByDescending(x => x.ClassNumber)
            .ThenByDescending(x => x.ColorNumber);
}
