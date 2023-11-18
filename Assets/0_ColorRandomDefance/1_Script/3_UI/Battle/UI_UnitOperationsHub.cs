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


    }

    UI_UnitOperater _currentOperater;


}
