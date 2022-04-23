using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellDefenser : MonoBehaviour
{
    public UnitManageButton unitManageButton;
    public void SellSolider()
    {
        GameManager.instance.Gold += 3;
        Destroy(GameManager.instance.HitEnemy);
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
        unitManageButton.gameObject.SetActive(true);
        UIManager.instance.ButtonDown();
    }
}
