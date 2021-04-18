using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }

    private static UIManager m_instance;

    public Text StageText;
    public Text GoldText;
    public Text EnemyCount;
    public Text GameOverText;
    public Text RestartText;
    public Text ClearText;
    // public Button SoldierCombine;
    public Button SellSoldier;
    public GameObject SoldiersCombineButton;
    public GameObject SoldiersCombineButton2;

    public void UpdateStageText(int Stage)
    {
        StageText.text = "Stage :" + Stage;
    }

    public void UpdateGoldText(int Gold)
    {
        GoldText.text = ""+Gold;
    }

    public void SetActiveButton(bool show, int SoldiersColornumber, int Soldiersnumber)
    {
        //SoldierCombine.gameObject.SetActive(show);
        SellSoldier.gameObject.SetActive(show);
        SoldiersCombineButton.transform.GetChild(SoldiersColornumber).transform.GetChild(Soldiersnumber).gameObject.SetActive(show);
    }

    public void SetActiveButton2(bool show, int SoldiersColornumber, int Soldiersnumber)
    {
        //SoldierCombine.gameObject.SetActive(show);
        //SellSoldier.gameObject.SetActive(show);
        SoldiersCombineButton2.transform.GetChild(SoldiersColornumber).transform.GetChild(Soldiersnumber).gameObject.SetActive(show);
    }

    public void UpdateCountEnemyText(int EnemyofCount)
    {
        EnemyCount.text = "Enemy :" + EnemyofCount;
    }

    public void SetActiveGameOverUI()
    {
        GameOverText.gameObject.SetActive(true);
        RestartText.gameObject.SetActive(true);
    }

    public void SetActiveClearUI()
    {
        ClearText.gameObject.SetActive(true);
        RestartText.gameObject.SetActive(true);
    }

    public void ButtonDown()
    {
        SetActiveButton(false, 0, 0);
        SetActiveButton(false, 1, 0);
        SetActiveButton(false, 2, 0);
        SetActiveButton(false, 3, 0);
        SetActiveButton(false, 4, 0);
        SetActiveButton(false, 5, 0);
        SetActiveButton(false, 0, 1);
        SetActiveButton(false, 1, 1);
        SetActiveButton(false, 2, 1);
        SetActiveButton(false, 3, 1);
        SetActiveButton(false, 4, 1);
        SetActiveButton(false, 5, 1);
        SetActiveButton(false, 0, 2);
        SetActiveButton(false, 1, 2);
        SetActiveButton(false, 2, 2);
        SetActiveButton(false, 3, 2);
        SetActiveButton(false, 4, 2);
        SetActiveButton(false, 5, 2);

        SetActiveButton2(false, 0, 0);
        SetActiveButton2(false, 1, 0);
        SetActiveButton2(false, 2, 0);
    }
}

