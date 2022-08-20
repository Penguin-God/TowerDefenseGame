using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnemySelector_UI : Multi_UI_Scene
{
    [SerializeField] Color selectColor;
    [SerializeField] Button selectButton;
    protected override void Init()
    {
        base.Init();
        Button[] buttons = GetComponentsInChildren<Button>();

        SetClickEvent();
        SetPointEvent();

        UpdateCurrentButton(buttons[0]);
        UpdateSelectEnemy(buttons[0].GetComponent<EnemySelector_Button>());

        void SetClickEvent()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                int buttonIndex = i;
                buttons[i].onClick.AddListener(() => UpdateCurrentButton(buttons[buttonIndex]));
                buttons[i].onClick.AddListener(() => UpdateSelectEnemy(buttons[buttonIndex].GetComponent<EnemySelector_Button>()));
            }
        }

        void SetPointEvent()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                AddTriggerEvent(buttons[i].GetComponent<EventTrigger>(), EventTriggerType.PointerEnter, PointEnter);
                AddTriggerEvent(buttons[i].GetComponent<EventTrigger>(), EventTriggerType.PointerExit, PointerExit);
            }
        }
    }

    void UpdateCurrentButton(Button button)
    {
        if (selectButton != null)
            selectButton.GetComponent<Image>().color = new Color(255, 255, 255);
        selectButton = button;
    }

    void UpdateSelectEnemy(EnemySelector_Button selectButton) => selectButton.SelectSpawnEnemy(selectColor);

    
    void AddTriggerEvent(EventTrigger trigger, EventTriggerType type, System.Action<EnemySelector_Button> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener((data) => action(trigger.GetComponent<EnemySelector_Button>()));
        trigger.triggers.Add(entry);
    }


    bool isShowInfoWindow;
    void PointEnter(EnemySelector_Button seleceButton) => StartCoroutine(Co_ShowEnemyInfo(seleceButton));

    void PointerExit(EnemySelector_Button seleceButton)
    {
        if (isShowInfoWindow)
        {
            isShowInfoWindow = false;
            Multi_Managers.UI.ClosePopupUI("BackGround");
        }
        else
            StopAllCoroutines();
    }

    IEnumerator Co_ShowEnemyInfo(EnemySelector_Button seleceButton)
    {
        yield return new WaitForSeconds(0.2f);
        isShowInfoWindow = true;
        seleceButton.ShwoInfoWindow();
    }
}
