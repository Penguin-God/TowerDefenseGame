using TMPro;
using UnityEngine.UI;

public class UI_ShopProduct : UI_Base
{
    enum Texts
    {
        ProductText,
        PriceText,
    }

    protected override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        GetComponent<Button>().onClick.AddListener(ComfirmBuy);
    }

    PurchaseManager _purchaseManager;
    MoneyData _price;
    string _productName;
    readonly MoneyPersenter _moneyPersenter = new();
    public void DependencyInject(PurchaseManager purchaseManager, MoneyData price, string productName)
    {
        CheckInit();
        _price = price;

        DependencyInject(purchaseManager, _moneyPersenter.GetMoneyText(_price), productName);
    }

    public void DependencyInject(PurchaseManager purchaseManager, string priceText, string productName)
    {
        CheckInit();
        _purchaseManager = purchaseManager;
        _productName = productName;

        GetTextMeshPro((int)Texts.ProductText).text = productName;
        GetTextMeshPro((int)Texts.PriceText).text = $"���� : {priceText}";
    }

    void ComfirmBuy() => Managers.UI.ShowPopupUI<UI_ComfirmPopup>().SetInfo($"{_moneyPersenter.GetMoneyTextWithSuffix(_price)} �����Ͽ� {_productName}�� �����Ͻðڽ��ϱ�?", TryBuy);

    void TryBuy()
    {
        Managers.UI.ClosePopupUI();
        if (_purchaseManager.Purchase(_price) == false)
            Managers.UI.ShowPopupUI<UI_NotifyWindow>().SetMessage($"{_moneyPersenter.TypeTextWithSuffix(_price.MoneyType)} �����մϴ�");
    }
}
