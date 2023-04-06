using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UI_EnemySelector : UI_Base
{
    [SerializeField] EnemySelector_Button currentSelectButton;
    protected override void Init()
    {
        List<EnemySelector_Button> enemySelectBtns = GetComponentsInChildren<EnemySelector_Button>().ToList();
        enemySelectBtns.ForEach(x => {
            x.Setup(ChangeSpawnEnemy);
            var mouseOverHandler = x.gameObject.AddComponent<MouseOverHandler>();
            mouseOverHandler.OnPointerEnterDelayedEvent += () => ShwoInfoWindow(x);
            mouseOverHandler.OnPointerExitEvent += PointerExit;
        });

        enemySelectBtns[0].StartSelectSpawnEnemy();
        ChangeSpawnEnemy(enemySelectBtns[0]);
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
