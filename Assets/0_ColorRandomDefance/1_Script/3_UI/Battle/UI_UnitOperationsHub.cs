using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnitOperationsHub : UI_Base
{
    enum GameObjects
    {

    }

    protected override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));    
    }
}
