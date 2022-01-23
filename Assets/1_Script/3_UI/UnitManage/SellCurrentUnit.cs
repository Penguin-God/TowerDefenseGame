using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellCurrentUnit : MonoBehaviour
{
    string UnitName { get { return FindObjectOfType<StoryMode>().unitTagName; } }
    GameObject SellUnit
    {
        get
        {
            TeamSoldier[] _units = UnitManager.instance.GetCurrnetUnits(UnitName);
            if (_units != null && _units.Length > 0) return _units[0].gameObject;
            else return null;
        }
    }

    public void SellActiveUnit()
    {
        if (SellUnit == null) return;

        int reword = GetUnitReword();
        int random = Random.Range(0, 100);

        if (10 > random)
        {
            GameManager.instance.Food += reword;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
            SoundManager.instance.PlayEffectSound_ByName("GetFood", 2f);
            ShowText(successText);
        }
        else
        {
            SoundManager.instance.PlayEffectSound_ByName("PopSound16");
            ShowText(failText);
        }
        CombineSoldierPooling.ReturnObject(SellUnit.transform.parent.gameObject, SellUnit.tag);
    }

    public int GetUnitReword()
    {
        TeamSoldier ts = SellUnit.GetComponent<TeamSoldier>();

        int rewordFood = 0;
        if(ts.GetComponent<Unit_Swordman>() != null) rewordFood = 1;
        else if (ts.GetComponent<Unit_Archer>() != null) rewordFood = 2;
        else if (ts.GetComponent<Unit_Spearman>() != null) rewordFood = 7;
        else if (ts.GetComponent<Unit_Mage>() != null) rewordFood = 20;
        else Debug.Log("타잆 없음");

        return rewordFood;
    }

    [SerializeField] GameObject successText = null;
    [SerializeField] GameObject failText = null;

    void ShowText(GameObject textObj) => StartCoroutine(Co_ShowText(textObj));

    IEnumerator Co_ShowText(GameObject textObj)
    {
        textObj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        textObj.SetActive(false);
    }
}
