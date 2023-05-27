using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_Status : UI_Scene
{
    enum Texts
    {
        MyEnemyCountText,
        FoodText,
        GoldText,
        StageText,
        MyUnitCountText, 
        MyKnigthText,
        MyArcherText,
        MySpearmanText,
        MyMageText,
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

        Init_UI();
        InitEvent();
    }

    void Init_UI()
    {
        GetText((int)Texts.GoldText).text = Multi_GameManager.Instance.CurrencyManager.Gold.ToString();
        GetText((int)Texts.FoodText).text = Multi_GameManager.Instance.CurrencyManager.Food.ToString();
        UpdateUnitText(0);

        UpdateStage(1);
        UpdateEnemyCountText(0);
        UpdateMySkillImage();
    }

    void InitEvent()
    {
        StageManager.Instance.OnUpdateStage -= UpdateStage;
        StageManager.Instance.OnUpdateStage += UpdateStage;

        Multi_GameManager.Instance.BattleData.OnMaxUnitChanged += (maxUnit) => UpdateUnitText(Managers.Unit.CurrentUnitCount);

        Managers.Unit.OnUnitCountChangeByClass += UpdateUnitClassByCount;

        Bind_Events();

        void Bind_Events()
        {
            BindGoldBarEvent();
            BindFoodBarEvent();
            BindMyCountEvent();
            BindUserSkillImageEvent();
        }

        void BindGoldBarEvent()
        {
            Multi_GameManager.Instance.OnGoldChanged -= (gold) => GetText((int)Texts.GoldText).text = gold.ToString();
            Managers.Camera.OnIsLookMyWolrd -= (lookMy) => GetObject((int)GameObjects.GoldBar).SetActive(lookMy);

            Multi_GameManager.Instance.OnGoldChanged += (gold) => GetText((int)Texts.GoldText).text = gold.ToString();
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

            Multi_GameManager.Instance.OnFoodChanged -= (food) => GetText((int)Texts.FoodText).text = food.ToString();
            Multi_GameManager.Instance.OnFoodChanged += (food) => GetText((int)Texts.FoodText).text = food.ToString();
        }

        void BindMyCountEvent()
        {
            Managers.Unit.OnUnitCountChange -= UpdateUnitText;
            Managers.Unit.OnUnitCountChange += UpdateUnitText;

            Multi_EnemyManager.Instance.OnEnemyCountChanged -= UpdateEnemyCountText;
            Multi_EnemyManager.Instance.OnEnemyCountChanged += UpdateEnemyCountText;
        }

        void BindUserSkillImageEvent()
        {
            Managers.Camera.OnLookMyWolrd -= UpdateMySkillImage;
            Managers.Camera.OnLookMyWolrd += UpdateMySkillImage;

            Managers.Camera.OnLookEnemyWorld -= UpdateOtherSkillImage;
            Managers.Camera.OnLookEnemyWorld += UpdateOtherSkillImage;
        }
    }

    readonly PlayerStatusPersenter _presneter = new PlayerStatusPersenter();
    void UpdateUnitText(int count) => GetText((int)Texts.MyUnitCountText).text = _presneter.BuildUnitCountText(count, Multi_GameManager.Instance.BattleData.MaxUnit);

    readonly Color DENGER_COLOR = Color.red;
    void UpdateEnemyCountText(int count)
    {
        Text text = GetText((int)Texts.MyEnemyCountText);
        if (count > 40)
        {
            text.color = DENGER_COLOR;
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
        else text.color = Color.white;
        text.text = _presneter.BuildMonsterCountText(count, Multi_GameManager.Instance.BattleData.MaxEnemyCount);
    }

    void UpdateUnitClassByCount(UnitClass unitClass, int count)
    {
        var textByUnitClass = new Dictionary<UnitClass, Texts>()
        {
            {UnitClass.Swordman, Texts.MyKnigthText },
            {UnitClass.Archer, Texts.MyArcherText },
            {UnitClass.Spearman, Texts.MySpearmanText },
            {UnitClass.Mage, Texts.MyMageText },
        };
        GetText((int)textByUnitClass[unitClass]).text = count.ToString();
    }

    Slider timerSlider;
    void UpdateStage(int stageNumber)
    {
        StopAllCoroutines();
        timerSlider.maxValue = StageManager.Instance.STAGE_TIME;
        timerSlider.value = timerSlider.maxValue;
        GetText((int)Texts.StageText).text = $"Stage {stageNumber} : " ;
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

    public void UpdateMySkillImage() => ChangeEquipSkillImages(Managers.ClientData.EquipSkillManager.MainSkill, Managers.ClientData.EquipSkillManager.SubSkill);
    void UpdateOtherSkillImage()
    {
        if (Multi_GameManager.Instance.OtherPlayerData != null)
            ChangeEquipSkillImages(Multi_GameManager.Instance.OtherPlayerData.MainSkill, Multi_GameManager.Instance.OtherPlayerData.SubSkill);
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
