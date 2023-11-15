using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ObjectCountDisplay : UI_Base
{
    enum Texts
    {
        KnigthCountText,
        ArcherCountText,
        SpearmanCountText,
        MageCountText,
        UnitCountText,
        MonsterCountText,
    }

    protected override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));

        foreach (UnitClass _class in Enum.GetValues(typeof(UnitClass)))
            UpdateUnitClassByCount(_class, 0);
        UpdateCurrentUnitText(0);
    }

    int _currentUnitCount;
    int _maxUnitCount;
    void UpdateUnitCountText() => GetTextMeshPro((int)Texts.UnitCountText).text = $"{_currentUnitCount}/{_maxUnitCount}";    
    public void UpdateCurrentUnitText(int count)
    {
        _currentUnitCount = count;
        UpdateUnitCountText();
    }
    public void UpdateMaxUnitCount(int count)
    {
        _maxUnitCount = count;
        UpdateUnitCountText();
    }


    public void UpdateUnitClassByCount(UnitClass unitClass, int count)
    {
        GetTextMeshPro((int)GetTextsByClass(unitClass)).text = count.ToString();

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
    public void UpdateMonsterCountText(int count)
    {
        var text = GetTextMeshPro((int)Texts.MonsterCountText);
        if (count > 40)
        {
            text.color = DENGER_COLOR;
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
        else text.color = Color.white;
        text.text = $"{count}/{Multi_GameManager.Instance.BattleData.MaxEnemyCount}";
    }
}
