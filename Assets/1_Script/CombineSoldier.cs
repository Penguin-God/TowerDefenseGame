using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSoldier : MonoBehaviour
{
   

    void Start()
    {
        
    }

    
    void Update()
    {
        
        
    }

    private void Combine()
    {
        int i;
        GameObject[] SoldierColor = GameObject.FindGameObjectsWithTag("Red");
        for (i = 0; i <= SoldierColor.Length; i++)
        {
            Debug.Log(SoldierColor[i]);
        }
    }

    public void ButtonOn()
    {
        if (GameManager.instance.hitSolider.transform.parent.name == "Swordman")
        {
            UIManager.instance.SetActiveButton(true);
        }
    }
}
