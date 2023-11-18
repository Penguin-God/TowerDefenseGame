using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitOperater : UI_Base
{
    enum Buttons
    {
        OperateButton,
    }

    enum GameObjects
    {
        UnitIconsParent,
        OperateImpossibleText,
    }

    protected override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        GetButton((int)Buttons.OperateButton).onClick.AddListener(ShowOperableUnits);
    }

    IUnitOperationHandler _operationHandler;
    WorldUnitManager _worldUnitManager;
    public void DependencyInject(IUnitOperationHandler operationHandler, WorldUnitManager worldUnitManager)
    {
        _operationHandler = operationHandler;
        _worldUnitManager = worldUnitManager;
    }

    public void ShowOperableUnits()
    {
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
            .ThenByDescending(x => x.ColorNumber)
            .Reverse();

    public void HideIcons()
    {
        GetObject((int)GameObjects.OperateImpossibleText).SetActive(false);
        GetObject((int)GameObjects.UnitIconsParent).SetActive(false);
    }
}
