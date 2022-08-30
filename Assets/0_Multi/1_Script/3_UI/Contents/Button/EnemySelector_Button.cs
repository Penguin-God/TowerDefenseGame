using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnemySelector_Button : Multi_UI_Base
{
    enum GameObjects
    {
        Offset,
    }

    [SerializeField] int enemyNumber;
    [SerializeField] string enemyInfoText;
    Image image;
    Color selectColor;
    Color originColor = new Color(1, 1, 1, 1);

    public void Setup(Color color, System.Action<EnemySelector_Button> action)
    {
        image = GetComponent<Image>();
        selectColor = color;

        Bind<GameObject>(typeof(GameObjects));
        infoWindowPos = GetObject((int)GameObjects.Offset).GetComponent<RectTransform>();

        GetComponent<Button>().onClick.AddListener(SelectSpawnEnemy);
        GetComponent<Button>().onClick.AddListener(() => action(this));
    }

    public void StartSelectSpawnEnemy()
    {
        Multi_SpawnManagers.NormalEnemy.SetOtherEnemyNumber(enemyNumber);
        image.color = selectColor;
    }

    void SelectSpawnEnemy()
    {
        Multi_SpawnManagers.NormalEnemy.SetOtherEnemyNumber(enemyNumber);
        image.color = selectColor;
        Multi_Managers.Sound.PlayEffect(EffectSoundType.EnemySelected);
    }

    public void UI_Reset()
    {
        image.color = originColor;
    }

    RectTransform infoWindowPos;
    public void ShwoInfoWindow()
    {
        BackGround window = Multi_Managers.UI.ShowPopupUI<BackGround>("BackGround");
        window.SetPosition(infoWindowPos.position);
        window.SetText(enemyInfoText);
    }
}
