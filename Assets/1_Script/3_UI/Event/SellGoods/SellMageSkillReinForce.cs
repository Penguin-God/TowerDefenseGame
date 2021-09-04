using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellMageSkillReinForce : MonoBehaviour, ISellEventShopItem
{
    [SerializeField] int mageColorNumber;

    public void Sell_Item()
    {
        BuyMageUltimate();
    }

    void BuyMageUltimate()
    {
        GameObject mage = UnitManager.instance.unitArrays[mageColorNumber].unitArray[3];
        mage.GetComponentInChildren<Unit_Mage>().isUltimate = true;
        SetCurrentMageUltimate(mageColorNumber);
    }
    // 현재 필드에 있는 법사 강화
    void SetCurrentMageUltimate(int mageColorNumber)
    {
        switch (mageColorNumber)
        {
            case 0:
                GameObject[] redMages = GameObject.FindGameObjectsWithTag("RedMage");
                SetMageUltimate(redMages);
                break;
            case 1:
                GameObject[] blueMages = GameObject.FindGameObjectsWithTag("BlueMage");
                SetMageUltimate(blueMages);
                break;
            case 2:
                GameObject[] yellowMages = GameObject.FindGameObjectsWithTag("YellowMage");
                SetMageUltimate(yellowMages);
                break;
            case 3:
                GameObject[] greenMages = GameObject.FindGameObjectsWithTag("GreenMage");
                SetMageUltimate(greenMages);
                break;
            case 4:
                GameObject[] orangeMages = GameObject.FindGameObjectsWithTag("OrangeMage");
                SetMageUltimate(orangeMages);
                break;
            case 5:
                GameObject[] violetMages = GameObject.FindGameObjectsWithTag("VioletMage");
                SetMageUltimate(violetMages);
                break;
            case 6:
                GameObject[] blackMages = GameObject.FindGameObjectsWithTag("BlackMage");
                SetMageUltimate(blackMages);
                break;
        }
    }

    void SetMageUltimate(GameObject[] mages)
    {
        for (int i = 0; i < mages.Length; i++)
        {
            Unit_Mage mage = mages[i].GetComponentInParent<Unit_Mage>();
            mage.isUltimate = true;
        }
    }
}
