using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitTracker : UI_Base
{
    [SerializeField] UnitFlags unitFlags;
    [SerializeField] Image backGround;
    [SerializeField] Image icon;
    [SerializeField] Text countText;
    [SerializeField] string _unitClassName;
    UnitTrakerDataModel _dataModel;
    void Awake()
    {
        backGround = GetComponent<Image>();
        countText = GetComponentInChildren<Text>();
        _dataModel = GetComponentInParent<UnitTrakerDataModel>();
    }

    protected override void Init()
    {
        GetComponentInChildren<Button>().onClick.AddListener(OnClicked);
    }

    void OnEnable()
    {
        Multi_UnitManager.Instance.OnUnitFlagCountChanged -= TrackUnitCount;
        Multi_UnitManager.Instance.OnUnitFlagCountChanged += TrackUnitCount;
        SetUnitCountText(Multi_UnitManager.Instance.UnitCountByFlag[unitFlags]);
    }

    void OnDisable()
    {
        if(Multi_UnitManager.Instance != null)
            Multi_UnitManager.Instance.OnUnitFlagCountChanged -= TrackUnitCount;
    }

    public void SetInfo(UI_UnitTrackerData data)
    {
        gameObject.SetActive(false);

        // TODO : 코드 꼬라지...... 고쳐야겠지?
        unitFlags = BuildUnitFlags(data.UnitFlags);
        if (data.BackGroundColor != Color.black) backGround.color = data.BackGroundColor;
        if (data.Icon != null) icon.sprite = data.Icon;
        if (string.IsNullOrEmpty(data.UnitClassName) == false) _unitClassName = data.UnitClassName;
        gameObject.SetActive(true); // OnEnalbe() 실행

        UnitFlags BuildUnitFlags(UnitFlags flag)
        {
            int colorNumber = flag.ColorNumber == -1 ? unitFlags.ColorNumber : flag.ColorNumber;
            int classNumber = flag.ClassNumber == -1 ? unitFlags.ClassNumber : flag.ClassNumber;
            return new UnitFlags(colorNumber, classNumber);
        }
    }

    //  여기 아래에 함수들로 리팩터링하면 됨. 필드가 null인지 아닌지로 구분하는 병신같은 코드 짜놔서 일단 빤스런함
    public void SetInfo(UnitFlags flag)
    {
        gameObject.SetActive(false);
        ApplyData(flag);
        gameObject.SetActive(true); // OnEnalbe() 실행
    }

    void ApplyData(UnitFlags flag)
    {
        unitFlags = BuildUnitFlags(flag);
        var data = _dataModel.BuildUnitTrackerData(unitFlags);
        backGround.color = data.BackGroundColor;
        icon.sprite = data.Icon;
        _unitClassName = data.UnitClassName;
    }

    UnitFlags BuildUnitFlags(UnitFlags flag)
    {
        int colorNumber = flag.ColorNumber == -1 ? transform.GetSiblingIndex() : flag.ColorNumber;
        int classNumber = flag.ClassNumber == -1 ? transform.GetSiblingIndex() : flag.ClassNumber;
        return new UnitFlags(colorNumber, classNumber);
    }

    void TrackUnitCount(UnitFlags unitFlag, int count)
    {
        if (unitFlag == unitFlags)
            SetUnitCountText(count);
    }
    void SetUnitCountText(int count) => countText.text = $"{_unitClassName} : {count}";

    void OnClicked()
    {
        Managers.UI.ShowPopGroupUI<UI_UnitManagedWindow>(PopupGroupType.UnitWindow, "UnitManagedWindow").Show(unitFlags);
        Managers.Sound.PlayEffect(EffectSoundType.ShowRandomShop);
    }
}
