using System.Collections;
using System.Collections.Generic;

public static class UnitUpgradeGoodsPresenter
{   
    public static string BuildGoodsText(UnitUpgradeData upgradeGoods) => $"{UnitTextPresenter.GetColorText(upgradeGoods.TargetColor)} 유닛 {GetUpgradeText(upgradeGoods)} 증가";
    static string GetUpgradeText(UnitUpgradeData upgradeData) => $"공격력 {upgradeData.Value}" + (upgradeData.UpgradeType == UnitUpgradeType.Value ? "" : "%");

    public static string BuildAddUpgradeText(UnitUpgradeGoodsData upgradeGoodsData) => $"{new GameCurrencyPresenter().BuildCurrencyText(upgradeGoodsData.Price)} {upgradeGoodsData.UpgradeInfo.BaseDamage} 증가";
    public static string BuildScaleUpgradeText(UnitUpgradeGoodsData upgradeGoodsData) => $"{new GameCurrencyPresenter().BuildCurrencyText(upgradeGoodsData.Price)} {upgradeGoodsData.UpgradeInfo.DamageRate * 100}% 증가";
}