using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitTrackerSetter : Multi_UI_Base
{
    [SerializeField] UI_UnitTrackerData _unitTrackerData;
    public UI_UnitTrackerData UnitTrackerData => _unitTrackerData;

    protected override void Init()
    {
        _unitTrackerData = new UI_UnitTrackerData(_unitTrackerData.UnitFlags, null, GetComponent<Image>().color);

        //GetComponentInParent<Multi_UI_Paint>().OnPaintChanged +=
        //GetComponent<Button>().onClick.AddListener(() => )
    }

    void SetTracker(UI_UnitTrackerData unitTrackerData)
    {

    }
}
