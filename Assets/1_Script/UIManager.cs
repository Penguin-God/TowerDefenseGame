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
    public Button SoldierCombine;
    public Button SellSoldier;

    public void UpdateStageText(int Stage)
    {
        StageText.text = "Stage :" + Stage;
    }

    public void UpdateGoldText(int Gold)
    {
        GoldText.text = ""+Gold;
    }

    public void SetActiveButton(bool show)
    {
        SoldierCombine.gameObject.SetActive(show);
        SellSoldier.gameObject.SetActive(show);
    }

    public void UpdateCountEnemyText(int EnemyofCount)
    {
        EnemyCount.text = "Enemy :" + EnemyofCount;
    }
}

