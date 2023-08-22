﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleButtons : UI_Scene
{
    enum GameObjects
    {
        Paint,
    }

    protected enum Buttons
    {
        SummonUnitButton,
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

        Managers.Camera.OnIsLookMyWolrd += (isLookMy) => GetButton((int)Buttons.SummonUnitButton).gameObject.SetActive(isLookMy);
        Managers.Camera.OnIsLookMyWolrd += (isLookMy) => GetObject((int)GameObjects.Paint).SetActive(isLookMy);

        GetButton((int)Buttons.StoryWolrd_EnterButton).onClick.AddListener(CameraPositionChanged);
        GetButton((int)Buttons.SummonUnitButton).onClick.AddListener(SommonUnit);
    }

    public void Inject(SwordmanGachaController swordmanGachaController, TextShowAndHideController textShowAndHideController)
    {
        _swordmanGachaController = swordmanGachaController;
        _textShowAndHideController = textShowAndHideController;
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

    protected virtual void GachaUnit()
    {
        if (_swordmanGachaController.TryDrawUnit())
            Managers.Sound.PlayEffect(EffectSoundType.DrawSwordman);
    }
}