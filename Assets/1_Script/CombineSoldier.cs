using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSoldier : MonoBehaviour
{
    public int Colornumber;
    public int Soldiernumber;
    public CreateDefenser createdefenser;
    public SoldiersTags TagSoldier;
    public UnitManageButton unitmanage;

    void Start()
    {
        TagSoldier = GetComponent<SoldiersTags>();
    }

    public void CombineRedArcher()
    {
        TagSoldier.RedSwordmanTag();
        if (TagSoldier.RedSwordman.Length >= 2)
        {

            Destroy(TagSoldier.RedSwordman[0]);
            Destroy(TagSoldier.RedSwordman[1]);


            SoldierChoose(0, 0, 1, 1);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);
        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.UpdateArcherCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineRedSpearman()
    {
        TagSoldier.RedSwordmanTag();
        TagSoldier.RedArcherTag();
        if (TagSoldier.RedSwordman.Length >= 2 && TagSoldier.RedArcher.Length >= 2)
        {

            Destroy(TagSoldier.RedSwordman[0]);
            Destroy(TagSoldier.RedSwordman[1]);
            Destroy(TagSoldier.RedArcher[0]);
            Destroy(TagSoldier.RedArcher[1]);


            SoldierChoose(0, 0, 2, 2);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.UpdateArcherCount();
        UIManager.instance.UpdateSpearmanCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
    }

    public void CombineRedMage()
    {
        TagSoldier.RedSpearmanTag();
        if (TagSoldier.RedSpearman.Length >= 2)
        {

            Destroy(TagSoldier.RedSpearman[0]);
            Destroy(TagSoldier.RedSpearman[1]);


            SoldierChoose(0, 0, 3, 3);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSpearmanCount();
        UIManager.instance.UpdateMageCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
    }

    public void CombineBlueArcher()
    {
        TagSoldier.BlueSwordmanTag();
        if (TagSoldier.BlueSwordman.Length >= 2)
        {

            Destroy(TagSoldier.BlueSwordman[0]);
            Destroy(TagSoldier.BlueSwordman[1]);


            SoldierChoose(1, 1, 1, 1);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.UpdateArcherCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineBlueSpearman()
    {
        TagSoldier.BlueSwordmanTag();
        TagSoldier.BlueArcherTag();
        if (TagSoldier.BlueSwordman.Length >= 2 && TagSoldier.BlueArcher.Length >= 2)
        {

            Destroy(TagSoldier.BlueSwordman[0]);
            Destroy(TagSoldier.BlueSwordman[1]);
            Destroy(TagSoldier.BlueArcher[0]);
            Destroy(TagSoldier.BlueArcher[1]);


            SoldierChoose(1, 1, 2, 2);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.UpdateArcherCount();
        UIManager.instance.UpdateSpearmanCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
    }

    public void CombineBlueMage()
    {
        TagSoldier.BlueSpearmanTag();
        if (TagSoldier.BlueSpearman.Length >= 2)
        {

            Destroy(TagSoldier.BlueSpearman[0]);
            Destroy(TagSoldier.BlueSpearman[1]);


            SoldierChoose(1, 1, 3, 3);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSpearmanCount();
        UIManager.instance.UpdateMageCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
    }

    public void CombineYellowArcher()
    {
        TagSoldier.YellowSwordmanTag();
        if (TagSoldier.YellowSwordman.Length >= 2)
        {

            Destroy(TagSoldier.YellowSwordman[0]);
            Destroy(TagSoldier.YellowSwordman[1]);
            GameManager.instance.Gold += 2;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


            SoldierChoose(2, 2, 1, 1);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.UpdateArcherCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineYellowSpearman()
    {
        TagSoldier.YellowSwordmanTag();
        TagSoldier.YellowArcherTag();
        if (TagSoldier.YellowSwordman.Length >= 2 && TagSoldier.YellowArcher.Length >= 2)
        {

            Destroy(TagSoldier.YellowSwordman[0]);
            Destroy(TagSoldier.YellowSwordman[1]);
            Destroy(TagSoldier.YellowArcher[0]);
            Destroy(TagSoldier.YellowArcher[1]);
            GameManager.instance.Gold += 2;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


            SoldierChoose(2, 2, 2, 2);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);
            UIManager.instance.UpdateSwordmanCount();
            UIManager.instance.UpdateArcherCount();
            UIManager.instance.UpdateSpearmanCount();
        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.UpdateArcherCount();
        UIManager.instance.UpdateSpearmanCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
    }

    public void CombineYellowMage()
    {
        TagSoldier.YellowSpearmanTag();
        if (TagSoldier.YellowSpearman.Length >= 2)
        {

            Destroy(TagSoldier.YellowSpearman[0]);
            Destroy(TagSoldier.YellowSpearman[1]);


            SoldierChoose(2, 2, 3, 3);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);
            UIManager.instance.UpdateSpearmanCount();
            UIManager.instance.UpdateMageCount();
        }
        UIManager.instance.UpdateSpearmanCount();
        UIManager.instance.UpdateMageCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
    }

    public void CombineGreenArcher()
    {
        TagSoldier.GreenSwordmanTag();
        if (TagSoldier.GreenSwordman.Length >= 2)
        {

            Destroy(TagSoldier.GreenSwordman[0]);
            Destroy(TagSoldier.GreenSwordman[1]);


            SoldierChoose(3, 3, 1, 1);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);
            UIManager.instance.UpdateSwordmanCount();
            UIManager.instance.UpdateArcherCount();
        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.UpdateArcherCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineGreenSpearman()
    {
        TagSoldier.BlueSwordmanTag();
        TagSoldier.YellowSwordmanTag();
        TagSoldier.GreenArcherTag();
        if (TagSoldier.BlueSwordman.Length >= 1 && TagSoldier.YellowSwordman.Length >= 1 && TagSoldier.GreenArcher.Length >= 2)
        {

            Destroy(TagSoldier.BlueSwordman[0]);
            Destroy(TagSoldier.YellowSwordman[0]);
            Destroy(TagSoldier.GreenArcher[0]);
            Destroy(TagSoldier.GreenArcher[1]);
            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


            SoldierChoose(3, 3, 2, 2);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.UpdateArcherCount();
        UIManager.instance.UpdateSpearmanCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
    }

    public void CombineGreenMage()
    {
        TagSoldier.GreenSpearmanTag();
        TagSoldier.BlueArcherTag();
        TagSoldier.YellowArcherTag();
        if (TagSoldier.GreenSpearman.Length >= 2 && TagSoldier.BlueArcher.Length >= 1 && TagSoldier.YellowArcher.Length >= 1)
        {

            Destroy(TagSoldier.GreenSpearman[0]);
            Destroy(TagSoldier.GreenSpearman[1]);
            Destroy(TagSoldier.BlueArcher[0]);
            Destroy(TagSoldier.YellowArcher[0]);


            SoldierChoose(3, 3, 3, 3);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSpearmanCount();
        UIManager.instance.UpdateMageCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);

    }

    public void CombineOrangeArcher()
    {
        TagSoldier.OrangeSwordmanTag();
        if (TagSoldier.OrangeSwordman.Length >= 2)
        {

            Destroy(TagSoldier.OrangeSwordman[0]);
            Destroy(TagSoldier.OrangeSwordman[1]);


            SoldierChoose(4, 4, 1, 1);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.UpdateArcherCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineOrangeSpearman()
    {
        TagSoldier.RedSwordmanTag();
        TagSoldier.YellowSwordmanTag();
        TagSoldier.OrangeArcherTag();
        if (TagSoldier.RedSwordman.Length >= 1 && TagSoldier.YellowSwordman.Length >= 1 && TagSoldier.OrangeArcher.Length >= 2)
        {

            Destroy(TagSoldier.RedSwordman[0]);
            Destroy(TagSoldier.YellowSwordman[0]);
            Destroy(TagSoldier.OrangeArcher[0]);
            Destroy(TagSoldier.OrangeArcher[1]);
            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


            SoldierChoose(4, 4, 2, 2);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.UpdateArcherCount();
        UIManager.instance.UpdateSpearmanCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(4).GetChild(1).gameObject.SetActive(false);
    }

    public void CombineOrangeMage()
    {
        TagSoldier.OrangeSpearmanTag();
        TagSoldier.RedArcherTag();
        TagSoldier.YellowArcherTag();
        if (TagSoldier.OrangeSpearman.Length >= 2 && TagSoldier.RedArcher.Length >= 1 && TagSoldier.YellowArcher.Length >= 1)
        {

            Destroy(TagSoldier.OrangeSpearman[0]);
            Destroy(TagSoldier.OrangeSpearman[1]);
            Destroy(TagSoldier.RedArcher[0]);
            Destroy(TagSoldier.YellowArcher[0]);


            SoldierChoose(4, 4, 3, 3);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSpearmanCount();
        UIManager.instance.UpdateMageCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(4).GetChild(2).gameObject.SetActive(false);
    }

    public void CombineVioletArcher()
    {
        TagSoldier.VioletSwordmanTag();
        if (TagSoldier.VioletSwordman.Length >= 2)
        {

            Destroy(TagSoldier.VioletSwordman[0]);
            Destroy(TagSoldier.VioletSwordman[1]);


            SoldierChoose(5, 5, 1, 1);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.UpdateArcherCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineVioletSpearman()
    {
        TagSoldier.BlueSwordmanTag();
        TagSoldier.RedSwordmanTag();
        TagSoldier.VioletArcherTag();
        if (TagSoldier.BlueSwordman.Length >= 1 && TagSoldier.RedSwordman.Length >= 1 && TagSoldier.VioletArcher.Length >= 2)
        {

            Destroy(TagSoldier.RedSwordman[0]);
            Destroy(TagSoldier.YellowSwordman[0]);
            Destroy(TagSoldier.VioletArcher[0]);
            Destroy(TagSoldier.VioletArcher[1]);
            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


            SoldierChoose(5, 5, 2, 2);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.UpdateArcherCount();
        UIManager.instance.UpdateSpearmanCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(5).GetChild(1).gameObject.SetActive(false);
    }

    public void CombineVioletMage()
    {
        TagSoldier.VioletSpearmanTag();
        TagSoldier.BlueArcherTag();
        TagSoldier.RedArcherTag();
        if (TagSoldier.VioletSpearman.Length >= 2 && TagSoldier.BlueArcher.Length >= 1 && TagSoldier.RedArcher.Length >= 1)
        {

            Destroy(TagSoldier.VioletSpearman[0]);
            Destroy(TagSoldier.VioletSpearman[1]);
            Destroy(TagSoldier.RedArcher[0]);
            Destroy(TagSoldier.BlueArcher[0]);


            SoldierChoose(5, 5, 3, 3);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSpearmanCount();
        UIManager.instance.UpdateMageCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(5).GetChild(2).gameObject.SetActive(false);
    }



    public void CombineGreenSwordman()
    {
        TagSoldier.YellowSwordmanTag();
        TagSoldier.BlueSwordmanTag();
        if (TagSoldier.YellowSwordman.Length >= 1 && TagSoldier.BlueSwordman.Length >= 1)
        {

            Destroy(TagSoldier.YellowSwordman[0]);
            Destroy(TagSoldier.BlueSwordman[0]);
            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


            SoldierChoose(3, 3, 0, 0);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineOrangeSwordman()
    {
        TagSoldier.YellowSwordmanTag();
        TagSoldier.RedSwordmanTag();
        if (TagSoldier.RedSwordman.Length >= 1 && TagSoldier.YellowSwordman.Length >= 1)
        {

            Destroy(TagSoldier.YellowSwordman[0]);
            Destroy(TagSoldier.RedSwordman[0]);
            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


            SoldierChoose(4, 4, 0, 0);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);


        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineVioletSwordman()
    {
        TagSoldier.BlueSwordmanTag();
        TagSoldier.RedSwordmanTag();
        if (TagSoldier.RedSwordman.Length >= 1 && TagSoldier.BlueSwordman.Length >= 1)
        {

            Destroy(TagSoldier.BlueSwordman[0]);
            Destroy(TagSoldier.RedSwordman[0]);


            SoldierChoose(5, 5, 0, 0);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

        }
        UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }

    public void ComeBackButton()
    {
        UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(3).GetChild(3).gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(3).GetChild(4).gameObject.SetActive(false);
        UIManager.instance.ExPlanationTexts.transform.GetChild(3).GetChild(5).gameObject.SetActive(false);
    }




    public void SoldierChoose(int Colornumber1, int Colornumber2, int Soldiernumber1, int Soldiernumber2)
    {
        Colornumber = Random.Range(Colornumber1, Colornumber2);
        Soldiernumber = Random.Range(Soldiernumber1, Soldiernumber2);

    }

    public void ButtonOn()
    {

        //GameManager.instance.Chilk();
        UIManager.instance.ButtonDown();
        if (GameManager.instance.hitSolider.transform.gameObject.tag == "RedSwordman")
        {
            UIManager.instance.SetActiveButton(true, 0, 0);
            UIManager.instance.SetActiveButton2(true, 0, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "BlueSwordman")
        {
            UIManager.instance.SetActiveButton(true, 1, 0);
            UIManager.instance.SetActiveButton2(true, 1, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "YellowSwordman")
        {
            UIManager.instance.SetActiveButton(true, 2, 0);
            UIManager.instance.SetActiveButton2(true, 2, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "GreenSwordman")
        {
            UIManager.instance.SetActiveButton(true, 3, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "OrangeSwordman")
        {
            UIManager.instance.SetActiveButton(true, 4, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "VioletSwordman")
        {
            UIManager.instance.SetActiveButton(true, 5, 0);
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

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "OrangeArcher")
        {
            UIManager.instance.SetActiveButton(true, 4, 1);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "VioletArcher")
        {
            UIManager.instance.SetActiveButton(true, 5, 1);
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

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "OrangeSpearman")
        {
            UIManager.instance.SetActiveButton(true, 4, 2);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "VioletSpearman")
        {
            UIManager.instance.SetActiveButton(true, 5, 2);
        }

    }

    public void Sommon()
    {
        SoldierChoose(0, 3, 0, 0);
        createdefenser.DrawSoldier(Colornumber, Soldiernumber);
        UIManager.instance.UpdateSwordmanCount();
        // createdefenser.CreateSoldier(Colornumber, Soldiernumber);
        // createdefenser.ExpenditureGold();
    }


}
