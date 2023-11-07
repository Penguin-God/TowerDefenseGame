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
    
    void Awake()
    {
        countText = GetComponentInChildren<TextMeshProUGUI>();
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
        GetImage((int)Images.BackGround).color = SpriteUtility.GetUnitColor(unitFlags.UnitColor);
        GetImage((int)Images.Icon).sprite = SpriteUtility.GetUnitClassIcon(unitFlags.UnitClass);
        _unitClassName = UnitTextPresenter.GetClassText(UnitFlags.UnitClass);
        UpdateUnitCountText();
    }

    public void UpdateUnitCountText() => UpdateUnitCountText(_worldUnitManager.GetUnitCount(PlayerIdManager.Id, unit => unit.UnitFlags == unitFlags));
    public void UpdateUnitCountText(int count) => countText.text = $"{_unitClassName} : {count}";

    void OnClicked()
    {
        Managers.UI.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow").Show(UnitFlags);
        Managers.Sound.PlayEffect(EffectSoundType.ShowRandomShop);
    }
}
