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
        if(Multi_UnitManager.Instance != null)
            Multi_UnitManager.Instance.OnUnitFlagCountChanged -= TrackUnitCount;
    }

    public void SetInfo(UI_UnitTrackerData data)
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        // TODO : 코드 꼬라지...... 고쳐야겠지?
        int colorNumber = data.UnitFlags.ColorNumber == -1 ? unitFlags.ColorNumber : data.UnitFlags.ColorNumber;
        int classNumber = data.UnitFlags.ClassNumber == -1 ? unitFlags.ClassNumber : data.UnitFlags.ClassNumber;
        unitFlags = new UnitFlags(colorNumber, classNumber);
        if (data.BackGroundColor != Color.black) backGround.color = data.BackGroundColor;
        if (data.Icon != null) icon.sprite = data.Icon;
        if (string.IsNullOrEmpty(data.UnitClassName) == false) _unitClassName = data.UnitClassName;

        Multi_UnitManager.Instance.OnUnitFlagCountChanged -= TrackUnitCount;
        Multi_UnitManager.Instance.OnUnitFlagCountChanged += TrackUnitCount;
        SetUnitCountText(Multi_UnitManager.Instance.UnitCountByFlag[unitFlags]);
    }

    void TrackUnitCount(UnitFlags unitFlag, int count)
    {
        if (unitFlag == unitFlags)
            SetUnitCountText(count);
    }
    void SetUnitCountText(int count) => countText.text = $"{_unitClassName} : {count}";

    void OnClicked()
    {
        Multi_Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow", PopupGroupType.UnitWindow).Show(unitFlags);
        Multi_Managers.Sound.PlayEffect(EffectSoundType.ShowRandomShop);
    }
}
