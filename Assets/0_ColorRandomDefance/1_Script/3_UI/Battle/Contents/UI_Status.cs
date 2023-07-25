﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_Status : UI_Scene
{
    enum Texts
    {
        FoodText,
        GoldText,
        StageText,
    }

    enum GameObjects
    {
        TimerSlider,
        GoldBar,
        FoodBar,
        MyCount,
        OpponentCount,
    }

    enum Images
    {
        MainSkill,
        SubSkill,
    }

    BattleEventDispatcher _dispatcher;
    MultiData<SkillBattleDataContainer> _multiEquipSkillData;
    SkillBattleDataContainer MySkillData => _multiEquipSkillData.GetData(PlayerIdManager.Id);
    SkillBattleDataContainer EnemySkillData => _multiEquipSkillData.GetData(PlayerIdManager.EnemyId);
    public void Injection(BattleEventDispatcher dispatcher, MultiData<SkillBattleDataContainer> multiEquipSkillData)
    {
        _dispatcher = dispatcher;
        _multiEquipSkillData = multiEquipSkillData;
    }

    protected override void Init()
    {
        base.Init();
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        timerSlider = GetObject((int)GameObjects.TimerSlider).GetComponent<Slider>();

        Init_UI();
        BindEvent(_dispatcher);
    }

    void Init_UI()
    {
        UpdateStage(1);
        GetText((int)Texts.GoldText).text = Multi_GameManager.Instance.CurrencyManager.Gold.ToString();
        GetText((int)Texts.FoodText).text = Multi_GameManager.Instance.CurrencyManager.Food.ToString();
        UpdateMySkillImage();
    }

    void BindEvent(BattleEventDispatcher dispatcher)
    {
        StageManager.Instance.OnUpdateStage -= UpdateStage;
        StageManager.Instance.OnUpdateStage += UpdateStage;

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
            var myCountDisplay = GetObject((int)GameObjects.MyCount).GetComponent<UI_ObjectCountDisplay>();

            Managers.Unit.OnUnitCountChange += myCountDisplay.UpdateCurrentUnitText;
            Multi_GameManager.Instance.BattleData.OnMaxUnitChanged += myCountDisplay.UpdateMaxUnitCount;
            Managers.Unit.OnUnitCountChangeByClass += myCountDisplay.UpdateUnitClassByCount;

            dispatcher.OnMonsterCountChanged += myCountDisplay.UpdateMonsterCountText;


            var oppentCountDisplay = GetObject((int)GameObjects.OpponentCount).GetComponent<UI_ObjectCountDisplay>();

            dispatcher.OnOpponentUnitCountChanged += oppentCountDisplay.UpdateCurrentUnitText;
            dispatcher.OnOpponentUnitCountChangedByClass += oppentCountDisplay.UpdateUnitClassByCount;
            dispatcher.OnOpponentUnitMaxCountChanged += oppentCountDisplay.UpdateMaxUnitCount;

            dispatcher.OnOpponentMonsterCountChange += oppentCountDisplay.UpdateMonsterCountText;
        }

        void BindUserSkillImageEvent()
        {
            Managers.Camera.OnLookMyWolrd -= UpdateMySkillImage;
            Managers.Camera.OnLookMyWolrd += UpdateMySkillImage;

            Managers.Camera.OnLookEnemyWorld -= UpdateOtherSkillImage;
            Managers.Camera.OnLookEnemyWorld += UpdateOtherSkillImage;
        }
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

    public void UpdateMySkillImage() => ChangeEquipSkillImages(MySkillData.MainSkill.SkillType, MySkillData.SubSkill.SkillType);
    void UpdateOtherSkillImage()
    {
        if (EnemySkillData != null)
            ChangeEquipSkillImages(EnemySkillData.MainSkill.SkillType, EnemySkillData.SubSkill.SkillType);
        else
            ChangeEquipSkillImages(SkillType.None, SkillType.None);
    }

    void ChangeEquipSkillImages(SkillType mainSkill, SkillType subSkill)
    {
        ChangeSkill_Image(mainSkill, (int)Images.MainSkill);
        ChangeSkill_Image(subSkill, (int)Images.SubSkill);
    }

    void ChangeSkill_Image(SkillType skill, int imageIndex)
    {
        if (skill == SkillType.None)
            GetImage(imageIndex).color = new Color(1, 1, 1, 0);
        else
        {
            GetImage(imageIndex).sprite = Managers.Data.UserSkill.GetSkillGoodsData(skill).ImageSprite;
            GetImage(imageIndex).color = new Color(1, 1, 1, 1);
        }
    }
}
