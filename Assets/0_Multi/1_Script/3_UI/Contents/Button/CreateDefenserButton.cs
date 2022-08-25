using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateDefenserButton : Multi_UI_Scene
{
    [SerializeField] int minColor;
    [SerializeField] int maxColor;
    protected override void Init()
    {
        base.Init();
        GetComponentInChildren<Button>().onClick.AddListener(Sommon);
    }

    void Sommon()
    {
        if (Multi_GameManager.instance.Gold >= 5)
        {
            if (Multi_GameManager.instance.UnitOver == false)
            {
                Multi_SpawnManagers.NormalUnit.Spawn(Random.Range(minColor, maxColor + 1), 0);
                Multi_GameManager.instance.AddGold(-5);
                Multi_Managers.Sound.PlayEffect(EffectSoundType.DrawSwordman);
            }
            else
            {
                Multi_Managers.UI.ShowPopupUI<WarningText>().Show("유닛 공간이 부족해 소환할 수 없습니다.");
                Multi_Managers.Sound.PlayEffect(EffectSoundType.Denger);
            }
        }
    }
}
