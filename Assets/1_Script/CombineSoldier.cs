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
        if (GameManager.instance.hitSolider.gameObject.name == "TB_Soldier_Swordman_Red(Clone)" && GameManager.instance.RedSwordman.Length >=2)
        {
            //GameManager.instance.SoldiersTag();
            //Destroy(GameManager.instance.RedSwordman[0]);
            Destroy(GameManager.instance.RedSwordman[0]);
            Destroy(GameManager.instance.RedSwordman[1]);


            SoldierChoose(0, 0, 1, 1);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);
            UIManager.instance.SetActiveButton(false);

        }
    }

    public void ButtonOn()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.instance.Chilk();
        }
        //if (GameManager.instance.hitSolider.transform.gameObject.tag == "Swordman" && GameManager.instance.hitSolider.gameObject.name == "TB_Soldier_Swordman_Red")
        //{
            //UIManager.instance.SetActiveButton(true);
        //}
        UIManager.instance.SetActiveButton(true);
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
