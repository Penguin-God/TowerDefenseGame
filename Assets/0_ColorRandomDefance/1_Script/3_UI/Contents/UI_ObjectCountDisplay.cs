using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ObjectCountDisplay : UI_Base
{
    enum Texts
    {
        KnigthCountText,
        ArcherCountText,
        SpearmanCountText,
        MageCountText,
        CurrentUnitCountText,
        MaxUnitCountText,
        MonsterCountText,
    }

    protected override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    public void BindEvent(Action<int> currentUnitCountChange, Action<int> maxUnitCountChange, Action<UnitClass, int> unitClassCountChange, Action<int> monsterCountChange)
    {
        currentUnitCountChange += UpdateCurrentUnitText;
        maxUnitCountChange += UpdateMaxUnitCount;
        unitClassCountChange += UpdateUnitClassByCount;

        monsterCountChange += UpdateMonsterCountText;
    }

    void UpdateMaxUnitCount(int count) => GetText((int)Texts.MaxUnitCountText).text = count.ToString();

    void UpdateCurrentUnitText(int count) => GetText((int)Texts.CurrentUnitCountText).text = count.ToString();
    void UpdateUnitClassByCount(UnitClass unitClass, int count)
    {
        GetText((int)GetTextsByClass(unitClass)).text = count.ToString();

        // ÁßÃ¸ ÇÔ¼ö
        Texts GetTextsByClass(UnitClass unitClass)
        {
            switch (unitClass)
            {
                case UnitClass.Swordman: return Texts.KnigthCountText;
                case UnitClass.Archer: return Texts.ArcherCountText;
                case UnitClass.Spearman: return Texts.SpearmanCountText;
                case UnitClass.Mage: return Texts.MageCountText;
                default: throw new ArgumentException();
            }
        }
    }


    readonly Color DENGER_COLOR = Color.red;
    void UpdateMonsterCountText(int count)
    {
        Text text = GetText((int)Texts.MonsterCountText);
        if (count > 40)
        {
            text.color = DENGER_COLOR;
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
        else text.color = Color.white;
        text.text = $"{count}, {Multi_GameManager.Instance.BattleData.MaxEnemyCount}";
    }
}
