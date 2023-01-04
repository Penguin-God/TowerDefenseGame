using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class UI_EnemySelector : UI_Base
{
    [SerializeField] EnemySelector_Button currentSelectButton;
    EnemySpawnNumManager _enemySpawnNumManager;
    protected override void Init()
    {
        List<EnemySelector_Button> enemySelectBtns = GetComponentsInChildren<EnemySelector_Button>().ToList();
        enemySelectBtns.ForEach(x => x.Setup(ChangeSpawnEnemy));
        enemySelectBtns[0].StartSelectSpawnEnemy();
        ChangeSpawnEnemy(enemySelectBtns[0]);

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

    public void SetInfo(EnemySpawnNumManager enemySpawnNumManager) => _enemySpawnNumManager = enemySpawnNumManager;

    void ChangeSpawnEnemy(EnemySelector_Button button)
    {
        if (currentSelectButton != button)
        {
            _enemySpawnNumManager.SetSpawnNumber(button.EnemyNumber);
            currentSelectButton?.UI_Reset();
            currentSelectButton = button;
        }
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
            Managers.UI.ClosePopupUI();
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
