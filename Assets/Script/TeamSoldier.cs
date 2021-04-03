using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TeamSoldier : MonoBehaviour
{
    public Button SoldierCombine;
    public Button SellSoldier;
  
    //private GameObject CombineSoldier;
    void Start()
    {
        SoldierCombine.GetComponent<Button>();
        SellSoldier.GetComponent<Button>();
        
    }

    
    void Update()
    {
       
    }

    private void OnMouseDown()
    {
        SoldierCombine.gameObject.SetActive(true);
        SellSoldier.gameObject.SetActive(true);
        GameManager.instance.Chilk();
        
    }

    public void CombineSolider()
    {

        SoldierCombine.gameObject.SetActive(false);
        SellSoldier.gameObject.SetActive(false);

    }

    public void SellSolider()
    {
        GameManager.instance.Gold += 3; 
        SoldierCombine.gameObject.SetActive(false);
        SellSoldier.gameObject.SetActive(false);
        Destroy(GameManager.instance.hit.transform.gameObject);
        

        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

    }
}
