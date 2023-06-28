using System.Collections;
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

    public void SetInfo(BattleEventDispatcher dispatcher) => _dispatcher = dispatcher;

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
