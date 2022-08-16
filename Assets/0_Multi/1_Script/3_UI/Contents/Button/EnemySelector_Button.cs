using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnemySelector_Button : Multi_UI_Base
{
    [SerializeField] int enemyNumber;
    [SerializeField] string enemyInfoText;
    
    public void SelectSpawnEnemy(Color color)
    {
        Multi_SpawnManagers.NormalEnemy.SetOtherEnemyNumber(enemyNumber);
        GetComponent<Image>().color = color;
    }

    [SerializeField] Vector3 offset;
    public void ShwoInfoWindow()
    {
        BackGround window = Multi_Managers.UI.ShowPopupUI<BackGround>("BackGround");
        window.SetPosition(transform.position + offset);
        window.SetText(enemyInfoText);
    }
}
