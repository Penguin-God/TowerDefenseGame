using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TeamSoldier : MonoBehaviour
{
    public SellDefenser sellDefenser;
    //private GameObject CombineSoldier;


    void Update()
    {

    }

    private void OnMouseDown()
    {
        UIManager.instance.SetActiveButton(true);
        GameManager.instance.Chilk();

    }

    //public void CombineSolider()
    //{

    //    sellDefenser.SetActiveButton(false);

    //}
}