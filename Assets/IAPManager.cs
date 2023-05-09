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

    private const string _iOS_GoldId = "com.studio.app.gold"; // google play 개발자 포털에서 정하기
    private const string _android_GoldId = "com.studio.app.gold";

    private const string _iOS_GemId = "com.studio.app.gem"; // google play 개발자 포털에서 정하기
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

    private IStoreController storeController; // 구매 과정을 제어하는 함수 제공
    private IExtensionProvider storeExtensionProvider; // 여러 플랫폼을 위한 확장 처리를 제공


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

        if (args.purchasedProduct.definition.id == ProductGold)
        {
            Debug.Log("골드 증가");
        }
        else if (args.purchasedProduct.definition.id == ProductSkill)
        {
            Debug.Log("스킬 보유량 증가");
        }
        else if (args.purchasedProduct.definition.id == ProductSubscription)
        {
            Debug.Log("구독 서비스 시작");
        }

        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.LogWarning($"구매 실패 - {product.definition.id}, {reason}");
    }

    
}
