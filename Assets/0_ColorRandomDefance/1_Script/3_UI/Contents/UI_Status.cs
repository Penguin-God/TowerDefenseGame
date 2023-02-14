﻿using System.Collections;
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

        InitEvent();
    }

    void InitEvent()
    {
        Multi_StageManager.Instance.OnUpdateStage -= UpdateStage;
        Multi_StageManager.Instance.OnUpdateStage += UpdateStage;

        Multi_GameManager.instance.BattleData.OnMaxUnitChanged += (maxUnit) => UpdateUnitText(Multi_UnitManager.Instance.CurrentUnitCount);

        Multi_UnitManager.Instance.OnUnitFlagCountChanged += (flag, count) => UpdateUnitClassByCount();

        Init_UI();
        Bind_Events();

        void Init_UI()
        {
            UpdateEnemyCountText(0);
            UpdateMySkillImage();
        }


        void Bind_Events()
        {
            BindGoldBarEvent();
            BindFoodBarEvent();
            BindMyCountEvent();
            BindUserSkillImageEvent();
        }

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

        void BindUserSkillImageEvent()
        {
            Managers.Camera.OnLookMyWolrd -= UpdateMySkillImage;
            Managers.Camera.OnLookMyWolrd += UpdateMySkillImage;

            Managers.Camera.OnLookEnemyWorld -= UpdateOtherSkillImage;
            Managers.Camera.OnLookEnemyWorld += UpdateOtherSkillImage;
        }
    }

    void UpdateUnitText(int count) => GetText((int)Texts.MyUnitCountText).text = $"{count}/{Multi_GameManager.instance.BattleData.MaxUnit}";

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
        text.text = $"{count}/{Multi_GameManager.instance.BattleData.MaxEnemyCount}";
    }

    public void UpdateUnitClassByCount()
    {
        GetText((int)Texts.MyKnigthText).text = "" + GetCountByClass(UnitClass.Swordman);
        GetText((int)Texts.MyArcherText).text = "" + GetCountByClass(UnitClass.Archer);
        GetText((int)Texts.MySpearmanText).text = "" + GetCountByClass(UnitClass.Spearman);
        GetText((int)Texts.MyMageText).text = "" + GetCountByClass(UnitClass.Mage);

        int GetCountByClass(UnitClass unitClass) => Multi_UnitManager.Instance.UnitCountByFlag.Where(x => x.Key.UnitClass == unitClass).Sum(x => x.Value);
    }

    Slider timerSlider;
    void UpdateStage(int stageNumber)
    {
        StopAllCoroutines();
        timerSlider.maxValue = Multi_StageManager.Instance.STAGE_TIME;
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
