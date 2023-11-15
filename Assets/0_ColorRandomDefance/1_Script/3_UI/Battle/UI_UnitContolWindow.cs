using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitContolWindow : UI_Popup
{
    enum Buttons
    {
        BringWorldButton,
        ToTowerButton,
        SellButton,
    }

    enum GameObjects
    {
        Backgorund,
    }

    UnitManagerController _unitManager;
    public void DependencyInject(UnitManagerController unitManager) => _unitManager = unitManager;

    protected override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
    }

    public void SetButtonAction(UnitFlags flag)
    {
        CheckInit();

        GetButton((int)Buttons.BringWorldButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.ToTowerButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.SellButton).onClick.RemoveAllListeners();

        GetButton((int)Buttons.BringWorldButton).onClick.AddListener(BringUnit);
        GetButton((int)Buttons.ToTowerButton).onClick.AddListener(UnitToTower);
        GetButton((int)Buttons.SellButton).onClick.AddListener(SellUnit);

        void UnitToTower() => ChangeUnitWorld(true);
        void BringUnit() => ChangeUnitWorld(false);
        void ChangeUnitWorld(bool isDefense) 
            => _unitManager.GetUnits(PlayerIdManager.Id).Where(x => x.UnitFlags == flag && x.IsInDefenseWorld == isDefense).FirstOrDefault()?.ChangeUnitWorld();

        void SellUnit()
        {
            const int HighUnitMultiple = 2;
            if (_unitManager.TryFindUnit(PlayerIdManager.Id, unit => unit.UnitFlags == flag, out var findUnit))
            {
                int amount = Multi_GameManager.Instance.BattleData.UnitSellRewardDatas[(int)findUnit.UnitClass].Amount;
                if (UnitFlags.GetUnitType(findUnit.UnitColor) == UnitType.High) amount *= HighUnitMultiple;
                Multi_GameManager.Instance.AddGold(amount);
                findUnit.Dead();
            }
        }
    }

    public void SetPositioin(Vector2 pos) => GetObject((int)GameObjects.Backgorund).transform.position = pos;
}
