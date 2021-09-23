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

    GameObject unit
    {
        get
        {
            return GameObject.FindGameObjectWithTag(unitName);
        }
    }
    public void SellActiveUnit()
    {
        if (unit == null) return;

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
        Destroy(unit.transform.parent.gameObject);
    }

    public int GetUnitReword()
    {
        TeamSoldier ts = unit.GetComponent<TeamSoldier>();

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
