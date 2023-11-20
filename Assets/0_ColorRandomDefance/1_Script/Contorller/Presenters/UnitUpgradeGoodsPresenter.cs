using System.Collections;
using System.Collections.Generic;

public static class UnitUpgradeGoodsPresenter
{   
    public static string BuildGoodsText(UnitUpgradeData upgradeGoods) => $"{UnitTextPresenter.GetColorText(upgradeGoods.TargetColor)} 유닛 {GetUpgradeText(upgradeGoods)} 증가";
    static string GetUpgradeText(UnitUpgradeData upgradeData) => $"공격력 {upgradeData.Value}" + (upgradeData.UpgradeType == UnitUpgradeType.Value ? "" : "%");

    static string GetUpgradeText(UnitUpgradeGoodsData upgradeData) => $"공격력 {upgradeData.Value}" + (upgradeData.UpgradeType == UnitUpgradeType.Value ? "" : "%");
    public static string BuildUpgradeInfoText(UnitUpgradeGoodsData upgradeGoodsData)
        => $"{new GameCurrencyPresenter().BuildCurrencyText(upgradeGoodsData.Price)} {GetUpgradeText(upgradeGoodsData)} 증가";
}