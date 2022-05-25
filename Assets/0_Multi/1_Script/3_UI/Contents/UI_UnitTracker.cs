using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitTracker : Multi_UI_Base
{
    [SerializeField] Multi_UI_Paint paint;
    [SerializeField] Image backGround;
    [SerializeField] Image icon;
    [SerializeField] Text countText;

    [SerializeField] UnitFlags unitFlags;

    void Awake()
    {
        backGround = GetComponent<Image>();
        paint = GetComponentInParent<Multi_UI_Paint>();
    }

    protected override void Init()
    {
        GetComponentInChildren<Button>().onClick.AddListener(OnClicked);
    }

    public void SetInfoByColor(UI_UnitTrackerData data)
    {
        unitFlags = new UnitFlags(data.UnitFlags.ColorNumber, unitFlags.ClassNumber);
        backGround.color = data.BackGroundColor;
    }

    public void SetInfoByClass(UI_UnitTrackerData data)
    {
        unitFlags = new UnitFlags(unitFlags.ColorNumber, data.UnitFlags.ClassNumber);
        icon.sprite = data.Icon;
    }

    void OnClicked()
    {
        paint.ShowUnitManagedWindow(unitFlags);
    }
}
