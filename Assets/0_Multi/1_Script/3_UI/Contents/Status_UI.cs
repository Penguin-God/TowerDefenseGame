﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status_UI : Multi_UI_Scene
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
        EnemyText,
    }

    enum GameObjects
    {
        TimerSlider,
        GoldBar,
        FoodBar,
    }

    protected override void Init()
    {
        base.Init();
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        timerSlider = GetObject((int)GameObjects.TimerSlider).GetComponent<Slider>();

        InitEvent();
    }

    void InitEvent()
    {
        Multi_StageManager.Instance.OnUpdateStage -= UpdateStage;
        Multi_StageManager.Instance.OnUpdateStage += UpdateStage;

        BindGoldBarEvent();
        BindFoodBarEvent();
        BindUnitPanelEvent();
        BindEnemyCountTextEvent();
        BindEnemyPlayerUnitCountEvent();
        BindEnemyPlayerEnemyCountEvent();

        void BindGoldBarEvent()
        {
            Multi_GameManager.instance.OnGoldChanged -= (gold) => GetText((int)Texts.GoldText).text = gold.ToString();
            Multi_Managers.Camera.OnIsLookMyWolrd -= (lookMy) => GetObject((int)GameObjects.GoldBar).SetActive(lookMy);

            Multi_GameManager.instance.OnGoldChanged += (gold) => GetText((int)Texts.GoldText).text = gold.ToString();
            Multi_Managers.Camera.OnIsLookMyWolrd += (lookMy) => GetObject((int)GameObjects.GoldBar).SetActive(lookMy);
        }

        void BindFoodBarEvent()
        {
            Multi_GameManager.instance.OnFoodChanged -= (food) => GetText((int)Texts.FoodText).text = food.ToString();
            Multi_Managers.Camera.OnIsLookMyWolrd -= (lookMy) => GetObject((int)GameObjects.FoodBar).SetActive(lookMy);

            Multi_GameManager.instance.OnFoodChanged += (food) => GetText((int)Texts.FoodText).text = food.ToString();
            Multi_Managers.Camera.OnIsLookMyWolrd += (lookMy) => GetObject((int)GameObjects.FoodBar).SetActive(lookMy);
        }

        void BindUnitPanelEvent()
        {
            Multi_UnitManager.Instance.OnUnitCountChanged -= UpdateUnitText;
            Multi_Managers.Camera.OnLookMyWolrd -= () => UpdateUnitText(Multi_UnitManager.Instance.CurrentUnitCount);
            
            Multi_UnitManager.Instance.OnUnitCountChanged += UpdateUnitText;
            Multi_Managers.Camera.OnLookMyWolrd += () => UpdateUnitText(Multi_UnitManager.Instance.CurrentUnitCount);
        }

        void BindEnemyCountTextEvent()
        {
            Multi_EnemyManager.Instance.OnEnemyCountChanged -= UpdateMyEnemyCountText;
            Multi_Managers.Camera.OnLookMyWolrd -= () => UpdateEnemyCountText(Multi_EnemyManager.Instance.MyEnemyCount);
            
            Multi_EnemyManager.Instance.OnEnemyCountChanged += UpdateMyEnemyCountText;
            Multi_Managers.Camera.OnLookMyWolrd += () => UpdateEnemyCountText(Multi_EnemyManager.Instance.MyEnemyCount);
        }

        void BindEnemyPlayerUnitCountEvent()
        {
            Multi_UnitManager.Instance.OnOtherUnitCountChanged -= (count) => UpdateEnemyPlayerUnitCount();
            Multi_UnitManager.Instance.OnOtherUnitCountChanged -= UpdateOtherUnitText;
            Multi_Managers.Camera.OnLookEnemyWorld -= () => UpdateUnitText(Multi_UnitManager.Instance.EnemyPlayerHasCount);

            Multi_UnitManager.Instance.OnOtherUnitCountChanged += (count) => UpdateEnemyPlayerUnitCount();
            Multi_UnitManager.Instance.OnOtherUnitCountChanged += UpdateOtherUnitText;
            Multi_Managers.Camera.OnLookEnemyWorld += () => UpdateUnitText(Multi_UnitManager.Instance.EnemyPlayerHasCount);
        }

        void BindEnemyPlayerEnemyCountEvent()
        {
            Multi_EnemyManager.Instance.OnOtherEnemyCountChanged -= UpdateOtherEnemyCountText;
            Multi_EnemyManager.Instance.OnOtherEnemyCountChanged -= UpdateOtherEnemyText;
            Multi_Managers.Camera.OnLookEnemyWorld -= () => UpdateEnemyCountText(Multi_EnemyManager.Instance.EnemyPlayerEnemyCount);


            Multi_EnemyManager.Instance.OnOtherEnemyCountChanged += UpdateOtherEnemyCountText;
            Multi_EnemyManager.Instance.OnOtherEnemyCountChanged += UpdateOtherEnemyText;
            Multi_Managers.Camera.OnLookEnemyWorld += () => UpdateEnemyCountText(Multi_EnemyManager.Instance.EnemyPlayerEnemyCount);
        }
    }

    // TODO : Multi_GameManager.instance.MaxUnitCount 각 플레이어걸로
    void UpdateUnitText(int count) => GetText((int)Texts.CurrentUnitText).text = $"최대 유닛 갯수 {count}/{Multi_GameManager.instance.MaxUnitCount}";

    void UpdateOtherUnitText(int count)
    {
        if (Multi_Managers.Camera.LookWorld_Id != Multi_Data.instance.Id)
            UpdateUnitText(count);
    }

    void UpdateOtherEnemyText(int count)
    {
        GetText((int)Texts.EnemyText).text = "" + count;
    }

    Color originColor = new Color(1, 1, 1, 1);
    Color dengerColor = new Color(1, 0, 0, 1);
    void UpdateEnemyCountText(int EnemyofCount)
    {
        Text text = GetText((int)Texts.EnemyofCount);
        if (EnemyofCount > 40)
        {
            text.color = dengerColor;
            Multi_Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
        else text.color = originColor;
        text.text = $"현재 적 유닛 카운트 : {EnemyofCount}/{Multi_GameManager.instance.MaxEnemyCount}";
    }

    void UpdateOtherEnemyCountText(int count)
    {
        if (Multi_Managers.Camera.IsLookOtherWolrd)
            UpdateEnemyCountText(count);
    }

    void UpdateMyEnemyCountText(int count)
    {
        if (Multi_Managers.Camera.IsLookOtherWolrd == false)
            UpdateEnemyCountText(count);
    }


    Slider timerSlider;
    void UpdateStage(int stage)
    {
        StopAllCoroutines();
        timerSlider.maxValue = Multi_SpawnManagers.NormalEnemy.EnemySpawnTime;
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

    void UpdateEnemyPlayerUnitCount()
    {
        GetText((int)Texts.KnigthText).text = "" + Multi_UnitManager.Instance.EnemyPlayerUnitCountByClass[UnitClass.sowrdman];
        GetText((int)Texts.ArcherText).text = "" + Multi_UnitManager.Instance.EnemyPlayerUnitCountByClass[UnitClass.archer];
        GetText((int)Texts.SpearmanText).text = "" + Multi_UnitManager.Instance.EnemyPlayerUnitCountByClass[UnitClass.spearman];
        GetText((int)Texts.MageText).text = "" + Multi_UnitManager.Instance.EnemyPlayerUnitCountByClass[UnitClass.mage];
    }
}
