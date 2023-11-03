using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_UnitTracker : UI_Base
{
    enum Images
    {
        BackGround,
        Icon,
    }

    [SerializeField] UnitFlags unitFlags;
    public UnitFlags UnitFlags => unitFlags;
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] string _unitClassName;
    UnitTrakerDataModel _dataModel;

    void Awake()
    {
        countText = GetComponentInChildren<TextMeshProUGUI>();
    }

    protected override void Init()
    {
        GetComponentInChildren<Button>().onClick.AddListener(OnClicked);
        _dataModel = GetComponentInParent<UnitTrakerDataModel>();

        Bind<Image>(typeof(Images));
    }

    void OnEnable()
    {
        Managers.Unit.OnUnitCountChangeByFlag -= TrackUnitCount;
        Managers.Unit.OnUnitCountChangeByFlag += TrackUnitCount;
        SetUnitCountText(Managers.Unit.GetUnitCount(unitFlags));
    }

    void OnDisable()
    {
        if(Application.isPlaying && Managers.Unit != null)
            Managers.Unit.OnUnitCountChangeByFlag -= TrackUnitCount;
    }

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
        Managers.UI.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow").Show(UnitFlags);
        Managers.Sound.PlayEffect(EffectSoundType.ShowRandomShop);
    }
}
