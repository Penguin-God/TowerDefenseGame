using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelector_Button : UI_Base
{
    enum GameObjects
    {
        Offset,
    }

    [SerializeField] byte enemyNumber;
    public byte EnemyNumber => enemyNumber;
    [SerializeField] string enemyInfoText;
    Image image;
    Color selectColor = new Color32(150, 150, 150, 255);
    
    public void Setup(System.Action<EnemySelector_Button> action) 
    {
        image = GetComponent<Image>();
        Bind<GameObject>(typeof(GameObjects));
        infoWindowPos = GetObject((int)GameObjects.Offset).GetComponent<RectTransform>();

        GetComponent<Button>().onClick.AddListener(SelectSpawnEnemy);
        GetComponent<Button>().onClick.AddListener(() => action(this));
    }

    public void StartSelectSpawnEnemy()
    {
        image.color = selectColor;
    }

    void SelectSpawnEnemy()
    {
        image.color = selectColor;
        Managers.Sound.PlayEffect(EffectSoundType.EnemySelected);
    }

    public void UI_Reset()
    {
        image.color = Color.white;
    }

    RectTransform infoWindowPos;
    public void ShwoInfoWindow()
    {
        BackGround window = Managers.UI.ShowPopupUI<BackGround>("BackGround");
        window.SetPosition(infoWindowPos.position);
        window.SetText(enemyInfoText);
    }
}
