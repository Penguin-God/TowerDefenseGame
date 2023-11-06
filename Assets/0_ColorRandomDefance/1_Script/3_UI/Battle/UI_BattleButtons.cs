using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleButtons : UI_Scene
{
    enum Buttons
    {
        SummonUnitButton,
        StoryWolrd_EnterButton,
        WorldChangeButton,
    }

    enum Texts
    {
        StoryWorldText,
        WorldDestinationText,
    }

    protected override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        Managers.Camera.OnIsLookMyWolrd += (isLookMy) => GetButton((int)Buttons.SummonUnitButton).gameObject.SetActive(isLookMy);
        GetButton((int)Buttons.StoryWolrd_EnterButton).onClick.AddListener(CameraPositionChanged);
        GetButton((int)Buttons.SummonUnitButton).onClick.AddListener(SommonUnit);
        GetButton((int)Buttons.WorldChangeButton).onClick.AddListener(ChangeWorld);
    }

    public void DependencyInject(SwordmanGachaController swordmanGachaController, TextShowAndHideController textShowAndHideController)
    {
        _swordmanGachaController = swordmanGachaController;
        _textShowAndHideController = textShowAndHideController;
    }

    void ChangeText(Texts textType, string text) => GetTextMeshPro((int)textType).text = text;

    void CameraPositionChanged()
    {
        Managers.UI.CloseAllPopupUI();
        Managers.Sound.PlayEffect(EffectSoundType.PopSound);
        if (Managers.Camera.IsLookEnemyTower)
        {
            Managers.Camera.LookWorld();
            ChangeText((int)Texts.StoryWorldText, "적군의 성으로");
        }
        else
        {
            Managers.Camera.LookEnemyTower();
            ChangeText((int)Texts.StoryWorldText, "월드로");
        }
    }

    [SerializeField] Sprite lookMyWorldIcon;
    [SerializeField] Sprite lookEnemyWorldIcon;
    void ChangeWorld()
    {
        Managers.Camera.LookWorldChanged();
        if (Managers.Camera.CameraSpot.IsInDefenseWorld == false)
            Managers.Camera.LookWorld();
        Managers.Sound.PlayEffect(EffectSoundType.PopSound);
        if (Managers.Camera.LookWorld_Id == PlayerIdManager.Id)
        {
            GetButton((int)Buttons.WorldChangeButton).image.sprite = lookMyWorldIcon;
            ChangeText(Texts.WorldDestinationText, "상대 진영으로");
            ChangeText((int)Texts.StoryWorldText, "적군의 성으로");
        }
        else
        {
            GetButton((int)Buttons.WorldChangeButton).image.sprite = lookEnemyWorldIcon;
            ChangeText(Texts.WorldDestinationText, "아군 진영으로");
            ChangeText((int)Texts.StoryWorldText, "적군의 성으로");
        }
    }

    SwordmanGachaController _swordmanGachaController;
    TextShowAndHideController _textShowAndHideController;
    void SommonUnit()
    {
        if (Multi_GameManager.Instance.UnitOver)
        {
            _textShowAndHideController.ShowTextForTime("유닛 공간이 부족해 소환할 수 없습니다.", Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
            return;
        }

        GachaUnit();
    }

    void GachaUnit()
    {
        if (_swordmanGachaController.TryDrawUnit())
            Managers.Sound.PlayEffect(EffectSoundType.DrawSwordman);
    }
}
