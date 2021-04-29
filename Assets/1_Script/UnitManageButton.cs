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

}
