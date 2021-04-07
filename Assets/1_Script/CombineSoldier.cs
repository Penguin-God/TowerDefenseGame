using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSoldier : MonoBehaviour
{
    private int Colornumber;
    private int Soldiernumber;
    public CreateDefenser createdefenser;

    void Start()
    {
        
    }

    
    void Update()
    {
        
        
    }

    public void Combine()
    {
        if (GameManager.instance.hitSolider.gameObject.tag == "Swardman")
        {

        }
    }

    public void ButtonOn()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.instance.Chilk();
        }
        if (GameManager.instance.hitSolider.transform.gameObject.tag == "Swordman")
        {
            UIManager.instance.SetActiveButton(true);
        }
    }

    private void SoldierChoose(int Colornumber1, int Colornumber2, int Soldiernumber1,int Soldiernumber2)
    {
        Colornumber = Random.Range(Colornumber1, Colornumber2);
        Soldiernumber = Random.Range(Soldiernumber1, Soldiernumber2);
        
    }

    public void Sommon()
    {
        SoldierChoose(0, 3, 0, 0);
        createdefenser.DrawSoldier(Colornumber, Soldiernumber);
        // createdefenser.CreateSoldier(Colornumber, Soldiernumber);
        // createdefenser.ExpenditureGold();
    }
}
