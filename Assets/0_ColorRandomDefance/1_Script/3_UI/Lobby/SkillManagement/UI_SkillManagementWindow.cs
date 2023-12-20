using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillManagementWindow : UI_Popup
{
    enum GameObjects
    {
        HasSkillFramesParent,
    }

    enum Buttons
    {
        UnEquipButton,
        MainTabBtn,
        SubTabBtn,
    }

    enum Images
    {
        EquipSkill1_Image,
        EquipSkill2_Image,
    }

    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.UnEquipButton).onClick.AddListener(_equipSkillManager.AllUnEquip);
        GetButton((int)Buttons.MainTabBtn).onClick.AddListener(() => ChangeTab(UserSkillClass.Main));
        GetButton((int)Buttons.SubTabBtn).onClick.AddListener(() => ChangeTab(UserSkillClass.Sub));

        _equipSkillManager.OnEquipSkillChanged -= DrawEquipSkillFrame;
        _equipSkillManager.OnEquipSkillChanged += DrawEquipSkillFrame;
    }

    SkillInventroy _skillInvertory;
    EquipSkillManager _equipSkillManager;
    SkillDataGetter _skillDataGetter;
    SkillUpgradeUseCase _skillUpgradeUseCase;
    public void DependencyInject(SkillInventroy skillInvertory, SkillDataGetter skillDataGetter, SkillUpgradeUseCase skillUpgradeUseCase, EquipSkillManager equipSkillManager)
    {
        _skillInvertory = skillInvertory;
        _skillDataGetter = skillDataGetter;
        _skillUpgradeUseCase = skillUpgradeUseCase;
        _equipSkillManager = equipSkillManager;
    }

    void OnDestroy()
    {
        _equipSkillManager.OnEquipSkillChanged -= DrawEquipSkillFrame;
    }

    public void RefreshUI()
    {
        CheckInit();

        ChangeTab(UserSkillClass.Main);
        RefreshEquipSkillFrame();
    }

    [SerializeField] Sprite _defualtSp;
    [SerializeField] Sprite _selectSp;
    void ChangeTab(UserSkillClass skillClass)
    {
        switch (skillClass)
        {
            case UserSkillClass.Main:
                GetButton((int)Buttons.MainTabBtn).image.sprite = _selectSp;
                GetButton((int)Buttons.SubTabBtn).image.sprite = _defualtSp;
                break;
            case UserSkillClass.Sub:
                GetButton((int)Buttons.MainTabBtn).image.sprite = _defualtSp;
                GetButton((int)Buttons.SubTabBtn).image.sprite = _selectSp;
                break;
        }

        DrawSkillImages(skillClass);
        // DrawSkillImages(_skillInvertory.GetAllHasSkills().Where(x => Managers.Data.UserSkill.GetSkillGoodsData(x).SkillClass == skillClass));
    }

    void DrawSkillImages(IEnumerable<SkillType> skills)
    {
        var frameParent = GetObject((int)GameObjects.HasSkillFramesParent).transform;
        foreach (Transform item in frameParent)
            Destroy(item.gameObject);

        foreach (SkillType skillType in skills)
            Managers.UI.MakeSubItem<UI_SkillFrame>(frameParent).SetInfo(new SkillInfoPresenter(skillType, _skillDataGetter), _skillUpgradeUseCase, _equipSkillManager);
    }

    void DrawSkillImages(UserSkillClass skillClass)
    {
        var frameParent = GetObject((int)GameObjects.HasSkillFramesParent).transform;
        foreach (Transform item in frameParent)
            Destroy(item.gameObject);

        new SkillViewr(frameParent.GetComponent<RectTransform>(), new Vector2(120, 200), _skillDataGetter, _skillUpgradeUseCase, _equipSkillManager).ViewSkills(skillClass, new SkillPresenter(_skillInvertory));
    }

    void RefreshEquipSkillFrame()
    {
        DrawEquipSkillFrame(UserSkillClass.Main, _equipSkillManager.MainSkill);
        DrawEquipSkillFrame(UserSkillClass.Sub, _equipSkillManager.SubSkill);
    }

    void DrawEquipSkillFrame(UserSkillClass skillClass, SkillType skillType)
    {
        switch (skillClass)
        {
            case UserSkillClass.Main:
                SetEquipImage(skillType, GetImage((int)Images.EquipSkill1_Image));
                break;
            case UserSkillClass.Sub:
                SetEquipImage(skillType, GetImage((int)Images.EquipSkill2_Image));
                break;
        }
    }

    void SetEquipImage(SkillType skillType, Image image)
    {
        if (skillType == SkillType.None)
        {
            image.color = new Color(1, 1, 1, 0);
            return;
        }

        image.color = new Color(1, 1, 1, 1);
        image.sprite = SpriteUtility.GetSkillImage(skillType);
    }
}

