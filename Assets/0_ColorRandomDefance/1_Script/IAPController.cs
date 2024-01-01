using UnityEngine;
using UnityEngine.Purchasing;

public class IAPController : IStoreListener
{
    //public const string ProductGold = "gold"; // Consumable
    //public const string ProductGem = "gem"; // Consumable
    //public const string ProductSkill = "skill"; // Unconsumable
    //public const string ProductSubscription = "sub"; // Subscription

    //private const string _iOS_GoldId = "com.studio.app.gold"; // google play ������ ���п��� ���ϱ�
    //private const string _android_GoldId = "com.studio.app.gold";

    //private const string _iOS_GemId = "com.studio.app.gem"; // google play ������ ���п��� ���ϱ�
    //private const string _android_GemId = "com.studio.app.gem";

    //private const string _iOS_SkillId = "com.studio.app.skill";
    //private const string _android_SkillId = "com.studio.app.skill";

    //private const string _iOS_Subscription = "com.studio.app.sub";
    //private const string _android_Subscription= "com.studio.app.sub";

    private IStoreController storeController; // ���� ������ �����ϴ� �Լ� ����
    private IExtensionProvider storeExtensionProvider; // ���� �÷����� ���� Ȯ�� ó���� ����

    bool IsInitialized => storeController != null && storeExtensionProvider != null;

    public IAPController()
    {
        InitUnityIAP();
    }

    void InitUnityIAP()
    {
        if (IsInitialized) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // builder.AddProduct(
        //     ProductGold, ProductType.Consumable,
        //     new IDs()
        //     {
        //         { _iOS_GoldId, AppleAppStore.Name},
        //         {_android_GoldId, GooglePlay.Name }
        //     }
        //);

        // builder.AddProduct(
        //     ProductGem, ProductType.Consumable,
        //     new IDs()
        //     {
        //         { _iOS_GemId, AppleAppStore.Name},
        //         {_android_GemId, GooglePlay.Name }
        //     }
        //);

        // builder.AddProduct(
        //     ProductSkill, ProductType.NonConsumable,
        //     new IDs()
        //     {
        //         { _iOS_SkillId, AppleAppStore.Name},
        //         {_android_SkillId, GooglePlay.Name }
        //     }
        //);

        // builder.AddProduct(
        //     ProductSkill, ProductType.Subscription,
        //     new IDs()
        //     {
        //         { _iOS_Subscription, AppleAppStore.Name},
        //         {_android_Subscription, GooglePlay.Name }
        //     }
        //);

        AddProduct("100_gems");
        AddProduct("500_gems");
        AddProduct("1000_gems");
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

        //if (args.purchasedProduct.definition.id == ProductGold)
        //{
        //    Debug.Log("��� ����");
        //}
        //else if (args.purchasedProduct.definition.id == ProductSkill)
        //{
        //    Debug.Log("��ų ������ ����");
        //}
        //else if (args.purchasedProduct.definition.id == ProductSubscription)
        //{
        //    Debug.Log("���� ���� ����");
        //}

        if (args.purchasedProduct.definition.id == "100_gems")
        {
            GiveGemsToUser(100);
        }
        else if (args.purchasedProduct.definition.id == "500_gems")
        {
            GiveGemsToUser(500);
        }
        else if (args.purchasedProduct.definition.id == "1000_gems")
        {
            GiveGemsToUser(1000);
        }

        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.LogWarning($"���� ���� - {product.definition.id}, {reason}");
    }

    void GiveGemsToUser(int gem)
    {
        Debug.Log($"�� {gem}�� ȹ��");
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
