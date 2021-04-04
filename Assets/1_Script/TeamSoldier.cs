using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TeamSoldier : MonoBehaviour
{
    public DefenserManager defenserManager;
    public int sellPrice; // 유닛에 따른 판매가격
    //public Button SoldierCombine;
    //public Button SellSoldier;

    private void OnMouseDown()
    {
        defenserManager.SetActiveButton(true);
        defenserManager.Chilk();
    }

    //void Start()
    //{
    //    SoldierCombine.GetComponent<Button>();
    //    SellSoldier.GetComponent<Button>();

    //}

    //public void CombineSolider()
    //{

    //    SoldierCombine.gameObject.SetActive(false);
    //    SellSoldier.gameObject.SetActive(false);

    //}

    //public void SellSolider()
    //{
    //    GameManager.instance.Gold += 3; 
    //    SoldierCombine.gameObject.SetActive(false);
    //    SellSoldier.gameObject.SetActive(false);
    //    Destroy(GameManager.instance.hitSoldier);


    //    UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

    //}
}
