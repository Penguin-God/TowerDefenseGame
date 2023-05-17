using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class BattleButton_UI : UI_Scene
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

        Managers.Camera.OnIsLookMyWolrd += (isLookMy) => GetObject((int)GameObjects.Create_Defenser_Button).SetActive(isLookMy);
        Managers.Camera.OnIsLookMyWolrd += (isLookMy) => GetObject((int)GameObjects.Paint).SetActive(isLookMy);

        GetButton((int)Buttons.StoryWolrd_EnterButton).onClick.AddListener(CameraPositionChanged);
        GetButton((int)Buttons.Create_Defenser_Button).onClick.AddListener(SommonUnit);
    }

    void CameraPositionChanged()
    {
        Managers.UI.CloseAllPopupUI();
        Managers.Sound.PlayEffect(EffectSoundType.PopSound);
        if (Managers.Camera.IsLookEnemyTower)
        {
            Managers.Camera.LookWorld();
            GetText((int)Texts.StoryWorldText).text = "적군의 성으로";
        }
        else
        {
            Managers.Camera.LookEnemyTower();
            GetText((int)Texts.StoryWorldText).text = "월드로";
        }
    }

    void SommonUnit()
    {
        if (Multi_GameManager.Instance.UnitOver)
        {
            Managers.UI.ShowDefualtUI<UI_PopupText>().Show("유닛 공간이 부족해 소환할 수 없습니다.", 2f, Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
            return;
        }

        if (MultiServiceMidiator.Spawner.TryDrawUnit())
            Managers.Sound.PlayEffect(EffectSoundType.DrawSwordman);
    }
}
