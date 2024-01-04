using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPController : IStoreListener
{
    private IStoreController storeController; // ���� ������ �����ϴ� �Լ� ����
    private IExtensionProvider storeExtensionProvider; // ���� �÷����� ���� Ȯ�� ó���� ����

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
        Debug.Log("����Ƽ IAP �ʱ�ȭ ����");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"����Ƽ IAP �ʱ�ȭ ���� {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"����Ƽ IAP �ʱ�ȭ ���� {error}, {message}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log($"���� ���� - ID : {args.purchasedProduct.definition.id}"); // ������ ��ǰ�� ���̵�

        if(_gemAmountById.TryGetValue(args.purchasedProduct.definition.id, out int amount))
            GiveGemsToUser(amount);

        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.LogWarning($"���� ���� - {product.definition.id}, {reason}");
    }

    void GiveGemsToUser(int gem)
    {
        _playerDataManager.Gem.Add(gem);
    }

    public void Purchase(string productId) // ���� �õ�
    {
        if (!IsInitialized) return;

        var product = storeController.products.WithID(productId); // id�� ��ǰ�� ������

        if (product != null && product.availableToPurchase) 
        {
            Debug.Log($"���� �õ� {product.definition.id}");
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.Log($"���� �õ� �Ұ� {productId}");
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
