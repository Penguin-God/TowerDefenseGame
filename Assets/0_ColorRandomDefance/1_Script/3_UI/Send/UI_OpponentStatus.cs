using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OpponentStatus : UI_Scene
{
    enum Texts
    {
        KnigthText,
        ArcherText,
        SpearmanText,
        MageText,
        OhterEnemyCountText,
        OtherUnitCountText,
    }

    protected override void Init()
    {
        base.Init();
        Bind<Text>(typeof(Texts));
        InitEvent();
    }

    void InitEvent()
    {
        Init_UI();
        BindOhterCountEvent();

        void Init_UI()
        {
            UpdateUnitCount(0);
            foreach (UnitClass _class in Enum.GetValues(typeof(UnitClass)))
                UpdateUnitClassByCount(_class, 0);
            UpdateMonsterCount(0);
        }

        void BindOhterCountEvent()
        {
            MultiServiceMidiator.Oppent.OnUnitCountChanged -= UpdateUnitCount;
            MultiServiceMidiator.Oppent.OnUnitCountChanged += UpdateUnitCount;

            MultiServiceMidiator.Oppent.OnUnitMaxCountChanged -= UpdateUnitMaxCount;
            MultiServiceMidiator.Oppent.OnUnitMaxCountChanged += UpdateUnitMaxCount;

            MultiServiceMidiator.Oppent.OnUnitCountChangedByClass -= UpdateUnitClassByCount;
            MultiServiceMidiator.Oppent.OnUnitCountChangedByClass += UpdateUnitClassByCount;

            Multi_EnemyManager.Instance.OnOtherEnemyCountChanged -= UpdateMonsterCount;
            Multi_EnemyManager.Instance.OnOtherEnemyCountChanged += UpdateMonsterCount;
        }
    }

    int _unitCount;
    public void UpdateUnitCount(int newCount)
    {
        _unitCount = newCount;
        UpdateUnitAllCount();
    }
    int _unitMaxCount;
    public void UpdateUnitMaxCount(int newCount)
    {
        _unitMaxCount = newCount;
        UpdateUnitAllCount();
    }
    void UpdateUnitAllCount() => GetText((int)Texts.OtherUnitCountText).text = $"{_unitCount}/{_unitMaxCount}";

    public void UpdateUnitClassByCount(UnitClass unitClass, int count)
    {
        GetText((int)GetTextsByClass(unitClass)).text = count.ToString(); 

        // 중첩 함수
        Texts GetTextsByClass(UnitClass unitClass)
        {
            switch (unitClass)
            {
                case UnitClass.Swordman: return Texts.KnigthText;
                case UnitClass.Archer: return Texts.ArcherText;
                case UnitClass.Spearman: return Texts.SpearmanText;
                case UnitClass.Mage: return Texts.MageText;
                default: throw new ArgumentException();
            }
        }
    }

    void UpdateMonsterCount(int count) => GetText((int)Texts.OhterEnemyCountText).text = $"{count}/{Multi_GameManager.Instance.BattleData.MaxEnemyCount}";
}
