using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellDefenser : MonoBehaviour
{
    public void SellSolider()
    {
        GameManager.instance.Gold += 3;
        Destroy(GameManager.instance.hitSolider);
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
        UIManager.instance.ButtonDown();
    }
}
