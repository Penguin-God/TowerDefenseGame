using UnityEngine;
using System;

public enum SkillBoxType
{
    희귀상자,
    고급상자,
    전설상자,
}

public class UI_LobbyShop : UI_Popup
{
    enum GameObjects
    {
        BoxGoodsParnet,
    }

    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        CreateProducts();
    }
    PlayerDataManager _playerDataManager;
    SkillDrawer _skillDrawer;
    IAPController _iapController;
    public void DependencyInject(PlayerDataManager playerDataManager, SkillDrawer skillDrawer, IAPController iapController)
    {
        _playerDataManager = playerDataManager;
        _skillDrawer = skillDrawer;
        _iapController = iapController;
    }

    void CreateProducts()
    {
        foreach (Transform child in GetObject((int)GameObjects.BoxGoodsParnet).transform)
            Destroy(child.gameObject);

        var boxDatas = Managers.Resources.LoadCsv<BoxProductData>("LobbyShopData/BoxData");
        foreach (var boxData in boxDatas)
            MakeGoods(new BoxPurchaseOperator(_skillDrawer, boxData.GetDrawInfos()), boxData.GetPriceData(), Enum.GetName(typeof(SkillBoxType), boxData.BoxType));

        var goldDatas = Managers.Resources.LoadCsv<GoldProductData>("LobbyShopData/GoldData");
        foreach (var goldData in goldDatas)
            MakeGoods(new GoldPurchaseOperator(goldData.GetGoldAmount()), goldData.GetPriceData(), new MoneyPersenter().GetMoneyText(new MoneyData(PlayerMoneyType.Gold, goldData.GetGoldAmount())));

        var iapDatas = Managers.Resources.LoadCsv<IAP_ProductData>("LobbyShopData/IAPData");
        foreach (var iapData in iapDatas)
            MakeGoods(new IAP_PurchaseOperator(iapData.ProductId, _iapController), new MoneyData(), new MoneyPersenter().GetMoneyText(new MoneyData(PlayerMoneyType.Gem, iapData.GemAmount)));
    }

    void MakeGoods(IPurchaseOperator purchase, MoneyData moneyData, string productName) 
        => Managers.UI.MakeSubItem<UI_ShopProduct>(GetObject((int)GameObjects.BoxGoodsParnet).transform)
            .DependencyInject(new PurchaseManager(purchase, _playerDataManager, new PlayerPrefabsSaver()), moneyData, productName);
}

public class MoneyPersenter
{
    public string GetMoneyText(MoneyData price) => $"{GetMoneyTypeText(price.MoneyType)} {price.Amount}{GetTextByType(price.MoneyType, "원", "개")}";
    public string GetMoneyTextWithSuffix(MoneyData price) => $"{GetMoneyText(price)}{GetAmountSuffix(price.MoneyType)}";

    public string GetMoneyTypeText(PlayerMoneyType moneyType) => GetTextByType(moneyType, "골드", "젬");
    public string TypeTextWithSuffix(PlayerMoneyType moneyType) => $"{GetMoneyTypeText(moneyType)}{GetTypeSuffix(moneyType)}";

    string GetTypeSuffix(PlayerMoneyType moneyType) => GetTextByType(moneyType, "가", "이");
    string GetAmountSuffix(PlayerMoneyType moneyType) => GetTextByType(moneyType, "을", "를");
    string GetTextByType(PlayerMoneyType type, string goldText, string gemText)
    {
        switch (type)
        {
            case PlayerMoneyType.Gold: return goldText;
            case PlayerMoneyType.Gem: return gemText;
            default: return "";
        }
    }
}
