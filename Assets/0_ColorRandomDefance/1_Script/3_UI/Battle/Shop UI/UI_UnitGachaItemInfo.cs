using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_UnitGachaItemInfo : UI_Base
{
    [SerializeField] Sprite[] sprites;
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
        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    public void ShowInfo(UnitClass unitClass, double rate)
    {
        CheckInit();
        GetImage((int)Images.UnitIcon).sprite = sprites[(int)unitClass];
        GetTextMeshPro((int)Texts.RateText).text = rate.ToString();
    }
}
