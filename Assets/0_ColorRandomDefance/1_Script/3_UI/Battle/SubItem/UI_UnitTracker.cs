using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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

    protected override void Init()
    {
        // GetComponentInChildren<Button>().onClick.AddListener(ShowUnitWindow);

        Bind<Image>(typeof(Images));
        _countText = GetComponentInChildren<TextMeshProUGUI>();
        _unitIcon = GetComponentInChildren<UI_UnitIcon>();
    }

    WorldUnitManager _worldUnitManager;
    public void SetInfo(UnitFlags flag, WorldUnitManager worldUnitManager)
    {
        CheckInit();

        unitFlags = flag;
        _worldUnitManager = worldUnitManager;

        _unitIcon.SetUnitIcon(unitFlags);
        UpdateUnitCountText();
    }

    public void UpdateUnitCountText() => UpdateUnitCountText(_worldUnitManager.GetUnitCount(PlayerIdManager.Id, unit => unit.UnitFlags == unitFlags));
    public void UpdateUnitCountText(int count) => _countText.text = $"{UnitTextPresenter.GetClassText(UnitFlags.UnitClass)} : {count}";

    //void ShowUnitWindow()
    //{
    //    if (UnitFlags.SpecialColors.Contains(unitFlags.UnitColor)) return;

    //    Managers.UI.ClosePopupUI();
    //    Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow").Show(UnitFlags);
    //    Managers.Sound.PlayEffect(EffectSoundType.ShowRandomShop);
    //}
}
