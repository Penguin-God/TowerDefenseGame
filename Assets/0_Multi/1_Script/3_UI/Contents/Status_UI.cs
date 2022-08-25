using System.Collections;
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
    }

    protected override void Init()
    {
        base.Init();
        Bind<Text>(typeof(Texts));

        Multi_EnemyManager.Instance.OnEnemyCountChanged += UpdateEnemyCountText;
        Multi_GameManager.instance.OnGoldChanged += (gold) => GetText((int)Texts.GoldText).text = gold.ToString();
        Multi_GameManager.instance.OnFoodChanged += (food) => GetText((int)Texts.FoodText).text = food.ToString();
        Multi_StageManager.Instance.OnUpdateStage += (_stage) => GetText((int)Texts.StageText).text = "현재 스테이지 : " + _stage;
        Multi_UnitManager.Count.OnUnitCountChanged += (count) => GetText((int)Texts.CurrentUnitText).text = $"최대 유닛 갯수 {count}/{Multi_GameManager.instance.MaxUnitCount}";
    }

    void UpdateEnemyCountText(int EnemyofCount)
    {
        Text text = GetText((int)Texts.EnemyofCount);
        if (EnemyofCount > 45) text.color = new Color32(255, 0, 0, 255);
        else text.color = new Color32(255, 255, 255, 255);
        text.text = "현재 적 유닛 카운트 : " + EnemyofCount + "/50";
    }
}
