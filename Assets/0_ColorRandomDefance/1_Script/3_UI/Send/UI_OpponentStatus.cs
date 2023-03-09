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
            UpdateUnitClassByCount();
            UpdateMonsterCount(0);
        }

        void BindOhterCountEvent()
        {
            Multi_UnitManager.Instance.OnOtherUnitCountChanged -= UpdateUnitCount;
            Multi_UnitManager.Instance.OnOtherUnitCountChanged += UpdateUnitCount;

            Multi_UnitManager.Instance.OnOtherUnitCountChanged -= (count) => UpdateUnitClassByCount();
            Multi_UnitManager.Instance.OnOtherUnitCountChanged += (count) => UpdateUnitClassByCount();

            Multi_EnemyManager.Instance.OnOtherEnemyCountChanged -= UpdateMonsterCount;
            Multi_EnemyManager.Instance.OnOtherEnemyCountChanged += UpdateMonsterCount;
        }
    }

    // TODO : Multi_GameManager.instance.MaxUnitCount 각 플레이어걸로
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

    public void UpdateUnitClassByCount()
    {
        GetText((int)Texts.KnigthText).text = "" + GetCountByClass(UnitClass.Swordman);
        GetText((int)Texts.ArcherText).text = "" + GetCountByClass(UnitClass.Archer);
        GetText((int)Texts.SpearmanText).text = "" + GetCountByClass(UnitClass.Spearman);
        GetText((int)Texts.MageText).text = "" + GetCountByClass(UnitClass.Mage);

        int GetCountByClass(UnitClass unitClass) => Multi_UnitManager.Instance.EnemyPlayerUnitCountByClass[unitClass];
    }

    void UpdateMonsterCount(int count) => GetText((int)Texts.OhterEnemyCountText).text = $"{count}/{Multi_GameManager.Instance.BattleData.MaxEnemyCount}";
}
