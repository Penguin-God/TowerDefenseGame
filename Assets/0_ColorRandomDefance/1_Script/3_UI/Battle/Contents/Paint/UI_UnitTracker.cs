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
    TextMeshProUGUI _countText;
    UI_UnitIcon _unitIcon;
    
    void Awake()
    {
        _countText = GetComponentInChildren<TextMeshProUGUI>();
        _unitIcon = GetComponentInChildren<UI_UnitIcon>();
    }

    protected override void Init()
    {
        GetComponentInChildren<Button>().onClick.AddListener(OnClicked);

        Bind<Image>(typeof(Images));
    }

    WorldUnitManager _worldUnitManager;
    public void SetInfo(UnitFlags flag, WorldUnitManager worldUnitManager)
    {
        if (_initDone == false)
        {
            Init();
            _initDone = true;
        }

        _worldUnitManager = worldUnitManager;
        ApplyData(flag);
    }

    void ApplyData(UnitFlags flag)
    {
        unitFlags = flag;
        _unitIcon.SetUnitIcon(unitFlags);
        UpdateUnitCountText();
    }

    public void UpdateUnitCountText() => UpdateUnitCountText(_worldUnitManager.GetUnitCount(PlayerIdManager.Id, unit => unit.UnitFlags == unitFlags));
    public void UpdateUnitCountText(int count) => _countText.text = $"{UnitTextPresenter.GetClassText(UnitFlags.UnitClass)} : {count}";

    void OnClicked()
    {
        Managers.UI.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow").Show(UnitFlags);
        Managers.Sound.PlayEffect(EffectSoundType.ShowRandomShop);
    }
}
