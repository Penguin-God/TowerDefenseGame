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
    //public CombineSoldierPooling combineSoldierPooling;

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

            //Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);
            //Destroy(TagSoldier.RedSwordman[1].transform.parent.gameObject);
            //Destroy(TagSoldier.RedSwordman[2].transform.parent.gameObject);

            
            //SoldierChoose(0, 0, 1, 1);
            //createdefenser.CreateSoldier(Colornumber, Soldiernumber);

            CombineSoldierPooling.ReturnObject(TagSoldier.RedSwordman[0].transform.parent.gameObject, "RedSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.RedSwordman[1].transform.parent.gameObject, "RedSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.RedSwordman[2].transform.parent.gameObject, "RedSwordman");

            CombineSoldierPooling.GetObject("RedArcher", 0, 1);

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

            CombineSoldierPooling.ReturnObject(TagSoldier.RedSwordman[0].transform.parent.gameObject,"RedSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.RedSwordman[1].transform.parent.gameObject, "RedSwordman");

            CombineSoldierPooling.ReturnObject(TagSoldier.RedArcher[0].transform.parent.gameObject, "RedArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.RedArcher[1].transform.parent.gameObject, "RedArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.RedArcher[2].transform.parent.gameObject, "RedArcher");

            CombineSoldierPooling.GetObject("RedSpearman", 0, 2);

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
            CombineSoldierPooling.ReturnObject(TagSoldier.RedArcher[0].transform.parent.gameObject, "RedArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.RedArcher[1].transform.parent.gameObject, "RedArcher");

            CombineSoldierPooling.ReturnObject(TagSoldier.RedSpearman[0].transform.parent.gameObject, "RedSpearman");
            CombineSoldierPooling.ReturnObject(TagSoldier.RedSpearman[1].transform.parent.gameObject, "RedSpearman");
            CombineSoldierPooling.ReturnObject(TagSoldier.RedSpearman[2].transform.parent.gameObject, "RedSpearman");

            CombineSoldierPooling.GetObject("RedMage", 0, 3);
      
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

            CombineSoldierPooling.ReturnObject(TagSoldier.BlueSwordman[0].transform.parent.gameObject, "BlueSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.BlueSwordman[1].transform.parent.gameObject, "BlueSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.BlueSwordman[2].transform.parent.gameObject, "BlueSwordman");

            CombineSoldierPooling.GetObject("BlueArcher", 1, 1);

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
            CombineSoldierPooling.ReturnObject(TagSoldier.BlueSwordman[0].transform.parent.gameObject, "BlueSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.BlueSwordman[1].transform.parent.gameObject, "BlueSwordman");

            CombineSoldierPooling.ReturnObject(TagSoldier.BlueArcher[0].transform.parent.gameObject, "BlueArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.BlueArcher[1].transform.parent.gameObject, "BlueArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.BlueArcher[2].transform.parent.gameObject, "BlueArcher");

            CombineSoldierPooling.GetObject("BlueSpearman", 1, 2);

            
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
            CombineSoldierPooling.ReturnObject(TagSoldier.BlueArcher[0].transform.parent.gameObject, "BlueArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.BlueArcher[1].transform.parent.gameObject, "BlueArcher");

            CombineSoldierPooling.ReturnObject(TagSoldier.BlueSpearman[0].transform.parent.gameObject, "BlueSpearman");
            CombineSoldierPooling.ReturnObject(TagSoldier.BlueSpearman[1].transform.parent.gameObject, "BlueSpearman");
            CombineSoldierPooling.ReturnObject(TagSoldier.BlueSpearman[2].transform.parent.gameObject, "BlueSpearman");

            CombineSoldierPooling.GetObject("BlueMage", 1, 3);

            
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
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowSwordman[0].transform.parent.gameObject, "YellowSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowSwordman[1].transform.parent.gameObject, "YellowSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowSwordman[2].transform.parent.gameObject, "YellowSwordman");

            CombineSoldierPooling.GetObject("YellowArcher", 2, 1);


            GameManager.instance.Gold += 3;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


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
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowSwordman[0].transform.parent.gameObject, "YellowSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowSwordman[1].transform.parent.gameObject, "YellowSwordman");

            CombineSoldierPooling.ReturnObject(TagSoldier.YellowArcher[0].transform.parent.gameObject, "YellowArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowArcher[1].transform.parent.gameObject, "YellowArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowArcher[2].transform.parent.gameObject, "YellowArcher");

            CombineSoldierPooling.GetObject("YellowSpearman", 2, 2);

            GameManager.instance.Gold += 2;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

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
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowArcher[0].transform.parent.gameObject, "YellowArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowArcher[1].transform.parent.gameObject, "YellowArcher");

            CombineSoldierPooling.ReturnObject(TagSoldier.YellowSpearman[0].transform.parent.gameObject, "YellowSpearman");
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowSpearman[1].transform.parent.gameObject, "YellowSpearman");
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowSpearman[2].transform.parent.gameObject, "YellowSpearman");

            CombineSoldierPooling.GetObject("YellowMage", 2, 3);

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
            CombineSoldierPooling.ReturnObject(TagSoldier.GreenSwordman[0].transform.parent.gameObject, "GreenSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.GreenSwordman[1].transform.parent.gameObject, "GreenSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.GreenSwordman[2].transform.parent.gameObject, "GreenSwordman");

            CombineSoldierPooling.GetObject("GreenArcher", 3, 1);

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
            CombineSoldierPooling.ReturnObject(TagSoldier.BlueSwordman[0].transform.parent.gameObject, "BlueSwordman");

            CombineSoldierPooling.ReturnObject(TagSoldier.YellowSwordman[0].transform.parent.gameObject, "YellowSwordman");

            CombineSoldierPooling.ReturnObject(TagSoldier.GreenArcher[0].transform.parent.gameObject, "GreenArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.GreenArcher[1].transform.parent.gameObject, "GreenArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.GreenArcher[2].transform.parent.gameObject, "GreenArcher");

            CombineSoldierPooling.GetObject("GreenSpearman", 3, 2);

            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

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
            CombineSoldierPooling.ReturnObject(TagSoldier.GreenSpearman[0].transform.parent.gameObject, "GreenSpearman");
            CombineSoldierPooling.ReturnObject(TagSoldier.GreenSpearman[1].transform.parent.gameObject, "GreenSpearman");
            CombineSoldierPooling.ReturnObject(TagSoldier.GreenSpearman[2].transform.parent.gameObject, "GreenSpearman");

            CombineSoldierPooling.ReturnObject(TagSoldier.BlueArcher[0].transform.parent.gameObject, "BlueArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowArcher[0].transform.parent.gameObject, "YellowArcher");

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
            CombineSoldierPooling.ReturnObject(TagSoldier.OrangeSwordman[0].transform.parent.gameObject, "OrangeSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.OrangeSwordman[1].transform.parent.gameObject, "OrangeSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.OrangeSwordman[2].transform.parent.gameObject, "OrangeSwordman");

            CombineSoldierPooling.GetObject("OrangeArcher", 4, 1);
           
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
            CombineSoldierPooling.ReturnObject(TagSoldier.RedSwordman[0].transform.parent.gameObject, "RedSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowSwordman[0].transform.parent.gameObject, "YellowSwordman");

            CombineSoldierPooling.ReturnObject(TagSoldier.OrangeArcher[0].transform.parent.gameObject, "OrangeArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.OrangeArcher[1].transform.parent.gameObject, "OrangeArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.OrangeArcher[2].transform.parent.gameObject, "OrangeArcher");

            CombineSoldierPooling.GetObject("OrangeSpearman", 4, 2);

            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

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
            CombineSoldierPooling.ReturnObject(TagSoldier.OrangeSpearman[0].transform.parent.gameObject, "OrangeSpearman");
            CombineSoldierPooling.ReturnObject(TagSoldier.OrangeSpearman[1].transform.parent.gameObject, "OrangeSpearman");
            CombineSoldierPooling.ReturnObject(TagSoldier.OrangeSpearman[2].transform.parent.gameObject, "OrangeSpearman");

            CombineSoldierPooling.ReturnObject(TagSoldier.RedArcher[0].transform.parent.gameObject, "RedArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowArcher[1].transform.parent.gameObject, "YellowArcher");


            CombineSoldierPooling.GetObject("OrangeMage", 4, 3);

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
            CombineSoldierPooling.ReturnObject(TagSoldier.VioletSwordman[0].transform.parent.gameObject, "VioletSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.VioletSwordman[1].transform.parent.gameObject, "VioletSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.VioletSwordman[2].transform.parent.gameObject, "VioletSwordman");

            CombineSoldierPooling.GetObject("VioletArcher", 5, 1);

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
            CombineSoldierPooling.ReturnObject(TagSoldier.RedSwordman[0].transform.parent.gameObject, "RedSwordman");

            CombineSoldierPooling.ReturnObject(TagSoldier.BlueSwordman[0].transform.parent.gameObject, "BlueSwordman");

            CombineSoldierPooling.ReturnObject(TagSoldier.VioletArcher[0].transform.parent.gameObject, "VioletArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.VioletArcher[1].transform.parent.gameObject, "VioletArcher");
            CombineSoldierPooling.ReturnObject(TagSoldier.VioletArcher[2].transform.parent.gameObject, "VioletArcher");

            CombineSoldierPooling.GetObject("VioletSpearman", 5, 2);

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
            CombineSoldierPooling.ReturnObject(TagSoldier.VioletSpearman[0].transform.parent.gameObject, "VioletSpearman");
            CombineSoldierPooling.ReturnObject(TagSoldier.VioletSpearman[1].transform.parent.gameObject, "VioletSpearman");
            CombineSoldierPooling.ReturnObject(TagSoldier.VioletSpearman[2].transform.parent.gameObject, "VioletSpearman");

            CombineSoldierPooling.ReturnObject(TagSoldier.RedArcher[0].transform.parent.gameObject, "RedArcher");

            CombineSoldierPooling.ReturnObject(TagSoldier.BlueArcher[1].transform.parent.gameObject, "BlueArcher");

            CombineSoldierPooling.GetObject("VioletMage", 5, 3);

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
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowSwordman[0].transform.parent.gameObject, "YellowSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.BlueSwordman[0].transform.parent.gameObject, "BlueSwordman");

            CombineSoldierPooling.GetObject("GreenSwordman", 3, 0);

            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


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
            CombineSoldierPooling.ReturnObject(TagSoldier.YellowSwordman[0].transform.parent.gameObject, "YellowSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.RedSwordman[0].transform.parent.gameObject, "RedSwordman");

            CombineSoldierPooling.GetObject("OrangeSwordman", 4, 0);

            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


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
            CombineSoldierPooling.ReturnObject(TagSoldier.BlueSwordman[0].transform.parent.gameObject, "BlueSwordman");
            CombineSoldierPooling.ReturnObject(TagSoldier.RedSwordman[0].transform.parent.gameObject, "RedSwordman");

            CombineSoldierPooling.GetObject("VioletSwordman", 5, 0);

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
        int randomnumber = Random.Range(0, 3);

        if (UnitManager.instance.UnitOver)
        {
            return;
        }
        //SoldierChoose(0, 3, 0, 0);

        if (randomnumber == 0)
        {
            CombineSoldierPooling.GetObject("RedSwordman", 0, 0);
        }
        else if (randomnumber == 1)
        {
            CombineSoldierPooling.GetObject("BlueSwordman", 1, 0);
        }
        else if (randomnumber == 2)
        {
            CombineSoldierPooling.GetObject("YellowSwordman", 2, 0);
        }
        else
        {
            CombineSoldierPooling.GetObject("RedSwordman", 0, 0);
        }
        //createdefenser.DrawSoldier(Colornumber, Soldiernumber);
        //UIManager.instance.CreateButtonAuido.Play();


        ////UIManager.instance.UpdateSwordmanCount();
        //// createdefenser.CreateSoldier(Colornumber, Soldiernumber);
        //// createdefenser.ExpenditureGold();
    }


}
