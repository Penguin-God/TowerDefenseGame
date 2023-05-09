using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public const string ProductGold = "gold"; // Consumable
    public const string ProductGem = "gem"; // Consumable
    public const string ProductSkill = "Skill"; // Unconsumable
    public const string ProductSubscription = "subscription"; // Subscription

    private const string _iOS_GoldId = "com.studio.app.gold"; // google play ������ ���п��� ���ϱ�
    private const string _android_GoldId = "com.studio.app.gold";

    private const string _iOS_GemId = "com.studio.app.gem"; // google play ������ ���п��� ���ϱ�
    private const string _android_GemId = "com.studio.app.gem";

    private const string _iOS_SkillId = "com.studio.app.skill";
    private const string _android_SkillId = "com.studio.app.skill";

    private const string _iOS_Subscription = "com.studio.app.sub";
    private const string _android_Subscription= "com.studio.app.sub";

    private static IAPManager m_instance;

    public static IAPManager Instance
    {
        get
        {
            if (m_instance != null) return m_instance;

            m_instance = FindObjectOfType<IAPManager>();

            if (m_instance == null) m_instance = new GameObject(name: "IAP Manager").AddComponent<IAPManager>();
            return m_instance;
        }
    }

    private IStoreController storeController; // ���� ������ �����ϴ� �Լ� ����
    private IExtensionProvider storeExtensionProvider; // ���� �÷����� ���� Ȯ�� ó���� ����


    void Awake()
    {
        InitUnityIAP();
    }

    void InitUnityIAP()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        
        builder.AddProduct(
            ProductGold, ProductType.Consumable,
            new IDs()
            {
                { _iOS_GoldId, AppleAppStore.Name},
                {_android_GoldId, GooglePlay.Name }
            }
       );

        builder.AddProduct(
            ProductGem, ProductType.Consumable,
            new IDs()
            {
                { _iOS_GemId, AppleAppStore.Name},
                {_android_GemId, GooglePlay.Name }
            }
       );

        builder.AddProduct(
            ProductSkill, ProductType.NonConsumable,
            new IDs()
            {
                { _iOS_SkillId, AppleAppStore.Name},
                {_android_SkillId, GooglePlay.Name }
            }
       );

        builder.AddProduct(
            ProductSkill, ProductType.Subscription,
            new IDs()
            {
                { _iOS_Subscription, AppleAppStore.Name},
                {_android_Subscription, GooglePlay.Name }
            }
       );

        UnityPurchasing.Initialize(this, builder);
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

        if (args.purchasedProduct.definition.id == ProductGold)
        {
            Debug.Log("��� ����");
        }
        else if (args.purchasedProduct.definition.id == ProductSkill)
        {
            Debug.Log("��ų ������ ����");
        }
        else if (args.purchasedProduct.definition.id == ProductSubscription)
        {
            Debug.Log("���� ���� ����");
        }

        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.LogWarning($"���� ���� - {product.definition.id}, {reason}");
    }

    
}
