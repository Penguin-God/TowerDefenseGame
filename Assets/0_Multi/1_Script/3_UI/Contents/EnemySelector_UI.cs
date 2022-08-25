using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class EnemySelector_UI : Multi_UI_Scene
{
    [SerializeField] Color selectColor;
    [SerializeField] EnemySelector_Button selectButton;
    protected override void Init()
    {
        base.Init();
        
        List<EnemySelector_Button> enemySelectBtns = GetComponentsInChildren<EnemySelector_Button>().ToList();
        enemySelectBtns.ForEach(x => x.Setup(selectColor, UpdateCurrentButton));
        enemySelectBtns[0].StartSelectSpawnEnemy();
        UpdateCurrentButton(enemySelectBtns[0]);

        SetPointEvent();

        void SetPointEvent()
        {
            for (int i = 0; i < enemySelectBtns.Count; i++)
            {
                AddTriggerEvent(enemySelectBtns[i].GetComponent<EventTrigger>(), EventTriggerType.PointerEnter, PointEnter);
                AddTriggerEvent(enemySelectBtns[i].GetComponent<EventTrigger>(), EventTriggerType.PointerExit, PointerExit);
            }
        }
    }

    void UpdateCurrentButton(EnemySelector_Button button)
    {
        if (selectButton != null)
            selectButton.UI_Reset();
        selectButton = button;
    }

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
