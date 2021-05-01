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
    public Text FoodText;
    public Text EnemyCount;
    public Text GameOverText;
    public Text RestartText;
    public Text ClearText;
    public SoldiersTags SoldiersTag;
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

    public void UpdateFoodText(int Food)
    {
        FoodText.text = "" + Food;
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

    public Text RedSwordmanText;
    public Text BlueSwordmanText;
    public Text YellowSwordmanText;
    public Text GreenSwordmanText;
    public Text OrangeSwordmanText;
    public Text VioletSwordmanText;

    public Text RedArcherText;
    public Text BlueArcherText;
    public Text YellowArcherText;
    public Text GreenArcherText;
    public Text OrangeArcherText;
    public Text VioletArcherText;

    public void UpdateSwordmanText(int RedSwordman,int BlueSwordman, int YellowSwordman, int GreenSwordman, int OrangeSwordman, int VioletSwordman)
    {
        RedSwordmanText.text = "빨간기사 :" + RedSwordman;
        BlueSwordmanText.text = "파란기사 :" + BlueSwordman;
        YellowSwordmanText.text = "노란기사 :" + YellowSwordman;
        GreenSwordmanText.text = "초록기사 :" + GreenSwordman;
        OrangeSwordmanText.text = "주황기사 :" + OrangeSwordman;
        VioletSwordmanText.text = "보라기사 :" + VioletSwordman;
    }

    public void UpdateArcherText(int RedArcher, int BlueArcher, int YellowArcher, int GreenArcher, int OrangeArcher, int VioletArcher)
    {
        RedArcherText.text = "빨간아처 :" + RedArcher;
        BlueArcherText.text = "파란아처 :" + BlueArcher;
        YellowArcherText.text = "노란아처 :" + YellowArcher;
        GreenArcherText.text = "초록아처 :" + GreenArcher;
        OrangeArcherText.text = "주황아처 :" + OrangeArcher;
        VioletArcherText.text = "보라아처 :" + VioletArcher;
    }

    public void UpdateSwordmanCount()
    {
        SoldiersTag.RedSwordmanTag();
        SoldiersTag.BlueSwordmanTag();
        SoldiersTag.YellowSwordmanTag();
        SoldiersTag.GreenSwordmanTag();
        SoldiersTag.OrangeSwordmanTag();
        SoldiersTag.VioletSwordmanTag();
        int RedSwordmanCount = SoldiersTag.RedSwordman.Length;
        int BlueSwordmanCount = SoldiersTag.BlueSwordman.Length;
        int YellowSwordmanCount = SoldiersTag.YellowSwordman.Length;
        int GreenSwordmanCount = SoldiersTag.GreenSwordman.Length;
        int OrangeSwordmanCount = SoldiersTag.OrangeSwordman.Length;
        int VioletSwordmanCount = SoldiersTag.VioletSwordman.Length;

        UpdateSwordmanText(RedSwordmanCount, BlueSwordmanCount, YellowSwordmanCount, GreenSwordmanCount, OrangeSwordmanCount, VioletSwordmanCount);
    }

    public void UpdateArcherCount()
    {
        SoldiersTag.RedArcherTag();
        SoldiersTag.BlueArcherTag();
        SoldiersTag.YellowArcherTag();
        SoldiersTag.GreenArcherTag();
        SoldiersTag.OrangeArcherTag();
        SoldiersTag.VioletArcherTag();
        int RedArcherCount = SoldiersTag.RedSwordman.Length;
        int BlueArcherCount = SoldiersTag.BlueSwordman.Length;
        int YellowArcherCount = SoldiersTag.YellowSwordman.Length;
        int GreenArcherCount = SoldiersTag.GreenSwordman.Length;
        int OrangeArcherCount = SoldiersTag.OrangeSwordman.Length;
        int VioletArcherCount = SoldiersTag.VioletSwordman.Length;

        UpdateArcherText(RedArcherCount, BlueArcherCount, YellowArcherCount, GreenArcherCount, OrangeArcherCount, VioletArcherCount);
    }
}

