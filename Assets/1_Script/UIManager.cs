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
   // public Button SoldierCombine;
    public Button SellSoldier;
    public GameObject SoldiersCombineButton;

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

    public void UpdateCountEnemyText(int EnemyofCount)
    {
        EnemyCount.text = "Enemy :" + EnemyofCount;
    }

    public void SetActiveGameOverUI()
    {
        GameOverText.gameObject.SetActive(true);
        RestartText.gameObject.SetActive(true);
    }

    public void ButtonDown()
    {
        SetActiveButton(false, 0, 0);
        SetActiveButton(false, 1, 0);
        SetActiveButton(false, 2, 0);
    }
}

