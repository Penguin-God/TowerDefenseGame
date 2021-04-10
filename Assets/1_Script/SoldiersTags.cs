using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldiersTags : MonoBehaviour
{
    public GameObject[] RedSwordman;
    public GameObject[] RedArcher;
    public GameObject[] RedSpearman;
    public GameObject[] RedMage;
    public GameObject[] BlueSwordman;
    public GameObject[] BlueArcher;
    public GameObject[] BlueSpearman;
    public GameObject[] BlueMage;
    public GameObject[] YellowSwordman;
    public GameObject[] YellowArcher;
    public GameObject[] YellowSpearman;
    public GameObject[] YellowMage;
    void Start()
    {
        
    }



    public void RedSwordmanTag()
    {
        RedSwordman = GameObject.FindGameObjectsWithTag("RedSwordman");

    }
    public void RedArcherTag()
    {
        RedArcher = GameObject.FindGameObjectsWithTag("RedArcher");

    }
    public void RedSpearmanTag()
    {
        RedSwordman = GameObject.FindGameObjectsWithTag("RedSpearman");

    }
    public void RedMageTag()
    {
        RedMage = GameObject.FindGameObjectsWithTag("RedMage");

    }
    public void BlueSwordmanTag()
    {
        BlueSwordman = GameObject.FindGameObjectsWithTag("BlueSwordman");

    }
    public void BlueArcherTag()
    {
        BlueArcher = GameObject.FindGameObjectsWithTag("BlueArcher");

    }
    public void BlueSpearmanTag()
    {
        BlueSpearman = GameObject.FindGameObjectsWithTag("BlueSpearman");

    }
    public void BlueMageTag()
    {
        BlueMage = GameObject.FindGameObjectsWithTag("BlueMage");

    }
    public void YellowSwordmanTag()
    {
        YellowSwordman = GameObject.FindGameObjectsWithTag("YellowSwordman");

    }
    public void YellowArcherTag()
    {
        YellowArcher = GameObject.FindGameObjectsWithTag("YellowArcher");

    }
    public void YellowSpearmanTag()
    {
        YellowSpearman = GameObject.FindGameObjectsWithTag("YellowSpearman");

    }
    public void YellowMageTag()
    {
        YellowMage = GameObject.FindGameObjectsWithTag("YellowMage");

    }
}
