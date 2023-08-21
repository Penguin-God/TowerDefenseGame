using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitCombineInfoItem : UI_Base
{
    [SerializeField] TextMeshProUGUI _combineRecipeText;
    [SerializeField] Button _combineButton;

    public void SetInfo(UnitFlags flag)
    {
        _combineRecipeText.text = Managers.Data.UnitNameDataByFlag[flag].KoearName;
        _combineButton.GetComponentInChildren<TextMeshProUGUI>().text = Managers.Data.UnitNameDataByFlag[flag].KoearName;
        _combineButton.onClick.RemoveAllListeners();
        _combineButton.onClick.AddListener(() => Managers.Unit.TryCombine(flag));
    }
}
