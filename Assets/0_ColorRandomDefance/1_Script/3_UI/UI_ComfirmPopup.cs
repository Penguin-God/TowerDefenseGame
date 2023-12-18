using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_ComfirmPopup : UI_Popup
{
    enum Texts
    {
        QuestionText
    }

    enum Buttons
    {
        YesButton
    }

    protected override void Init()
    {
        base.Init();
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
    }

    public void SetInfoWithClose(string questionText, UnityAction yesAction)
    {
        SetInfo(questionText, yesAction);
        GetButton((int)Buttons.YesButton).onClick.AddListener(Managers.UI.ClosePopupUI);
    }

    public void SetInfo(string questionText, UnityAction yesAction)
    {
        if (_initDone == false)
        {
            Init();
            _initDone = true;
        }

        GetText((int)Texts.QuestionText).text = questionText;
        GetButton((int)Buttons.YesButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.YesButton).onClick.AddListener(yesAction);
    }
}
