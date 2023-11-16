using System.Collections;
using System.Collections.Generic;

public class UnitUpgradeGoodsPresenter
{   
    public string BuildGoodsText(UnitUpgradeData upgradeGoods) => $"{UnitTextPresenter.GetColorText(upgradeGoods.TargetColor)} 유닛 {GetUpgradeText(upgradeGoods)} 증가";
    string GetUpgradeText(UnitUpgradeData upgradeData) => $"공격력 {upgradeData.Value}" + (upgradeData.UpgradeType == UnitUpgradeType.Value ? "" : "%");
}