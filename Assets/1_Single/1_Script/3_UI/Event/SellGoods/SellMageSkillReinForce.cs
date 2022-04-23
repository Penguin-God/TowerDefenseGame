using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellMageSkillReinForce : MonoBehaviour, ISellEventShopItem
{
    [SerializeField] UnitColor mageColor;

    public void Sell_Item()
    {
        BuyMageUltimate();
    }

    void BuyMageUltimate()
    {
        EventManager.instance.UpMageUltimateFlag(mageColor);
        TeamSoldier[] _units = UnitManager.instance.GetCurrnetUnits(mageColor, UnitClass.mage);
        SetMageUltimate(_units);
    }

    // 현재 필드에 있는 법사 강화
    //void SetCurrentMageUltimate(int mageColorNumber)
    //{
    //    switch (mageColorNumber)
    //    {
    //        case 0:
    //            GameObject[] redMages = GameObject.FindGameObjectsWithTag("RedMage");
    //            SetMageUltimate(redMages);
    //            break;
    //        case 1:
    //            GameObject[] blueMages = GameObject.FindGameObjectsWithTag("BlueMage");
    //            SetMageUltimate(blueMages);
    //            break;
    //        case 2:
    //            GameObject[] yellowMages = GameObject.FindGameObjectsWithTag("YellowMage");
    //            SetMageUltimate(yellowMages);
    //            break;
    //        case 3:
    //            GameObject[] greenMages = GameObject.FindGameObjectsWithTag("GreenMage");
    //            SetMageUltimate(greenMages);
    //            break;
    //        case 4:
    //            GameObject[] orangeMages = GameObject.FindGameObjectsWithTag("OrangeMage");
    //            SetMageUltimate(orangeMages);
    //            break;
    //        case 5:
    //            GameObject[] violetMages = GameObject.FindGameObjectsWithTag("VioletMage");
    //            SetMageUltimate(violetMages);
    //            break;
    //        case 6:
    //            GameObject[] blackMages = GameObject.FindGameObjectsWithTag("BlackMage");
    //            SetMageUltimate(blackMages);
    //            break;
    //    }
    //}

    void SetMageUltimate(TeamSoldier[] _mages)
    {
        for (int i = 0; i < _mages.Length; i++)
        {
            Unit_Mage _mage = _mages[i].GetComponent<Unit_Mage>();
            _mage.isUltimate = true;
        }
    }
}
