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
    public GameObject[] GreenSwordman;
    public GameObject[] GreenArcher;
    public GameObject[] GreenSpearman;
    public GameObject[] GreenMage;
    public GameObject[] OrangeSwordman;
    public GameObject[] OrangeArcher;
    public GameObject[] OrangeSpearman;
    public GameObject[] OrangeMage;
    public GameObject[] VioletSwordman;
    public GameObject[] VioletArcher;
    public GameObject[] VioletSpearman;
    public GameObject[] VioletMage;
    public GameObject[] BlackSwordman;
    public GameObject[] BlackArcher;
    public GameObject[] BlackSpearman;
    public GameObject[] BlackMage;
    public GameObject[] WhiteSwordman;
    public GameObject[] WhiteArcher;
    public GameObject[] WhiteSpearman;
    public GameObject[] WhiteMage;

    public static Dictionary<string, GameObject[]> dic_CurrentUnits;

    private void Awake()
    {
        dic_CurrentUnits = new Dictionary<string, GameObject[]>();

        dic_CurrentUnits.Add("RedSwordman", RedSwordman);
        dic_CurrentUnits.Add("RedArcher", RedArcher);
        dic_CurrentUnits.Add("RedSpearman", RedSpearman);
        dic_CurrentUnits.Add("RedMage", RedMage);
        dic_CurrentUnits.Add("BlueSwordman", BlueSwordman);
        dic_CurrentUnits.Add("BlueArcher", BlueArcher);
        dic_CurrentUnits.Add("BlueSpearman", BlueSpearman);
        dic_CurrentUnits.Add("BlueMage", BlueMage);
        dic_CurrentUnits.Add("YellowSwordman", YellowSwordman);
        dic_CurrentUnits.Add("YellowArcher", YellowArcher);
        dic_CurrentUnits.Add("YellowSpearman", YellowSpearman);
        dic_CurrentUnits.Add("YellowMage", YellowMage);

        dic_CurrentUnits.Add("GreenSwordman", GreenSwordman);
        dic_CurrentUnits.Add("GreenArcher", GreenArcher);
        dic_CurrentUnits.Add("GreenSpearman", GreenSpearman);
        dic_CurrentUnits.Add("GreenMage", GreenMage);
        dic_CurrentUnits.Add("OrangeSwordman", OrangeSwordman);
        dic_CurrentUnits.Add("OrangeArcher", OrangeArcher);
        dic_CurrentUnits.Add("OrangeSpearman", OrangeSpearman);
        dic_CurrentUnits.Add("OrangeMage", OrangeMage);
        dic_CurrentUnits.Add("VioletSwordman", VioletSwordman);
        dic_CurrentUnits.Add("VioletArcher", VioletArcher);
        dic_CurrentUnits.Add("VioletSpearman", VioletSpearman);
        dic_CurrentUnits.Add("VioletMage", VioletMage);
    }

    private void Update()
    {
        dic_CurrentUnits["RedSwordman"] = RedSwordman;
        dic_CurrentUnits["RedArcher"] = RedArcher;
        dic_CurrentUnits["RedSpearman"] = RedSpearman;
        dic_CurrentUnits["RedMage"] = RedMage;
        dic_CurrentUnits["BlueSwordman"] = BlueSwordman;
        dic_CurrentUnits["BlueArcher"] = BlueArcher;
        dic_CurrentUnits["BlueSpearman"] = BlueSpearman;
        dic_CurrentUnits["BlueMage"] = BlueMage;
        dic_CurrentUnits["YellowSwordman"] = YellowSwordman;
        dic_CurrentUnits["YellowArcher"] = YellowArcher;
        dic_CurrentUnits["YellowSpearman"] = YellowSpearman;
        dic_CurrentUnits["YellowMage"] = YellowMage;
        dic_CurrentUnits["GreenSwordman"] = GreenSwordman;
        dic_CurrentUnits["GreenArcher"] = GreenArcher;
        dic_CurrentUnits["GreenSpearman"] = GreenSpearman;
        dic_CurrentUnits["GreenMage"] = GreenMage;
        dic_CurrentUnits["OrangeSwordman"] = OrangeSwordman;
        dic_CurrentUnits["OrangeArcher"] = OrangeArcher;
        dic_CurrentUnits["OrangeSpearman"] = OrangeSpearman;
        dic_CurrentUnits["OrangeMage"] = OrangeMage;
        dic_CurrentUnits["VioletSwordman"] = VioletSwordman;
        dic_CurrentUnits["VioletArcher"] = VioletArcher;
        dic_CurrentUnits["VioletSpearman"] = VioletSpearman;
        dic_CurrentUnits["VioletMage"] = VioletMage;
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
        RedSpearman = GameObject.FindGameObjectsWithTag("RedSpearman");

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

    public void GreenSwordmanTag()
    {
        GreenSwordman = GameObject.FindGameObjectsWithTag("GreenSwordman");

    }
    public void GreenArcherTag()
    {
        GreenArcher = GameObject.FindGameObjectsWithTag("GreenArcher");

    }
    public void GreenSpearmanTag()
    {
        GreenSpearman = GameObject.FindGameObjectsWithTag("GreenSpearman");

    }
    public void GreenMageTag()
    {
        GreenMage = GameObject.FindGameObjectsWithTag("GreenMage");

    }

    public void OrangeSwordmanTag()
    {
        OrangeSwordman = GameObject.FindGameObjectsWithTag("OrangeSwordman");

    }
    public void OrangeArcherTag()
    {
        OrangeArcher = GameObject.FindGameObjectsWithTag("OrangeArcher");

    }
    public void OrangeSpearmanTag()
    {
        OrangeSpearman = GameObject.FindGameObjectsWithTag("OrangeSpearman");

    }
    public void OrangeMageTag()
    {
        OrangeMage = GameObject.FindGameObjectsWithTag("OrangeMage");

    }

    public void VioletSwordmanTag()
    {
        VioletSwordman = GameObject.FindGameObjectsWithTag("VioletSwordman");

    }
    public void VioletArcherTag()
    {
        VioletArcher = GameObject.FindGameObjectsWithTag("VioletArcher");

    }
    public void VioletSpearmanTag()
    {
        VioletSpearman = GameObject.FindGameObjectsWithTag("VioletSpearman");

    }
    public void VioletMageTag()
    {
        VioletMage = GameObject.FindGameObjectsWithTag("VioletMage");

    }

    public void BlackSwordmanTag()
    {
        BlackSwordman = GameObject.FindGameObjectsWithTag("BlackSwordman");

    }
    public void BlackArcherTag()
    {
        BlackArcher = GameObject.FindGameObjectsWithTag("BlackArcher");

    }
    public void BlackSpearmanTag()
    {
        BlackSpearman = GameObject.FindGameObjectsWithTag("BlackSpearman");

    }
    public void BlackMageTag()
    {
        BlackMage = GameObject.FindGameObjectsWithTag("BlackMage");

    }

    public void WhiteSwordmanTag()
    {
        WhiteSwordman = GameObject.FindGameObjectsWithTag("WhiteSwordman");

    }
    public void WhiteArcherTag()
    {
        WhiteArcher = GameObject.FindGameObjectsWithTag("WhiteArcher");

    }
    public void WhiteSpearmanTag()
    {
        WhiteSpearman = GameObject.FindGameObjectsWithTag("WhiteSpearman");

    }
    public void WhiteMageTag()
    {
        WhiteMage = GameObject.FindGameObjectsWithTag("WhiteMage");

    }
}
