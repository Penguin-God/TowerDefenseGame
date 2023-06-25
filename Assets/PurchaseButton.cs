using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    public string targetProductId;

    public void Click()
    {
        if (targetProductId == IAPManager.ProductSkill || targetProductId == IAPManager.ProductSubscription) // NonConsumable구매 처리
        {
            if(IAPManager.Instance.HadPurchased(targetProductId))
            {
                Debug.Log("이미 구매한 상품");
                return;
            }
        }

        IAPManager.Instance.Purchase(targetProductId);
    }
}
