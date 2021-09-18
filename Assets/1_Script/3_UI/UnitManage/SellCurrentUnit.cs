using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellCurrentUnit : MonoBehaviour
{
    string unitName
    {
        get
        {
            return FindObjectOfType<StoryMode>().unitTagName;
        }
    }

    GameObject unit;
    public void SellActiveUnit()
    {
        int random = Random.Range(0, 100);
        int reword = CheckUnitType();
        if (unit != null) Destroy(unit.transform.parent.gameObject);
        else return;

        if (10 > random)
        {
            GameManager.instance.Food += reword;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
            SoundManager.instance.PlayEffectSound_ByName("GetFood", 2f);
        }
        else SoundManager.instance.PlayEffectSound_ByName("TP_Unit", 0.3f);
    }

    public int CheckUnitType()
    {
        unit = GameObject.FindGameObjectWithTag(unitName);
        TeamSoldier ts = unit.GetComponent<TeamSoldier>();

        int rewordFood = 0;
        if(ts.GetComponent<Unit_Swordman>() != null) rewordFood = 1;
        else if (ts.GetComponent<Unit_Archer>() != null) rewordFood = 2;
        else if (ts.GetComponent<Unit_Spearman>() != null) rewordFood = 7;
        else if (ts.GetComponent<Unit_Mage>() != null) rewordFood = 20;
        else Debug.Log("타잆 없음");

        return rewordFood;
    }
}
