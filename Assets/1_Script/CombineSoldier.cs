using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSoldier : MonoBehaviour
{
    private int Colornumber;
    private int Soldiernumber;
    public CreateDefenser createdefenser;
    public SoldiersTags TagSoldier;
    

    void Start()
    {
        TagSoldier = GetComponent<SoldiersTags>();
    }

    
    void Update()
    {
        
        
    }

    public void Combine()
    {
        if (GameManager.instance.hitSolider.gameObject.name == "TB_Soldier_Swordman_Red(Clone)" )
        {
            TagSoldier.RedSwordmanTag();
            if (TagSoldier.RedSwordman.Length >= 2)
            {
                
                Destroy(TagSoldier.RedSwordman[0]);
                Destroy(TagSoldier.RedSwordman[1]);


                SoldierChoose(0, 0, 1, 1);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.SetActiveButton(false,0,0);
            }
        }

        if (GameManager.instance.hitSolider.gameObject.name == "TB_Soldier_Swordman_Blue(Clone)")
        {
            TagSoldier.BlueSwordmanTag();
            if (TagSoldier.BlueSwordman.Length >= 2)
            {

                Destroy(TagSoldier.BlueSwordman[0]);
                Destroy(TagSoldier.BlueSwordman[1]);


                SoldierChoose(1, 1, 1, 1);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.SetActiveButton(false,1,0);
            }
        }

        if (GameManager.instance.hitSolider.gameObject.name == "TB_Soldier_Swordman_Yellow(Clone)")
        {
            TagSoldier.YellowSwordmanTag();
            if (TagSoldier.YellowSwordman.Length >= 2)
            {

                Destroy(TagSoldier.YellowSwordman[0]);
                Destroy(TagSoldier.YellowSwordman[1]);


                SoldierChoose(2, 2, 1, 1);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.SetActiveButton(false,2,0);
            }
        }

        //if (GameManager.instance.hitSolider.gameObject.name == "TB_Soldier_Swordman_Green(Clone)")
        //{
            //TagSoldier.BlueSwordmanTag();
            //TagSoldier.YellowSwordmanTag();
            //if (TagSoldier.GreenSwordman.Length >= 2)
            //{

                //Destroy(TagSoldier.YellowSwordman[0]);
                //Destroy(TagSoldier.BlueSwordman[0]);


                //SoldierChoose(3, 3, 0, 0);
                //createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                //UIManager.instance.SetActiveButton(false);
            //}
        //}

        if (GameManager.instance.hitSolider.gameObject.name == "TB_Soldier_Archer_Red(Clone)") 
        {
            TagSoldier.RedSwordmanTag();
            TagSoldier.RedArcherTag();
            if (TagSoldier.RedSwordman.Length >= 2 && TagSoldier.RedArcher.Length >=1)
            {

                Destroy(TagSoldier.RedSwordman[0]);
                Destroy(TagSoldier.RedSwordman[1]);
                Destroy(TagSoldier.RedArcher[0]);
                Destroy(TagSoldier.RedArcher[1]);


                SoldierChoose(0, 0, 2, 2);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.SetActiveButton(false,0,1);
            }
        }

        if (GameManager.instance.hitSolider.gameObject.name == "TB_Soldier_Archer_Blue(Clone)")
        {
            TagSoldier.BlueSwordmanTag();
            TagSoldier.BlueArcherTag();
            if (TagSoldier.BlueSwordman.Length >= 2 && TagSoldier.BlueArcher.Length >= 1)
            {

                Destroy(TagSoldier.BlueSwordman[0]);
                Destroy(TagSoldier.BlueSwordman[1]);
                Destroy(TagSoldier.BlueArcher[0]);
                Destroy(TagSoldier.BlueArcher[1]);


                SoldierChoose(1, 1, 2, 2);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.SetActiveButton(false,1,1);
            }
        }

        if (GameManager.instance.hitSolider.gameObject.name == "TB_Soldier_Archer_Yellow(Clone)")
        {
            TagSoldier.YellowSwordmanTag();
            TagSoldier.YellowArcherTag();
            if (TagSoldier.YellowSwordman.Length >= 2 && TagSoldier.YellowArcher.Length >= 1)
            {

                Destroy(TagSoldier.YellowSwordman[0]);
                Destroy(TagSoldier.YellowSwordman[1]);
                Destroy(TagSoldier.YellowArcher[0]);
                Destroy(TagSoldier.YellowArcher[1]);


                SoldierChoose(2, 2, 2, 2);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.SetActiveButton(false,2,1);
            }
        }

        if (GameManager.instance.hitSolider.gameObject.name == "TB_Soldier_Spearman_Red(Clone)")
        {
            TagSoldier.RedSpearmanTag();
            if (TagSoldier.RedSpearman.Length >= 2)
            {

                Destroy(TagSoldier.RedSpearman[0]);
                Destroy(TagSoldier.RedSpearman[1]);


                SoldierChoose(0, 0, 3, 3);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.SetActiveButton(false,0,2);
            }
        }

        if (GameManager.instance.hitSolider.gameObject.name == "TB_Soldier_Spearman_Blue(Clone)")
        {
            TagSoldier.BlueSpearmanTag();
            if (TagSoldier.BlueSpearman.Length >= 2)
            {

                Destroy(TagSoldier.BlueSpearman[0]);
                Destroy(TagSoldier.BlueSpearman[1]);


                SoldierChoose(1, 1, 3, 3);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.SetActiveButton(false,1,2);
            }
        }

        if (GameManager.instance.hitSolider.gameObject.name == "TB_Soldier_Spearman_Yellow(Clone)")
        {
            TagSoldier.YellowSpearmanTag();
            if (TagSoldier.YellowSpearman.Length >= 2)
            {

                Destroy(TagSoldier.YellowSpearman[0]);
                Destroy(TagSoldier.YellowSpearman[1]);


                SoldierChoose(2, 2, 3, 3);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.SetActiveButton(false,2,2);
            }
        }
    }


    private void SoldierChoose(int Colornumber1, int Colornumber2, int Soldiernumber1,int Soldiernumber2)
    {
        Colornumber = Random.Range(Colornumber1, Colornumber2);
        Soldiernumber = Random.Range(Soldiernumber1, Soldiernumber2);
        
    }

    public void ButtonOn()
    {

        GameManager.instance.Chilk();
        UIManager.instance.ButtonDown();
        if (GameManager.instance.hitSolider.transform.gameObject.tag == "RedSwordman")
        {
            UIManager.instance.SetActiveButton(true, 0, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "BlueSwordman")
        {
            UIManager.instance.SetActiveButton(true, 1, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "YellowSwordman")
        {
            UIManager.instance.SetActiveButton(true, 2, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "GreenSwordman")
        {
            UIManager.instance.SetActiveButton(true, 3, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "RedArcher")
        {
            UIManager.instance.SetActiveButton(true, 0, 1);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "BlueArcher")
        {
            UIManager.instance.SetActiveButton(true, 1, 1);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "YellowArcher")
        {
            UIManager.instance.SetActiveButton(true, 2, 1);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "GreenArcher")
        {
            UIManager.instance.SetActiveButton(true, 3, 1);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "RedSpearman")
        {
            UIManager.instance.SetActiveButton(true, 0, 2);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "BlueSpearman")
        {
            UIManager.instance.SetActiveButton(true, 1, 2);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "YellowSpearman")
        {
            UIManager.instance.SetActiveButton(true, 2, 2);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "GreenSpearman")
        {
            UIManager.instance.SetActiveButton(true, 3, 2);
        }

    }

    public void Sommon()
    {
        SoldierChoose(0, 3, 0, 0);
        createdefenser.DrawSoldier(Colornumber, Soldiernumber);
        // createdefenser.CreateSoldier(Colornumber, Soldiernumber);
        // createdefenser.ExpenditureGold();
    }
}