public class SkillPresenter
{
    readonly SkillInventroy _skillInventroy;
    readonly IEnumerable<UserSkill> UserSkillDatas = Managers.Resources.LoadCsv<UserSkillData>("SkillData/UserSkillData").Select(x => x.CreateUserSkill());
    public SkillPresenter(SkillInventroy skillInventroy)
    {
        _skillInventroy = skillInventroy;
    }
    
    public IEnumerable<SkillType> GetHasSkills(UserSkillClass skillClass) => GetSkills(skillClass, true);

    public IEnumerable<SkillType> GetDontHasSkills(UserSkillClass skillClass) => GetSkills(skillClass, false);

    public IEnumerable<SkillType> GetSkills(UserSkillClass skillClass, bool has) 
        => UserSkillDatas.Where(x => x.SkillClass == skillClass && _skillInventroy.HasSkill(x.SkillType) == has).Select(x => x.SkillType);
}

public class SkillViewr
{
    readonly RectTransform _content;
    readonly Vector2 ItemSize;
    readonly SkillDataGetter _skillDataGetter;
    readonly SkillUpgradeUseCase _skillUpgradeUseCase;
    readonly EquipSkillManager _equipSkillManager;

    float _currentY = 0f;
    int _itemLineCount;

    public SkillViewr(RectTransform content, Vector2 itemSize, SkillDataGetter skillDataGetter, SkillUpgradeUseCase skillUpgradeUseCase, EquipSkillManager equipSkillManager)
    {
        _content = content;
        ItemSize = itemSize;
        _skillDataGetter = skillDataGetter;
        _skillUpgradeUseCase = skillUpgradeUseCase;
        _equipSkillManager = equipSkillManager;
    }

    public void ViewSkills(UserSkillClass userSkillClass, SkillPresenter skillPresenter)
    {
        var hasSkills = skillPresenter.GetHasSkills(userSkillClass);
        AddLineCount(hasSkills);
        AddItems(hasSkills);
        if(hasSkills.Count() % 4 != 0) AddScroll(ItemSize.y);
        var divider = AddDivider();

        var dontHasSkills = skillPresenter.GetDontHasSkills(userSkillClass);
        AddLineCount(dontHasSkills);
        AddItems(dontHasSkills);
        if (dontHasSkills.Count() % 4 == 0) _itemLineCount--;
        _content.sizeDelta = new Vector2(_content.sizeDelta.x, _itemLineCount * ItemSize.y + divider.sizeDelta.y);
    }

    void AddLineCount(IEnumerable<SkillType> skills) => _itemLineCount += skills.Count() / 4 + 1;

    public void AddItems(IEnumerable<SkillType> skills)
    {
        float[] xpos = new float[] { 0, 160, 320, 480 };
        int xIndex = 0;
        foreach (SkillType skill in skills)
        {
            AddItem(skill, xpos[xIndex]);
            xIndex++;
            if(xIndex >= xpos.Length)
            {
                xIndex = 0;
                AddScroll(ItemSize.y);
            }
        }
    }

    void AddItem(SkillType skill, float xPos)
    {
        var item = Managers.UI.MakeSubItem<UI_SkillFrame>(_content);
        item.SetInfo(new SkillInfoPresenter(skill, _skillDataGetter), _skillUpgradeUseCase, _equipSkillManager);
        var rect = item.GetComponent<RectTransform>();
        rect.anchorMax = new Vector2(0, 1);
        rect.anchorMin = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.sizeDelta = ItemSize;
        rect.anchoredPosition = new Vector2(xPos, _currentY);
    }

    RectTransform AddDivider()
    {
        var divider = CreateItem(Managers.Resources.Load<GameObject>("Prefabs/UI/Divider")).GetComponent<RectTransform>();
        divider.anchoredPosition = new Vector2(0, _currentY);
        divider.anchorMax = new Vector2(0, 1);
        divider.anchorMin = new Vector2(0, 1);
        divider.pivot = new Vector2(0, 1);
        AddScroll(divider.sizeDelta.y);
        return divider;
    }
    void AddScroll(float y) => _currentY -= y;

    RectTransform CreateItem(GameObject prefab) => Object.Instantiate(prefab, _content).GetComponent<RectTransform>();
}