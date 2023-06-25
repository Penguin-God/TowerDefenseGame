using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSub : MonoBehaviour
{
    IEnumerator start()
    {
        yield return new WaitForSeconds(3f);

        if (IAPManager.Instance.HadPurchased(IAPManager.ProductSubscription))
        {
            Debug.Log("±¸µ¶ Áß");
        }
    }
}
