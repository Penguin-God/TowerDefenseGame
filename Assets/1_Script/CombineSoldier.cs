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
    public ButtonDown buttonDown;

    void Start()
    {
        TagSoldier = GetComponent<SoldiersTags>();
    }

    private void CombineSuccessTextDown()
    {
        UIManager.instance.CombineSuccessText.gameObject.SetActive(false);
    }

    private void CombineFailTextDown()
    {
        UIManager.instance.CombineFailText.gameObject.SetActive(false);
    }

    public void CombineRedArcher()
    {
        TagSoldier.RedSwordmanTag();
        if (TagSoldier.RedSwordman.Length >= 3)
        {

            Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.RedSwordman[1].transform.parent.gameObject);
            Destroy(TagSoldier.RedSwordman[2].transform.parent.gameObject);


            SoldierChoose(0, 0, 1, 1);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("빨간 궁수 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        //UIManager.instance.UpdateArcherCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.CreateButtonAuido.Play();
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineRedSpearman()
    {
        TagSoldier.RedSwordmanTag();
        TagSoldier.RedArcherTag();
        if (TagSoldier.RedSwordman.Length >= 2 && TagSoldier.RedArcher.Length >= 3)
        {

            Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.RedSwordman[1].transform.parent.gameObject);
            Destroy(TagSoldier.RedArcher[0].transform.parent.gameObject);
            Destroy(TagSoldier.RedArcher[1].transform.parent.gameObject);
            Destroy(TagSoldier.RedArcher[2].transform.parent.gameObject);


            SoldierChoose(0, 0, 2, 2);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("빨간 창병 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        //UIManager.instance.UpdateArcherCount();
        //UIManager.instance.UpdateSpearmanCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
    }

    public void CombineRedMage()
    {
        TagSoldier.RedSpearmanTag();
        TagSoldier.RedArcherTag();
        if (TagSoldier.RedSpearman.Length >= 3 && TagSoldier.RedArcher.Length >= 2)
        {
            Destroy(TagSoldier.RedArcher[0].transform.parent.gameObject);
            Destroy(TagSoldier.RedArcher[1].transform.parent.gameObject);
            Destroy(TagSoldier.RedSpearman[0].transform.parent.gameObject);
            Destroy(TagSoldier.RedSpearman[1].transform.parent.gameObject);
            Destroy(TagSoldier.RedSpearman[2].transform.parent.gameObject);


            SoldierChoose(0, 0, 3, 3);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("빨간 마법사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateArcherCount();
        //UIManager.instance.UpdateSpearmanCount();
        //UIManager.instance.UpdateMageCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
    }

    public void CombineBlueArcher()
    {
        TagSoldier.BlueSwordmanTag();
        if (TagSoldier.BlueSwordman.Length >= 3)
        {

            Destroy(TagSoldier.BlueSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.BlueSwordman[1].transform.parent.gameObject);
            Destroy(TagSoldier.BlueSwordman[2].transform.parent.gameObject);


            SoldierChoose(1, 1, 1, 1);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("파란 궁수 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        //UIManager.instance.UpdateArcherCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineBlueSpearman()
    {
        TagSoldier.BlueSwordmanTag();
        TagSoldier.BlueArcherTag();
        if (TagSoldier.BlueSwordman.Length >= 2 && TagSoldier.BlueArcher.Length >= 3)
        {

            Destroy(TagSoldier.BlueSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.BlueSwordman[1].transform.parent.gameObject);
            Destroy(TagSoldier.BlueArcher[0].transform.parent.gameObject);
            Destroy(TagSoldier.BlueArcher[1].transform.parent.gameObject);
            Destroy(TagSoldier.BlueArcher[2].transform.parent.gameObject);


            SoldierChoose(1, 1, 2, 2);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("파란 창병 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        //UIManager.instance.UpdateArcherCount();
        //UIManager.instance.UpdateSpearmanCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
    }

    public void CombineBlueMage()
    {
        TagSoldier.BlueSpearmanTag();
        TagSoldier.BlueArcherTag();
        if (TagSoldier.BlueSpearman.Length >= 3 && TagSoldier.BlueArcher.Length >= 2)
        {
            Destroy(TagSoldier.BlueArcher[0].transform.parent.gameObject);
            Destroy(TagSoldier.BlueArcher[1].transform.parent.gameObject);
            Destroy(TagSoldier.BlueSpearman[0].transform.parent.gameObject);
            Destroy(TagSoldier.BlueSpearman[1].transform.parent.gameObject);
            Destroy(TagSoldier.BlueSpearman[2].transform.parent.gameObject);


            SoldierChoose(1, 1, 3, 3);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("파란 마법사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateArcherCount();
        //UIManager.instance.UpdateSpearmanCount();
        //UIManager.instance.UpdateMageCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
    }

    public void CombineYellowArcher()
    {
        TagSoldier.YellowSwordmanTag();
        if (TagSoldier.YellowSwordman.Length >= 3)
        {
            Destroy(TagSoldier.YellowSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.YellowSwordman[1].transform.parent.gameObject);
            Destroy(TagSoldier.YellowSwordman[2].transform.parent.gameObject);
            GameManager.instance.Gold += 3;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


            SoldierChoose(2, 2, 1, 1);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("노란 궁수 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        //UIManager.instance.UpdateArcherCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineYellowSpearman()
    {
        TagSoldier.YellowSwordmanTag();
        TagSoldier.YellowArcherTag();
        if (TagSoldier.YellowSwordman.Length >= 2 && TagSoldier.YellowArcher.Length >= 3)
        {
            Destroy(TagSoldier.YellowSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.YellowSwordman[1].transform.parent.gameObject);
            Destroy(TagSoldier.YellowArcher[0].transform.parent.gameObject);
            Destroy(TagSoldier.YellowArcher[1].transform.parent.gameObject);
            Destroy(TagSoldier.YellowArcher[2].transform.parent.gameObject);
            GameManager.instance.Gold += 2;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            SoldierChoose(2, 2, 2, 2);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("노란 창병 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        //UIManager.instance.UpdateArcherCount();
        //UIManager.instance.UpdateSpearmanCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
    }

    public void CombineYellowMage()
    {
        TagSoldier.YellowSpearmanTag();
        TagSoldier.YellowArcherTag();
        if (TagSoldier.YellowSpearman.Length >= 3 && TagSoldier.YellowArcher.Length >= 2)
        {
            Destroy(TagSoldier.YellowArcher[0].transform.parent.gameObject);
            Destroy(TagSoldier.YellowArcher[1].transform.parent.gameObject);
            Destroy(TagSoldier.YellowSpearman[0].transform.parent.gameObject);
            Destroy(TagSoldier.YellowSpearman[1].transform.parent.gameObject);
            Destroy(TagSoldier.YellowSpearman[2].transform.parent.gameObject);


            SoldierChoose(2, 2, 3, 3);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("노란 마법사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateArcherCount();
        //UIManager.instance.UpdateSpearmanCount();
        //UIManager.instance.UpdateMageCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
    }

    public void CombineGreenArcher()
    {
        TagSoldier.GreenSwordmanTag();
        if (TagSoldier.GreenSwordman.Length >= 3)
        {

            Destroy(TagSoldier.GreenSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.GreenSwordman[1].transform.parent.gameObject);
            Destroy(TagSoldier.GreenSwordman[2].transform.parent.gameObject);


            SoldierChoose(3, 3, 1, 1);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("초록 궁수 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        //UIManager.instance.UpdateArcherCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineGreenSpearman()
    {
        TagSoldier.BlueSwordmanTag();
        TagSoldier.YellowSwordmanTag();
        TagSoldier.GreenArcherTag();
        if (TagSoldier.BlueSwordman.Length >= 1 && TagSoldier.YellowSwordman.Length >= 1 && TagSoldier.GreenArcher.Length >= 3)
        {
            Destroy(TagSoldier.BlueSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.YellowSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.GreenArcher[0].transform.parent.gameObject);
            Destroy(TagSoldier.GreenArcher[1].transform.parent.gameObject);
            Destroy(TagSoldier.GreenArcher[2].transform.parent.gameObject);
            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            SoldierChoose(3, 3, 2, 2);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("초록 창병 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        //UIManager.instance.UpdateArcherCount();
        //UIManager.instance.UpdateSpearmanCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
    }

    public void CombineGreenMage()
    {
        TagSoldier.GreenSpearmanTag();
        TagSoldier.BlueArcherTag();
        TagSoldier.YellowArcherTag();
        if (TagSoldier.GreenSpearman.Length >= 3 && TagSoldier.BlueArcher.Length >= 1 && TagSoldier.YellowArcher.Length >= 1)
        {

            Destroy(TagSoldier.GreenSpearman[0].transform.parent.gameObject);
            Destroy(TagSoldier.GreenSpearman[1].transform.parent.gameObject);
            Destroy(TagSoldier.GreenSpearman[2].transform.parent.gameObject);
            Destroy(TagSoldier.BlueArcher[0].transform.parent.gameObject);
            Destroy(TagSoldier.YellowArcher[0].transform.parent.gameObject);


            SoldierChoose(3, 3, 3, 3);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("초록 마법사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateArcherCount();
        //UIManager.instance.UpdateSpearmanCount();
        //UIManager.instance.UpdateMageCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);

    }

    public void CombineOrangeArcher()
    {
        TagSoldier.OrangeSwordmanTag();
        if (TagSoldier.OrangeSwordman.Length >= 3)
        {

            Destroy(TagSoldier.OrangeSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.OrangeSwordman[1].transform.parent.gameObject);
            Destroy(TagSoldier.OrangeSwordman[2].transform.parent.gameObject);


            SoldierChoose(4, 4, 1, 1);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("주황 궁수 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        //UIManager.instance.UpdateArcherCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineOrangeSpearman()
    {
        TagSoldier.RedSwordmanTag();
        TagSoldier.YellowSwordmanTag();
        TagSoldier.OrangeArcherTag();
        if (TagSoldier.RedSwordman.Length >= 1 && TagSoldier.YellowSwordman.Length >= 1 && TagSoldier.OrangeArcher.Length >= 3)
        {
            Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.YellowSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.OrangeArcher[0].transform.parent.gameObject);
            Destroy(TagSoldier.OrangeArcher[1].transform.parent.gameObject);
            Destroy(TagSoldier.OrangeArcher[2].transform.parent.gameObject);
            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            SoldierChoose(4, 4, 2, 2);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("주황 창병 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        //UIManager.instance.UpdateArcherCount();
        //UIManager.instance.UpdateSpearmanCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(4).GetChild(1).gameObject.SetActive(false);
    }

    public void CombineOrangeMage()
    {
        TagSoldier.OrangeSpearmanTag();
        TagSoldier.RedArcherTag();
        TagSoldier.YellowArcherTag();
        if (TagSoldier.OrangeSpearman.Length >= 3 && TagSoldier.RedArcher.Length >= 1 && TagSoldier.YellowArcher.Length >= 1)
        {

            Destroy(TagSoldier.OrangeSpearman[0].transform.parent.gameObject);
            Destroy(TagSoldier.OrangeSpearman[1].transform.parent.gameObject);
            Destroy(TagSoldier.OrangeSpearman[2].transform.parent.gameObject);
            Destroy(TagSoldier.RedArcher[0].transform.parent.gameObject);
            Destroy(TagSoldier.YellowArcher[0].transform.parent.gameObject);


            SoldierChoose(4, 4, 3, 3);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("주황 마법사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateArcherCount();
        //UIManager.instance.UpdateSpearmanCount();
        //UIManager.instance.UpdateMageCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(4).GetChild(2).gameObject.SetActive(false);
    }

    public void CombineVioletArcher()
    {
        TagSoldier.VioletSwordmanTag();
        if (TagSoldier.VioletSwordman.Length >= 3)
        {

            Destroy(TagSoldier.VioletSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.VioletSwordman[1].transform.parent.gameObject);
            Destroy(TagSoldier.VioletSwordman[2].transform.parent.gameObject);


            SoldierChoose(5, 5, 1, 1);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("보라 궁수 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        //UIManager.instance.UpdateArcherCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineVioletSpearman()
    {
        TagSoldier.BlueSwordmanTag();
        TagSoldier.RedSwordmanTag();
        TagSoldier.VioletArcherTag();
        if (TagSoldier.BlueSwordman.Length >= 1 && TagSoldier.RedSwordman.Length >= 1 && TagSoldier.VioletArcher.Length >= 3)
        {
            Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.VioletArcher[0].transform.parent.gameObject);
            Destroy(TagSoldier.VioletArcher[1].transform.parent.gameObject);
            Destroy(TagSoldier.VioletArcher[2].transform.parent.gameObject);


            SoldierChoose(5, 5, 2, 2);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("보라 창병 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        //UIManager.instance.UpdateArcherCount();
        //UIManager.instance.UpdateSpearmanCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(5).GetChild(1).gameObject.SetActive(false);
    }

    public void CombineVioletMage()
    {
        TagSoldier.VioletSpearmanTag();
        TagSoldier.BlueArcherTag();
        TagSoldier.RedArcherTag();
        if (TagSoldier.VioletSpearman.Length >= 3 && TagSoldier.BlueArcher.Length >= 1 && TagSoldier.RedArcher.Length >= 1)
        {

            Destroy(TagSoldier.VioletSpearman[0].transform.parent.gameObject);
            Destroy(TagSoldier.VioletSpearman[1].transform.parent.gameObject);
            Destroy(TagSoldier.VioletSpearman[2].transform.parent.gameObject);
            Destroy(TagSoldier.RedArcher[0].transform.parent.gameObject);
            Destroy(TagSoldier.BlueArcher[0].transform.parent.gameObject);


            SoldierChoose(5, 5, 3, 3);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("보라 마법사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateArcherCount();
        //UIManager.instance.UpdateSpearmanCount();
        //UIManager.instance.UpdateMageCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(5).GetChild(2).gameObject.SetActive(false);
    }



    public void CombineGreenSwordman()
    {
        TagSoldier.YellowSwordmanTag();
        TagSoldier.BlueSwordmanTag();
        if (TagSoldier.YellowSwordman.Length >= 1 && TagSoldier.BlueSwordman.Length >= 1)
        {
            Destroy(TagSoldier.YellowSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.BlueSwordman[0].transform.parent.gameObject);
            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            SoldierChoose(3, 3, 0, 0);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("초록 기사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineOrangeSwordman()
    {
        TagSoldier.YellowSwordmanTag();
        TagSoldier.RedSwordmanTag();
        if (TagSoldier.RedSwordman.Length >= 1 && TagSoldier.YellowSwordman.Length >= 1)
        { 
            Destroy(TagSoldier.YellowSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);

            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            SoldierChoose(4,4,0,0);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("주황 기사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);
        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
    }

    public void CombineVioletSwordman()
    {
        TagSoldier.BlueSwordmanTag();
        TagSoldier.RedSwordmanTag();
        if (TagSoldier.RedSwordman.Length >= 1 && TagSoldier.BlueSwordman.Length >= 1)
        {
            Destroy(TagSoldier.BlueSwordman[0].transform.parent.gameObject);
            Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);


            SoldierChoose(5, 5, 0, 0);
            createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("보라 기사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        //UIManager.instance.UpdateSwordmanCount();
        buttonDown.AllButtonDown();
        //UIManager.instance.ButtonDown();
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(false);
        //UIManager.instance.ExPlanationTexts.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }

    //public void ComeBackButton()
    //{
    //    UIManager.instance.ButtonDown();
    //    unitmanage.UnitManagementButton.gameObject.SetActive(true);
    //    UIManager.instance.BackGround.gameObject.SetActive(false);
    //    UIManager.instance.ExPlanationTexts.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
    //    UIManager.instance.ExPlanationTexts.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
    //    UIManager.instance.ExPlanationTexts.transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
    //    UIManager.instance.ExPlanationTexts.transform.GetChild(3).GetChild(3).gameObject.SetActive(false);
    //    UIManager.instance.ExPlanationTexts.transform.GetChild(4).GetChild(3).gameObject.SetActive(false);
    //    UIManager.instance.ExPlanationTexts.transform.GetChild(5).GetChild(3).gameObject.SetActive(false);
    //}




    public void SoldierChoose(int Colornumber1, int Colornumber2, int Soldiernumber1, int Soldiernumber2)
    {
        Colornumber = Random.Range(Colornumber1, Colornumber2);
        Soldiernumber = Random.Range(Soldiernumber1, Soldiernumber2);

    }


    public void Sommon()
    {
        if (UnitManager.instance.UnitOver)
        {

            return;
        }
        SoldierChoose(0, 3, 0, 0);
        createdefenser.DrawSoldier(Colornumber, Soldiernumber);
        //UIManager.instance.UpdateSwordmanCount();
        UIManager.instance.CreateButtonAuido.Play();
        // createdefenser.CreateSoldier(Colornumber, Soldiernumber);
        // createdefenser.ExpenditureGold();
    }


}
