using TMPro;
using UnityEngine.UI;

public class UI_ShopProduct : UI_Base
{
    enum Texts
    {
        ProductText,
        PriceText,
    }

    enum Images
    {
        PriceImage,
    }
    
    protected override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));

        GetComponent<Button>().onClick.AddListener(ComfirmBuy);
    }

    PurchaseManager _purchaseManager;
    MoneyData _price;
    string _buyText;
    readonly MoneyPersenter _moneyPersenter = new();

    public void SetPurchaseManager(PurchaseManager purchaseManager) => _purchaseManager = purchaseManager;

    public void Refresh(MoneyData price, string productName)
    {
        CheckInit();
        _price = price;
        GetImage((int)Images.PriceImage).sprite = _moneyPersenter.GetMoneySprite(price.MoneyType);

        Refresh(price.Amount.ToString(), $"{_moneyPersenter.GetMoneyTextWithSuffix(_price)} 지불하여 {productName} 구매하시겠습니까?", productName);
    }
    public void InActivePriceImage() => GetImage((int)Images.PriceImage).gameObject.SetActive(false); 
    public void Refresh(string priceText, string buyText, string productName)
    {
        CheckInit();
        _buyText = buyText;

        GetTextMeshPro((int)Texts.ProductText).text = productName;
        GetTextMeshPro((int)Texts.PriceText).text = priceText;
    }

    void ComfirmBuy() => Managers.UI.ShowPopupUI<UI_ComfirmPopup>().SetInfo(_buyText, TryBuy);

    void TryBuy()
    {
        Managers.UI.ClosePopupUI();
        if (_purchaseManager.Purchase(_price) == false)
            Managers.UI.ShowPopupUI<UI_NotifyWindow>().SetMessage($"{_moneyPersenter.TypeTextWithSuffix(_price.MoneyType)} 부족합니다");
    }
}
