using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPController : IStoreListener
{
    private IStoreController storeController; // 구매 과정을 제어하는 함수 제공
    private IExtensionProvider storeExtensionProvider; // 여러 플랫폼을 위한 확장 처리를 제공

    bool IsInitialized => storeController != null && storeExtensionProvider != null;

    Dictionary<string, int> _gemAmountById = new Dictionary<string, int>();

    readonly PlayerDataManager _playerDataManager;
    public IAPController(IEnumerable<IAP_ProductData> datas, PlayerDataManager playerDataManager)
    {
        _playerDataManager = playerDataManager;
        _gemAmountById = datas.ToDictionary(x => x.ProductId, x => x.GemAmount);
        InitUnityIAP(_gemAmountById.Keys);
    }

    void InitUnityIAP(IEnumerable<string> ids)
    {
        if (IsInitialized) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (string id in ids)
            AddProduct(id);

        UnityPurchasing.Initialize(this, builder);

        void AddProduct(string playStroreId) => builder.AddProduct(playStroreId, ProductType.Consumable, new IDs() { { $"android_{playStroreId}", GooglePlay.Name } });
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
        Debug.Log("유니티 IAP 초기화 성공");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"유니티 IAP 초기화 실패 {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"유니티 IAP 초기화 실패 {error}, {message}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log($"구매 성공 - ID : {args.purchasedProduct.definition.id}"); // 구매한 상품의 아이디

        if(_gemAmountById.TryGetValue(args.purchasedProduct.definition.id, out int amount))
            GiveGemsToUser(amount);

        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.LogWarning($"구매 실패 - {product.definition.id}, {reason}");
    }

    void GiveGemsToUser(int gem)
    {
        _playerDataManager.Gem.Add(gem);
    }

    public void Purchase(string productId) // 구매 시도
    {
        if (!IsInitialized) return;

        var product = storeController.products.WithID(productId); // id의 상품을 가져옴

        if (product != null && product.availableToPurchase) 
        {
            Debug.Log($"구매 시도 {product.definition.id}");
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.Log($"구매 시도 불가 {productId}");
        }
    }

    //public bool HadPurchased(string productId)
    //{
    //    if (!IsInitialized) return false;

    //    var product = storeController.products.WithID(productId);

    //    if (product != null)
    //    {
    //        return product.hasReceipt;
    //    }

    //    return false;
    //}
}
