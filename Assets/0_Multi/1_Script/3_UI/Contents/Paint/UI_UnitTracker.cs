using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitTracker : UI_Base
{
    enum Images
    {
        BackGround,
        Icon,
    }

    [SerializeField] UnitFlags unitFlags;
    public UnitFlags UnitFlags => unitFlags;
    [SerializeField] Text countText;
    [SerializeField] string _unitClassName;
    UnitTrakerDataModel _dataModel;

    void Awake()
    {
        countText = GetComponentInChildren<Text>();
    }

    protected override void Init()
    {
        GetComponentInChildren<Button>().onClick.AddListener(OnClicked);
        _dataModel = GetComponentInParent<UnitTrakerDataModel>();

        Bind<Image>(typeof(Images));
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

    //  여기 아래에 함수들로 리팩터링하면 됨. 필드가 null인지 아닌지로 구분하는 병신같은 코드 짜놔서 일단 빤스런함
    public void SetInfo(UnitFlags flag)
    {
        gameObject.SetActive(false);
        ApplyData(flag);
        gameObject.SetActive(true); // OnEnalbe() 실행
    }

    void ApplyData(UnitFlags flag)
    {
        if (_initDone == false)
        {
            Init();
            _initDone = true;
        }
        unitFlags = flag;
        var data = _dataModel.BuildUnitTrackerData(unitFlags);
        GetImage((int)Images.BackGround).color = data.BackGroundColor;
        GetImage((int)Images.Icon).sprite = data.Icon;
        _unitClassName = data.UnitClassName;
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
