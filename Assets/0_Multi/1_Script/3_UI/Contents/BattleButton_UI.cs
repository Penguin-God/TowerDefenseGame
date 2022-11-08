using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleButton_UI : Multi_UI_Scene
{
    enum GameObjects
    {
        Create_Defenser_Button,
        Paint,
    }

    enum Buttons
    {
        Create_Defenser_Button,
        StoryWolrd_EnterButton,
    }

    enum Texts
    {
        StoryWorldText,
    }

    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        Multi_Managers.Camera.OnIsLookMyWolrd += (isLookMy) => GetObject((int)GameObjects.Create_Defenser_Button).SetActive(isLookMy);
        Multi_Managers.Camera.OnIsLookMyWolrd += (isLookMy) => GetObject((int)GameObjects.Paint).SetActive(isLookMy);

        GetButton((int)Buttons.StoryWolrd_EnterButton).onClick.AddListener(CameraPositionChanged);
        GetButton((int)Buttons.Create_Defenser_Button).onClick.AddListener(SommonUnit);
    }

    void CameraPositionChanged()
    {
        Multi_Managers.UI.CloseAllPopupUI();
        Multi_Managers.Sound.PlayEffect(EffectSoundType.PopSound);
        if (Multi_Managers.Camera.IsLookEnemyTower)
        {
            Multi_Managers.Camera.LookWorld();
            GetText((int)Texts.StoryWorldText).text = "적군의 성으로";
        }
        else
        {
            Multi_Managers.Camera.LookEnemyTower();
            GetText((int)Texts.StoryWorldText).text = "월드로";
        }
    }

    void SommonUnit()
    {
        if (Multi_GameManager.instance.UnitOver)
        {
            Multi_Managers.UI.ShowPopupUI<WarningText>().Show("유닛 공간이 부족해 소환할 수 없습니다.");
            Multi_Managers.Sound.PlayEffect(EffectSoundType.Denger);
            return;
        }

        var summonData = Multi_GameManager.instance.BattleData.UnitSummonData;
        if (Multi_GameManager.instance.TryUseGold(summonData.price))
        {
            Multi_SpawnManagers.NormalUnit.Spawn(Random.Range(0, summonData.maxColorNumber ), 0);
            Multi_Managers.Sound.PlayEffect(EffectSoundType.DrawSwordman);
        }
    }
}
