using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitTracker : Multi_UI_Base
{
    Multi_UI_Paint paint;
    [SerializeField] UnitFlags unitFlags;
    [SerializeField] Image backGround;
    [SerializeField] Image icon;
    [SerializeField] Text countText;

    public Image BackGround { get => backGround; set => backGround = value;}
    public Image Icon { get => icon; set => icon = value; }
    
    
    public UnitFlags UnitFlags { get => unitFlags; set => unitFlags = value; }

    void Awake()
    {
        backGround = GetComponent<Image>();
        paint = GetComponentInParent<Multi_UI_Paint>();
    }

    protected override void Init()
    {
        GetComponentInChildren<Button>().onClick.AddListener(OnClicked);
    }

    public void SetInfo(UI_UnitTrackerData data)
    {
        unitFlags = new UnitFlags(data.UnitFlags.ColorNumber, unitFlags.ClassNumber);
        if (data.BackGroundColor != Color.black) backGround.color = data.BackGroundColor;
        if (data.Icon != null) icon.sprite = data.Icon;
    }

    public void SetInfoByColor(UI_UnitTrackerData data)
    {
        unitFlags = new UnitFlags(data.UnitFlags.ColorNumber, unitFlags.ClassNumber);
        if (data.BackGroundColor != Color.white) backGround.color = data.BackGroundColor;
        if (data.Icon != null) icon.sprite = data.Icon;
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
