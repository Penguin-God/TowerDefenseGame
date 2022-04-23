using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDown : MonoBehaviour
{
    public GameObject BackGround;
    public AudioSource XButtonAudio;

    public GameObject BlackTowerUi;
    public GameObject WhiteTowerUi;

    public GameObject UnitManageBUtton;

    public GameObject SwordmanManageButton;
    public GameObject ArcherManageButton;
    public GameObject SpearmanManageButton;
    public GameObject MageManageButton;

    public GameObject ManageColorswordmanButton;
    public GameObject ManageColorArcherButton;
    public GameObject ManageColorSpearmanButton;
    public GameObject ManageColorMageButton;

    public GameObject RedSwordmanExplanText;
    public GameObject RedArcherExplanText;
    public GameObject RedSpearmanExplanText;
    public GameObject RedMageExplanText;

    public GameObject BlueSwordmanExplanText;
    public GameObject BlueArcherExplanText;
    public GameObject BlueSpearmanExplanText;
    public GameObject BlueMageExplanText;

    public GameObject YellowSwordmanExplanText;
    public GameObject YellowArcherExplanText;
    public GameObject YellowSpearmanExplanText;
    public GameObject YellowMageExplanText;

    public GameObject GreenSwordmanExplanText;
    public GameObject GreenArcherExplanText;
    public GameObject GreenSpearmanExplanText;
    public GameObject GreenMageExplanText;

    public GameObject OrangeSwordmanExplanText;
    public GameObject OrangeArcherExplanText;
    public GameObject OrangeSpearmanExplanText;
    public GameObject OrangeMageExplanText;

    public GameObject VioletSwordmanExplanText;
    public GameObject VioletArcherExplanText;
    public GameObject VioletSpearmanExplanText;
    public GameObject VioletMageExplanText;

    public GameObject RedSwordmanButton;
    public GameObject RedArcherButton;
    public GameObject RedSpearmanButton;

    public GameObject BlueSwordmanButton;
    public GameObject BlueArcherButton;
    public GameObject BlueSpearmanButton;

    public GameObject YellowSwordmanButton;
    public GameObject YellowArcherButton;
    public GameObject YellowSpearmanButton;

    public GameObject GreenSwordmanButton;
    public GameObject GreenArcherButton;
    public GameObject GreenSpearmanButton;

    public GameObject OrangeSwordmanButton;
    public GameObject OrangeArcherButton;
    public GameObject OrangeSpearmanButton;

    public GameObject VioletSwordmanButton;
    public GameObject VioletArcherButton;
    public GameObject VioletSpearmanButton;

    public GameObject RedPlusViolet;
    public GameObject BluePlusGreen;
    public GameObject YellowPlusOrange;

    public GameObject BlackSoldiersCombineButtons;

    public GameObject storyModeEeterButton;

    public GameObject SettingMenu;


    public void AllButtonDown()
    {
        XButtonAudio.Play();
        BackGround.SetActive(false);
        BlackSoldiersCombineButtons.SetActive(false);
        

        BlackTowerUi.SetActive(false);
        WhiteTowerUi.SetActive(false);
        SettingMenu.gameObject.SetActive(false);

        UnitManageBUtton.SetActive(true);

        SwordmanManageButton.SetActive(false);
        ArcherManageButton.SetActive(false);
        SpearmanManageButton.SetActive(false);
        MageManageButton.SetActive(false);

        ManageColorswordmanButton.SetActive(false);
        ManageColorArcherButton.SetActive(false);
        ManageColorSpearmanButton.SetActive(false);
        ManageColorMageButton.SetActive(false);

        RedSwordmanExplanText.SetActive(false);
        RedArcherExplanText.SetActive(false);
        RedSpearmanExplanText.SetActive(false);
        RedMageExplanText.SetActive(false);

        BlueSwordmanExplanText.SetActive(false);
        BlueArcherExplanText.SetActive(false);
        BlueSpearmanExplanText.SetActive(false);
        BlueMageExplanText.SetActive(false);

        YellowSwordmanExplanText.SetActive(false);
        YellowArcherExplanText.SetActive(false);
        YellowSpearmanExplanText.SetActive(false);
        YellowMageExplanText.SetActive(false);

        GreenSwordmanExplanText.SetActive(false);
        GreenArcherExplanText.SetActive(false);
        GreenSpearmanExplanText.SetActive(false);
        GreenMageExplanText.SetActive(false);

        OrangeSwordmanExplanText.SetActive(false);
        OrangeArcherExplanText.SetActive(false);
        OrangeSpearmanExplanText.SetActive(false);
        OrangeMageExplanText.SetActive(false);

        VioletSwordmanExplanText.SetActive(false);
        VioletArcherExplanText.SetActive(false);
        VioletSpearmanExplanText.SetActive(false);
        VioletMageExplanText.SetActive(false);

        RedSwordmanButton.SetActive(false);
        RedArcherButton.SetActive(false);
        RedSpearmanButton.SetActive(false);


        BlueSwordmanButton.SetActive(false);
        BlueArcherButton.SetActive(false);
        BlueSpearmanButton.SetActive(false);


        YellowSwordmanButton.SetActive(false);
        YellowArcherButton.SetActive(false);
        YellowSpearmanButton.SetActive(false);


        GreenSwordmanButton.SetActive(false);
        GreenArcherButton.SetActive(false);
        GreenSpearmanButton.SetActive(false);


        OrangeSwordmanButton.SetActive(false);
        OrangeArcherButton.SetActive(false);
        OrangeSpearmanButton.SetActive(false);

        VioletSwordmanButton.SetActive(false);
        VioletArcherButton.SetActive(false);
        VioletSpearmanButton.SetActive(false);

        RedPlusViolet.SetActive(false);
        BluePlusGreen.SetActive(false);
        YellowPlusOrange.SetActive(false);

        //storyModeEeterButton.SetActive(false);
    }











}
