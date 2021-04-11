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
        UIManager.instance.SetActiveButton(false,0,0);
        UIManager.instance.SetActiveButton(false, 1, 0);
        UIManager.instance.SetActiveButton(false, 2, 0);
    }
}
