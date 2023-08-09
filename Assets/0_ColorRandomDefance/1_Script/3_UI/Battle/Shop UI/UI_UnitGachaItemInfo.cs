using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_UnitGachaItemInfo : UI_Base
{
    enum Images
    {
        UnitIcon,
    }

    enum Texts
    {
        RateText,
    }

    protected override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshPro>(typeof(Texts));
    }

    public void ShowInfo(UnitClass unitClass, double rate)
    {
        GetImage((int)Images.UnitIcon).sprite = null;
        GetTextMeshPro((int)Texts.RateText).text = rate.ToString();
    }
}
