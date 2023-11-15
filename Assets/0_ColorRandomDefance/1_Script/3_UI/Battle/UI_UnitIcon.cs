using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitIcon : UI_Base
{
    enum Images
    {
        BackGround,
        Icon,
    }
    protected override void Init()
    {
        Bind<Image>(typeof(Images));
    }

    public void SetUnitIcon(UnitFlags flag)
    {
        CheckInit();
        GetImage((int)Images.BackGround).color = SpriteUtility.GetUnitColor(flag.UnitColor);
        GetImage((int)Images.Icon).sprite = SpriteUtility.GetUnitClassIcon(flag.UnitClass);
    }

    public void BindClickEvent(Action action) => BindEvnet(GetImage((int)Images.BackGround).gameObject, _ => action());
}
