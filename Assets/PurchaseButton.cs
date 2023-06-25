using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    public string targetProductId;

    public void Click()
    {
        if (targetProductId == IAPManager.ProductSkill || targetProductId == IAPManager.ProductSubscription) // NonConsumable���� ó��
        {
            if(IAPManager.Instance.HadPurchased(targetProductId))
            {
                Debug.Log("�̹� ������ ��ǰ");
                return;
            }
        }

        IAPManager.Instance.Purchase(targetProductId);
    }
}
