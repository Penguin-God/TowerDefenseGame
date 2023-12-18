using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillInfoWindow : UI_Popup
{
    enum Images
    {
        Skill_Image,
        FillMask,
    }

    enum Texts
    {
        SkillName,
        SkillExplaneText,
        Exp_Text,
    }

    enum Buttons
    {
        UpgradeButton,
    }

    enum GameObjects
    {
        SkillStatInfoRoot,
    }

    protected override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        _initDone = true;
        RefreshUI();
    }

    SkillUpgradeUseCase _skillUpgradeUseCase;
    PlayerDataManager _playerDataManager;
    public void SetInfo(UserSkillGoodsData newData, SkillUpgradeUseCase skillUpgradeUseCase, PlayerDataManager playerDataManager)
    {
        _skillData = newData;
        _skillUpgradeUseCase = skillUpgradeUseCase;
        _playerDataManager = playerDataManager;
        RefreshUI();
    }

    UserSkillGoodsData _skillData = null;
    public void RefreshUI()
    {
        if (_initDone == false || _skillData == null) return;

        GetTextMeshPro((int)Texts.SkillName).text = _skillData.SkillName;
        GetTextMeshPro((int)Texts.SkillExplaneText).text = _skillData.Description;
        GetImage((int)Images.Skill_Image).sprite = SpriteUtility.GetSkillImage(_skillData.SkillType);

        var levelData = Managers.Data.UserSkill.GetSkillLevelData(_skillData.SkillType, _playerDataManager.SkillInventroy.GetSkillInfo(_skillData.SkillType).Level);
        GetTextMeshPro((int)Texts.Exp_Text).text = $"{_playerDataManager.SkillInventroy.GetSkillInfo(_skillData.SkillType).HasAmount} / {levelData.Exp}";
        GetImage((int)Images.FillMask).fillAmount = (float)_playerDataManager.SkillInventroy.GetSkillInfo(_skillData.SkillType).HasAmount / levelData.Exp;

        GetButton((int)Buttons.UpgradeButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.UpgradeButton).onClick.AddListener(UpgradeSkill);
        ShowSkillStat();
    }

    void UpgradeSkill()
    {
        if (_skillData == null) return;

        if (_skillUpgradeUseCase.CanUpgrade(_skillData.SkillType))
        {
            _skillUpgradeUseCase.Upgrade(_skillData.SkillType);
            RefreshUI();
        }
    }

    void ShowSkillStat()
    {
        foreach (Transform child in GetObject((int)GameObjects.SkillStatInfoRoot).transform)
            Destroy(child.gameObject);

        if (_skillData.StatInfoFraems == null || _skillData.StatInfoFraems.Count() == 0) return;
        for (int i = 0; i < _skillData.StatInfoFraems.Count(); i++)
            Managers.UI.MakeSubItem<UI_SkillStatInfo>(GetObject((int)GameObjects.SkillStatInfoRoot).transform).ShowSkillStat(_skillData.SkillType, i);
    }
}
