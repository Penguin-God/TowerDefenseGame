using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ColseButton : UI_Base
{
    [SerializeField] GameObject colseObj;

    protected override void Init()
    {
        GetComponent<Button>().onClick.AddListener(ColseUI);        
    }

    void ColseUI()
    {
        colseObj?.SetActive(false);
        Managers.Sound.PlayEffect(EffectSoundType.Click_XButton);
    }
}
