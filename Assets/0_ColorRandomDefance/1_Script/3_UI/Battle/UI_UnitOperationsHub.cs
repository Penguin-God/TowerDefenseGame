using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnitOperationsHub : UI_Base
{
    enum GameObjects
    {
        Seller,
        WroldMover,
        TowerMover,
        Combiner,
    }

    protected override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        foreach (GameObjects type in Enum.GetValues(typeof(GameObjects)))
        {
            var operater = GetObject((int)type).GetComponent<UI_UnitOperater>();
            operater.DependencyInject(CreateOperater(type), _unitManagerController.WorldUnitManager);
            operater.BindOperateClickEvent(() => ChangeCurrnetOperater(operater));
        }

        _dispatcher.OnUnitCountChange += _ => UpdateOperater();
    }

    UnitManagerController _unitManagerController;
    UnitCombineMultiController _combineController;
    BattleEventDispatcher _dispatcher;
    public void DependencyInject(UnitManagerController unitManagerController, UnitCombineMultiController combineController, BattleEventDispatcher dispatcher)
    {
        _unitManagerController = unitManagerController;
        _combineController = combineController;
        _dispatcher = dispatcher;
    }

    IUnitOperationHandler CreateOperater(GameObjects gameObjects)
    {
        switch (gameObjects)
        {
            case GameObjects.Seller: return new UnitSellHandler(_unitManagerController);
            case GameObjects.WroldMover: return new UnitWolrdMoveHandler(_unitManagerController);
            case GameObjects.TowerMover: return new UnitTowerMoveHandler(_unitManagerController);
            case GameObjects.Combiner: return new UnitCombineHandler(_combineController);
            default: return null;
        }
    }

    UI_UnitOperater _currentOperater;
    void ChangeCurrnetOperater(UI_UnitOperater operater)
    {
        if(_currentOperater == operater) return;
        _currentOperater?.HideIcons();
        _currentOperater = operater;
    }

    void UpdateOperater() => _currentOperater?.UpdateOperableUnits();
}
