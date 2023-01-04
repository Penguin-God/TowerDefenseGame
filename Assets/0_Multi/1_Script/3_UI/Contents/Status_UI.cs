using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Status_UI : UI_Scene
{
    enum Texts
    {
        EnemyofCount,
        FoodText,
        GoldText,
        StageText,
        CurrentUnitText,
        KnigthText,
        ArcherText,
        SpearmanText,
        MageText,
        OhterEnemyCountText,
        OtherUnitCountText,
    }

    enum GameObjects
    {
        TimerSlider,
        GoldBar,
        FoodBar,
    }

    enum Images
    {
        MainSkill,
        SubSkill,
    }

    protected override void Init()
    {
        base.Init();
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        timerSlider = GetObject((int)GameObjects.TimerSlider).GetComponent<Slider>();

        InitEvent();
    }

    void InitEvent()
    {
        Multi_StageManager.Instance.OnUpdateStage -= UpdateStage;
        Multi_StageManager.Instance.OnUpdateStage += UpdateStage;

        Multi_GameManager.instance.BattleData.OnMaxUnitChanged += (maxUnit) => UpdateUnitText(Multi_UnitManager.Instance.CurrentUnitCount);

        UpdateOtherUnitAllCount(0);
        UpdateOtherUnitClassCount();
        UpdateOtherEnemyCountText(0);
        UpdateEnemyCountText(0);
        UpdateMySkillImage();

        BindGoldBarEvent();
        BindFoodBarEvent();
        BindMyCountEvent();
        BindOhterCountEvent();
        BindUserSkillImageEvent();

        void BindGoldBarEvent()
        {
            Multi_GameManager.instance.OnGoldChanged -= (gold) => GetText((int)Texts.GoldText).text = gold.ToString();
            Managers.Camera.OnIsLookMyWolrd -= (lookMy) => GetObject((int)GameObjects.GoldBar).SetActive(lookMy);

            Multi_GameManager.instance.OnGoldChanged += (gold) => GetText((int)Texts.GoldText).text = gold.ToString();
            Managers.Camera.OnIsLookMyWolrd += (lookMy) => GetObject((int)GameObjects.GoldBar).SetActive(lookMy);
        }

        void BindFoodBarEvent()
        {
            Managers.Camera.OnIsLookMyWolrd -= (lookMy) => GetObject((int)GameObjects.FoodBar).SetActive(lookMy);
            Managers.Camera.OnIsLookMyWolrd += (lookMy) => GetObject((int)GameObjects.FoodBar).SetActive(lookMy);

            if (Managers.ClientData.EquipSkillManager.EquipSkills.Contains(SkillType.고기혐오자))
            {
                return;
            }

            Multi_GameManager.instance.OnFoodChanged -= (food) => GetText((int)Texts.FoodText).text = food.ToString();
            Multi_GameManager.instance.OnFoodChanged += (food) => GetText((int)Texts.FoodText).text = food.ToString();
        }

        void BindMyCountEvent()
        {
            Multi_UnitManager.Instance.OnUnitCountChanged -= UpdateUnitText;
            Multi_UnitManager.Instance.OnUnitCountChanged += UpdateUnitText;

            Multi_EnemyManager.Instance.OnEnemyCountChanged -= UpdateEnemyCountText;
            Multi_EnemyManager.Instance.OnEnemyCountChanged += UpdateEnemyCountText;
        }

        void BindOhterCountEvent()
        {
            Multi_UnitManager.Instance.OnOtherUnitCountChanged -= UpdateOtherUnitAllCount;
            Multi_UnitManager.Instance.OnOtherUnitCountChanged += UpdateOtherUnitAllCount;

            Multi_UnitManager.Instance.OnOtherUnitCountChanged -= (count) => UpdateOtherUnitClassCount();
            Multi_UnitManager.Instance.OnOtherUnitCountChanged += (count) => UpdateOtherUnitClassCount();

            Multi_EnemyManager.Instance.OnOtherEnemyCountChanged -= UpdateOtherEnemyCountText;
            Multi_EnemyManager.Instance.OnOtherEnemyCountChanged += UpdateOtherEnemyCountText;
        }

        void BindUserSkillImageEvent()
        {
            Managers.Camera.OnLookMyWolrd -= UpdateMySkillImage;
            Managers.Camera.OnLookMyWolrd += UpdateMySkillImage;

            Managers.Camera.OnLookEnemyWorld -= UpdateOtherSkillImage;
            Managers.Camera.OnLookEnemyWorld += UpdateOtherSkillImage;
        }
    }

    // TODO : Multi_GameManager.instance.MaxUnitCount 각 플레이어걸로
    void UpdateUnitText(int count) => GetText((int)Texts.CurrentUnitText).text = $"최대 유닛 갯수 {count}/{Multi_GameManager.instance.BattleData.MaxUnit}";

    Color originColor = new Color(1, 1, 1, 1);
    Color dengerColor = new Color(1, 0, 0, 1);
    void UpdateEnemyCountText(int EnemyofCount)
    {
        Text text = GetText((int)Texts.EnemyofCount);
        if (EnemyofCount > 40)
        {
            text.color = dengerColor;
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
        else text.color = originColor;
        text.text = $"현재 적 유닛 카운트 : {EnemyofCount}/{Multi_GameManager.instance.BattleData.MaxEnemyCount}";
    }


    Slider timerSlider;
    void UpdateStage(int stage)
    {
        StopAllCoroutines();
        timerSlider.maxValue = Multi_StageManager.Instance.STAGE_TIME;
        timerSlider.value = timerSlider.maxValue;
        GetText((int)Texts.StageText).text = "현재 스테이지 : " + stage;
        StartCoroutine(Co_UpdateTimer());
    }

    IEnumerator Co_UpdateTimer()
    {
        while (true)
        {
            yield return null;
            timerSlider.value -= Time.deltaTime;
        }
    }

    void UpdateOtherUnitAllCount(int count) => GetText((int)Texts.OtherUnitCountText).text = $"{count}/??";

    void UpdateOtherUnitClassCount()
    {
        GetText((int)Texts.KnigthText).text = "" + Multi_UnitManager.Instance.EnemyPlayerUnitCountByClass[UnitClass.sowrdman];
        GetText((int)Texts.ArcherText).text = "" + Multi_UnitManager.Instance.EnemyPlayerUnitCountByClass[UnitClass.archer];
        GetText((int)Texts.SpearmanText).text = "" + Multi_UnitManager.Instance.EnemyPlayerUnitCountByClass[UnitClass.spearman];
        GetText((int)Texts.MageText).text = "" + Multi_UnitManager.Instance.EnemyPlayerUnitCountByClass[UnitClass.mage];
    }

    void UpdateOtherEnemyCountText(int count) => GetText((int)Texts.OhterEnemyCountText).text = "" + count;


    void UpdateMySkillImage() => ChangeEquipSkillImages(Managers.ClientData.EquipSkillManager.MainSkill, Managers.ClientData.EquipSkillManager.SubSkill);
    void UpdateOtherSkillImage()
    {
        if (Multi_GameManager.instance.OtherPlayerData != null)
            ChangeEquipSkillImages(Multi_GameManager.instance.OtherPlayerData.MainSkill, Multi_GameManager.instance.OtherPlayerData.SubSkill);
        else
            ChangeEquipSkillImages(SkillType.None, SkillType.None);
    }

    void ChangeEquipSkillImages(SkillType mainSkill, SkillType subSkill)
    {
        if (mainSkill == SkillType.None)
            GetImage((int)Images.MainSkill).color = new Color(1, 1, 1, 0);
        else
        {
            GetImage((int)Images.MainSkill).sprite = Managers.Data.UserSkill.GetSkillGoodsData(mainSkill).ImageSprite;
            GetImage((int)Images.MainSkill).color = new Color(1, 1, 1, 1);
        }

        if (mainSkill == SkillType.None)
            GetImage((int)Images.SubSkill).color = new Color(1, 1, 1, 0);
        else
        {
            GetImage((int)Images.SubSkill).sprite = Managers.Data.UserSkill.GetSkillGoodsData(subSkill).ImageSprite;
            GetImage((int)Images.SubSkill).color = new Color(1, 1, 1, 1);
        }
    }
}
