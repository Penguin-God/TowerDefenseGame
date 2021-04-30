using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManageButton : MonoBehaviour
{
    public GameObject UnitManagementButton;
    public GameObject SwordmansButton;
    public GameObject ArchersButton;
    public GameObject SpearmansButton;
    public GameObject MagesButton;
    public GameObject ColorSwordmansButton;
    public GameObject ColorArchersButton;
    public GameObject ColorSpearmansButton;
    public GameObject ColorMagesButton;

    public void FirstChilk()
    {
        UnitManagementButton.gameObject.SetActive(false);
        SwordmansButton.gameObject.SetActive(true);
        ArchersButton.gameObject.SetActive(true);
        SpearmansButton.gameObject.SetActive(true);
        MagesButton.gameObject.SetActive(true);
    }

    public void ManageButtonDown()
    {
        SwordmansButton.gameObject.SetActive(false);
        ArchersButton.gameObject.SetActive(false);
        SpearmansButton.gameObject.SetActive(false);
        MagesButton.gameObject.SetActive(false);
    }

    public void ChlikSwordmanButton()
    {
        ManageButtonDown();
        ColorSwordmansButton.gameObject.SetActive(true);
    }

    public void ChlikArcherButton()
    {
        ManageButtonDown();
        ColorArchersButton.gameObject.SetActive(true);
    }

    public void ChlikSpearmanButton()
    {
        ManageButtonDown();
        ColorSpearmansButton.gameObject.SetActive(true);
    }

    public void ChlikMageButton()
    {
        ManageButtonDown();
        ColorMagesButton.gameObject.SetActive(true);
    }

    public void ChlikRedSwordmanButton()   // 기사 6개버튼
    {
        ColorSwordmansButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikBlueSwordmanButton()
    {
        ColorSwordmansButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikYellowSwordmanButton()
    {
        ColorSwordmansButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikGreenSwordmanButton()
    {
        ColorSwordmansButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikOrangeSwordmanButton()
    {
        ColorSwordmansButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikVioletSwordmanButton()
    {
        ColorSwordmansButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikRedArcherButton()  // 아처 6개 버튼
    {
        ColorArchersButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikBlueArcherButton()
    {
        ColorArchersButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikYellowArcherButton()
    {
        ColorArchersButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikGreenArcherButton()
    {
        ColorArchersButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikOrangeArcherButton()
    {
        ColorArchersButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikVioletArcherButton()
    {
        ColorArchersButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikRedSpearmanButton()  // 창병 6개 버튼
    {
        ColorSpearmansButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikBlueSpearmanButton()
    {
        ColorSpearmansButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikYellowSpearmanButton()
    {
        ColorSpearmansButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikGreenSpearmanButton()
    {
        ColorSpearmansButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikOrangeSpearmanButton()
    {
        ColorSpearmansButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikVioletSpearmanButton()
    {
        ColorSpearmansButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikRedMageButton()  // 마법사 6개 버튼
    {
        ColorMagesButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikBlueMageButton()
    {
        ColorMagesButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikYellowMageButton()
    {
        ColorMagesButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikGreenMageButton()
    {
        ColorMagesButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikOrangeMageButton()
    {
        ColorMagesButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

    public void ChlikVioletMageButton()
    {
        ColorMagesButton.gameObject.SetActive(false);
        UnitManagementButton.gameObject.SetActive(true);
    }

}
