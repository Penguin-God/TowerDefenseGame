using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_Status : UI_Scene
{
    enum Texts
    {
        EnemyofCount,
        FoodText,
        GoldText,
        StageText,
        CurrentUnitText,
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

    void UpdateUnitText(int count) => GetText((int)Texts.CurrentUnitText).text = $"최대 유닛 갯수 {count}/{Multi_GameManager.instance.BattleData.MaxUnit}";

    readonly Color DENGER_COLOR = Color.red;
    void UpdateEnemyCountText(int EnemyofCount)
    {
        Text text = GetText((int)Texts.EnemyofCount);
        if (EnemyofCount > 40)
        {
            text.color = DENGER_COLOR;
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
        else text.color = Color.white;
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
