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


    public void AllButtonDown()
    {
        XButtonAudio.Play();
        BackGround.gameObject.SetActive(false);
        BlackSoldiersCombineButtons.gameObject.SetActive(false);

        BlackTowerUi.gameObject.SetActive(false);
        WhiteTowerUi.gameObject.SetActive(false);

        UnitManageBUtton.gameObject.SetActive(true);

        SwordmanManageButton.gameObject.SetActive(false);
        ArcherManageButton.gameObject.SetActive(false);
        SpearmanManageButton.gameObject.SetActive(false);
        MageManageButton.gameObject.SetActive(false);

        ManageColorswordmanButton.gameObject.SetActive(false);
        ManageColorArcherButton.gameObject.SetActive(false);
        ManageColorSpearmanButton.gameObject.SetActive(false);
        ManageColorMageButton.gameObject.SetActive(false);

        RedSwordmanExplanText.gameObject.SetActive(false);
        RedArcherExplanText.gameObject.SetActive(false);
        RedSpearmanExplanText.gameObject.SetActive(false);
        RedMageExplanText.gameObject.SetActive(false);

        BlueSwordmanExplanText.gameObject.SetActive(false);
        BlueArcherExplanText.gameObject.SetActive(false);
        BlueSpearmanExplanText.gameObject.SetActive(false);
        BlueMageExplanText.gameObject.SetActive(false);

        YellowSwordmanExplanText.gameObject.SetActive(false);
        YellowArcherExplanText.gameObject.SetActive(false);
        YellowSpearmanExplanText.gameObject.SetActive(false);
        YellowMageExplanText.gameObject.SetActive(false);

        GreenSwordmanExplanText.gameObject.SetActive(false);
        GreenArcherExplanText.gameObject.SetActive(false);
        GreenSpearmanExplanText.gameObject.SetActive(false);
        GreenMageExplanText.gameObject.SetActive(false);

        OrangeSwordmanExplanText.gameObject.SetActive(false);
        OrangeArcherExplanText.gameObject.SetActive(false);
        OrangeSpearmanExplanText.gameObject.SetActive(false);
        OrangeMageExplanText.gameObject.SetActive(false);

        VioletSwordmanExplanText.gameObject.SetActive(false);
        VioletArcherExplanText.gameObject.SetActive(false);
        VioletSpearmanExplanText.gameObject.SetActive(false);
        VioletMageExplanText.gameObject.SetActive(false);

        RedSwordmanButton.gameObject.SetActive(false);
        RedArcherButton.gameObject.SetActive(false);
        RedSpearmanButton.gameObject.SetActive(false);


        BlueSwordmanButton.gameObject.SetActive(false);
        BlueArcherButton.gameObject.SetActive(false);
        BlueSpearmanButton.gameObject.SetActive(false);


        YellowSwordmanButton.gameObject.SetActive(false);
        YellowArcherButton.gameObject.SetActive(false);
        YellowSpearmanButton.gameObject.SetActive(false);


        GreenSwordmanButton.gameObject.SetActive(false);
        GreenArcherButton.gameObject.SetActive(false);
        GreenSpearmanButton.gameObject.SetActive(false);


        OrangeSwordmanButton.gameObject.SetActive(false);
        OrangeArcherButton.gameObject.SetActive(false);
        OrangeSpearmanButton.gameObject.SetActive(false);

        VioletSwordmanButton.gameObject.SetActive(false);
        VioletArcherButton.gameObject.SetActive(false);
        VioletSpearmanButton.SetActive(false);

        RedPlusViolet.SetActive(false);
        BluePlusGreen.SetActive(false);
        YellowPlusOrange.SetActive(false);

        //storyModeEeterButton.SetActive(false);
    }











}
