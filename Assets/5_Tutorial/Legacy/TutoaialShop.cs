using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoaialShop : TutorialGuideTrigger
{
    [SerializeField] Shop shop = null;
    //[SerializeField] GameObject ShopObject = null;

    bool isOnShop = false;
    public override bool TutorialCondition()
    {
        StartCoroutine(Co_OnShop());
        return isOnShop;
    }

    // Shop은 처음에 켜졌다가 상점 정보를 세팅하고 꺼지기 때문에 0.1초 대기 후 켜지면 다른 변수에 true 대입
    IEnumerator Co_OnShop()
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !shop.gameObject.activeSelf);
        yield return new WaitUntil(() => shop.gameObject.activeSelf);
        isOnShop = true;
    }
}
