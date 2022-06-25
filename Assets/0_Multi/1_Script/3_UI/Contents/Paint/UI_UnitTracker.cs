using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitTracker : Multi_UI_Base
{
    [SerializeField] UnitFlags unitFlags;
    [SerializeField] Image backGround;
    [SerializeField] Image icon;
    [SerializeField] Text countText;
    [SerializeField] string _unitClassName;
    //public Image BackGround { get => backGround; set => backGround = value;}
    //public Image Icon { get => icon; set => icon = value; }
    //public UnitFlags UnitFlags { get => unitFlags; set => unitFlags = value; }

    void Awake()
    {
        backGround = GetComponent<Image>();
        countText = GetComponentInChildren<Text>();
    }

    protected override void Init()
    {
        GetComponentInChildren<Button>().onClick.AddListener(OnClicked);
    }

    void OnDisable()
    {
        Multi_UnitManager.Instance.OnUnitFlagDictChanged -= TrackUnitCount;
        print("잘 빠짐");
    }

    public void SetInfo(UI_UnitTrackerData data)
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        unitFlags = new UnitFlags(data.UnitFlags.ColorNumber, unitFlags.ClassNumber);
        if (data.BackGroundColor != Color.black) backGround.color = data.BackGroundColor;
        if (data.Icon != null) icon.sprite = data.Icon;
        if (string.IsNullOrEmpty(data.UnitClassName) == false) _unitClassName = data.UnitClassName;

        SetUnitCountText(Multi_UnitManager.Instance.GetUnitFlagCount(unitFlags));
        Multi_UnitManager.Instance.OnUnitFlagDictChanged -= TrackUnitCount;
        Multi_UnitManager.Instance.OnUnitFlagDictChanged += TrackUnitCount;
    }

    void TrackUnitCount(UnitFlags unitFlag, int count)
    {
        if (unitFlag == unitFlags)
            SetUnitCountText(count);
    }
    void SetUnitCountText(int count) => countText.text = $"{_unitClassName} : {count}";

    void OnClicked() => Multi_Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("Paint/UnitManagedWindow").Show(unitFlags);
}
