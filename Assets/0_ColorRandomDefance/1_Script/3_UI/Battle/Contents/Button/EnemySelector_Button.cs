using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelector_Button : UI_Base
{
    [SerializeField] byte enemyNumber;
    public byte EnemyNumber => enemyNumber;
    [SerializeField] string enemyInfoText;
    Image image;
    Color selectColor = new Color32(150, 150, 150, 255);
    
    public void Setup(System.Action<EnemySelector_Button> action) 
    {
        image = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(SelectSpawnEnemy);
        GetComponent<Button>().onClick.AddListener(() => action(this));
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

    public void ShwoInfoWindow(float offsetY)
    {
        BackGround window = Managers.UI.ShowPopupUI<BackGround>("BackGround");
        float screenScaleFactor = Screen.height / Managers.UI.UIScreenHeight;
        window.SetPosition(transform.position + new Vector3(0, offsetY * screenScaleFactor, 0));
        window.SetFontSize(27);
        window.SetText(enemyInfoText);
    }
}
