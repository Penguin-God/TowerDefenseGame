using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_ComfirmPopup : Multi_UI_Popup
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
        GetButton((int)Buttons.YesButton).onClick.AddListener(Multi_Managers.UI.ClosePopupUI);
    }
}
