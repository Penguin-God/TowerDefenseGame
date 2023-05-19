using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UI_EnemySelector : UI_Scene
{
    [SerializeField] EnemySelector_Button currentSelectButton;
    protected override void Init()
    {
        List<EnemySelector_Button> enemySelectBtns = GetComponentsInChildren<EnemySelector_Button>().ToList();
        enemySelectBtns.ForEach(BindMousePointerEvents);
        // enemySelectBtns[0].StartSelectSpawnEnemy();
        // ChangeSpawnEnemy(enemySelectBtns[0]);
    }

    void BindMousePointerEvents(EnemySelector_Button btn)
    {
        btn.Setup(ChangeSpawnEnemy);
        var mouseOverHandler = btn.gameObject.AddComponent<MouseOverHandler>();
        mouseOverHandler.OnPointerEnterDelayedEvent += () => ShwoInfoWindow(btn);
        mouseOverHandler.OnPointerExitEvent += PointerExit;
    }

    EnemySpawnNumManager _enemySpawnNumManager;
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

    bool isShowInfoWindow;
    void PointerExit()
    {
        if (isShowInfoWindow)
        {
            isShowInfoWindow = false;
            Managers.UI.ClosePopupUI();
        }
    }

    void ShwoInfoWindow(EnemySelector_Button seleceButton)
    {
        seleceButton.ShwoInfoWindow(110f);
        isShowInfoWindow = true;
    }
}
