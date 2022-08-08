using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelector_UI : Multi_UI_Base
{
    [SerializeField] Color selectColor;
    [SerializeField] Button selectButton;
    protected override void Init()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() => SetSpawnEnemyNumber(3, buttons[0]));
        buttons[1].onClick.AddListener(() => SetSpawnEnemyNumber(0, buttons[1]));
        buttons[2].onClick.AddListener(() => SetSpawnEnemyNumber(2, buttons[2]));
        buttons[3].onClick.AddListener(() => SetSpawnEnemyNumber(1, buttons[3]));
    }

    void SetSpawnEnemyNumber(int num, Button button)
    {
        Multi_SpawnManagers.NormalEnemy.SetOtherEnemyNumber(num);
        if (selectButton != null)
            selectButton.GetComponent<Image>().color = new Color(255, 255, 255);
        selectButton = button;
        button.GetComponent<Image>().color = selectColor;
    }
}
