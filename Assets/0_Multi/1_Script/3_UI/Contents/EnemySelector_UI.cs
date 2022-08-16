using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnemySelector_UI : Multi_UI_Base
{
//    EventTrigger trigger = GetComponent<EventTrigger>();
//    EventTrigger.Entry entry = new EventTrigger.Entry();
//    entry.eventID = EventTriggerType.PointerDown;
//		entry.callback.AddListener((data ) => { OnPointerDownDelegate((PointerEventData) data);
//} );
//trigger.triggers.Add(entry);

    [SerializeField] Color selectColor;
    [SerializeField] Button selectButton;
    protected override void Init()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() => SetSpawnEnemyNumber(3, buttons[0]));
        buttons[1].onClick.AddListener(() => SetSpawnEnemyNumber(0, buttons[1]));
        buttons[2].onClick.AddListener(() => SetSpawnEnemyNumber(2, buttons[2]));
        buttons[3].onClick.AddListener(() => SetSpawnEnemyNumber(1, buttons[3]));

        SetSpawnEnemyNumber(3, buttons[0]);

        for (int i = 0; i < buttons.Length; i++)
        {
            AddTriggerEvent(buttons[i].GetComponent<EventTrigger>(), EventTriggerType.PointerEnter, PointEnter);
            AddTriggerEvent(buttons[i].GetComponent<EventTrigger>(), EventTriggerType.PointerExit, PointerExit);
        }
    }

    void SetSpawnEnemyNumber(int num, Button button)
    {
        Multi_SpawnManagers.NormalEnemy.SetOtherEnemyNumber(num);
        if (selectButton != null)
            selectButton.GetComponent<Image>().color = new Color(255, 255, 255);
        selectButton = button;
        button.GetComponent<Image>().color = selectColor;
    }

    void ShowEnemyInfo(Button button)
    {
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        // AddTriggerEvent()
    }

    void AddTriggerEvent(EventTrigger trigger, EventTriggerType type, System.Action action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener((data) => action());
        trigger.triggers.Add(entry);
    }


    bool isShowInfoWindow;
    void PointEnter()
    {
        print("진입!!");
        StartCoroutine("Co_ShowEnemyInfo");
    }

    void PointerExit()
    {
        if (isShowInfoWindow)
        {
            isShowInfoWindow = false;
            print("정보창 꺼지는 코드 실행....");
        }
        else
        {
            StopCoroutine("Co_ShowEnemyInfo");
            print("멈춰!!");
        }
        print("나감");
    }

    IEnumerator Co_ShowEnemyInfo()
    {
        yield return new WaitForSeconds(1f);
        isShowInfoWindow = true;
        print("내가 왔따!");
    }
}
