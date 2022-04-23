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

    [SerializeField] GameObject title_UI;
    [SerializeField] GameObject game_UI;
    public void Set_GameUI()
    {
        title_UI.SetActive(false);
        game_UI.SetActive(true);
    }

    private static UIManager m_instance;

    public Text StageText;
    public Text GoldText;
    public Text FoodText;
    public Text EnemyCount;
    public Text CurrnetUnitText;
    public Text GameOverText;
    public Text RestartText;
    public Text ClearText;
    public SoldiersTags SoldiersTag;
    // public Button SoldierCombine;
    public Button SellSoldier;

    public GameObject SoldiersCombineButton;
    public GameObject SoldiersCombineButton2;

    public GameObject BackGround;

    public GameObject ExPlanationTexts;

    public GameObject BlackTowerButton;
    public GameObject WhiteTowerButton;

    public GameObject FailText;
    public GameObject SuccessText;

    public GameObject XButton;

    public AudioSource CreateButtonAuido;

    public Text CombineSuccessText;
    public Text CombineFailText;

    public Text HighScoreText;

    private void Start()
    {
        GameManager.instance.OnStart += () => UpdateStageText(1);
    }

    public void UpdateHighScoreText(int HighScore)
    {
        HighScoreText.text = "최고 스테이지 : " + HighScore;
    }

    public void UpdateStageText(int Stage)
    {
        StageText.text = "현재 스테이지 : " + Stage + " / " + ((GameManager.instance.isChallenge) ? "Infinity" : EnemySpawn.instance.maxStage.ToString());
    }

    public void UpdateGoldText(int Gold)
    {
        GoldText.text = "" + Gold;
    }

    public void UpdateFoodText(int Food)
    {
        FoodText.text = "" + Food;
    }

    public void UpdateCountEnemyText(int EnemyofCount)
    {
        if (EnemyofCount > 45) EnemyCount.color = new Color32(255, 0, 0, 255);
        else EnemyCount.color = new Color32(255, 255, 255, 255);
        EnemyCount.text = "현재 적 유닛 카운트 : " + EnemyofCount + "/50";
    }

    public void UpdateCombineSuccessText(string Moonja)
    {
        CombineSuccessText.text = Moonja;
    }

    public void UpdateCurrentUnitText(int currentUnit, int maxUnit)
    {
        string text = "최대 유닛 갯수 " + currentUnit + "/" + maxUnit;
        CurrnetUnitText.text = text;
    }

    public void SetActiveButton(bool show, int SoldiersColornumber, int Soldiersnumber)
    {
        //SoldierCombine.gameObject.SetActive(show)
        //SellSoldier.gameObject.SetActive(show);
        SoldiersCombineButton.transform.GetChild(SoldiersColornumber).transform.GetChild(Soldiersnumber).gameObject.SetActive(show);
    }

    public void SetActiveButton2(bool show, int SoldiersColornumber, int Soldiersnumber)
    {
        //SoldierCombine.gameObject.SetActive(show);
        //SellSoldier.gameObject.SetActive(show);
        SoldiersCombineButton2.transform.GetChild(SoldiersColornumber).transform.GetChild(Soldiersnumber).gameObject.SetActive(show);
    }



    public void SetActiveGameOverUI()
    {
        GameOverText.gameObject.SetActive(true);
        RestartText.gameObject.SetActive(true);
    }

    public GameObject clearObject;
    public void SetActiveClearUI()
    {
        clearObject.SetActive(true);
        //ClearText.gameObject.SetActive(true);
        //RestartText.gameObject.SetActive(true);
    }

    public void ButtonDown()
    {
        SellSoldier.gameObject.SetActive(false);
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
        SetActiveButton(false, 0, 3);
        SetActiveButton(false, 1, 3);
        SetActiveButton(false, 2, 3);
        SetActiveButton(false, 3, 3);
        SetActiveButton(false, 4, 3);
        SetActiveButton(false, 5, 3);

        SetActiveButton2(false, 0, 0);
        SetActiveButton2(false, 1, 0);
        SetActiveButton2(false, 2, 0);
    }

    //public Text RedSwordmanText;
    //public Text BlueSwordmanText;
    //public Text YellowSwordmanText;
    //public Text GreenSwordmanText;
    //public Text OrangeSwordmanText;
    //public Text VioletSwordmanText;

    //public Text RedArcherText;
    //public Text BlueArcherText;
    //public Text YellowArcherText;
    //public Text GreenArcherText;
    //public Text OrangeArcherText;
    //public Text VioletArcherText;

    //public Text RedSpearmanText;
    //public Text BlueSpearmanText;
    //public Text YellowSpearmanText;
    //public Text GreenSpearmanText;
    //public Text OrangeSpearmanText;
    //public Text VioletSpearmanText;

    //public Text RedMageText;
    //public Text BlueMageText;
    //public Text YellowMageText;
    //public Text GreenMageText;
    //public Text OrangeMageText;
    //public Text VioletMageText;

    //public void UpdateSwordmanText(int RedSwordman, int BlueSwordman, int YellowSwordman, int GreenSwordman, int OrangeSwordman, int VioletSwordman)
    //{
    //    RedSwordmanText.text = "빨간기사 :" + RedSwordman;
    //    BlueSwordmanText.text = "파란기사 :" + BlueSwordman;
    //    YellowSwordmanText.text = "노란기사 :" + YellowSwordman;
    //    GreenSwordmanText.text = "초록기사 :" + GreenSwordman;
    //    OrangeSwordmanText.text = "주황기사 :" + OrangeSwordman;
    //    VioletSwordmanText.text = "보라기사 :" + VioletSwordman;
    //}

    //public void UpdateArcherText(int RedArcher, int BlueArcher, int YellowArcher, int GreenArcher, int OrangeArcher, int VioletArcher)
    //{
    //    RedArcherText.text = "빨간궁수 :" + RedArcher;
    //    BlueArcherText.text = "파란궁수 :" + BlueArcher;
    //    YellowArcherText.text = "노란궁수 :" + YellowArcher;
    //    GreenArcherText.text = "초록궁수 :" + GreenArcher;
    //    OrangeArcherText.text = "주황궁수 :" + OrangeArcher;
    //    VioletArcherText.text = "보라궁수 :" + VioletArcher;
    //}

    //public void UpdateSpearmanText(int RedSpearman, int BlueSpearman, int YellowSpearman, int GreenSpearman, int OrangeSpearman, int VioletSpearman)
    //{
    //    RedSpearmanText.text = "빨간창병 :" + RedSpearman;
    //    BlueSpearmanText.text = "파란창병 :" + BlueSpearman;
    //    YellowSpearmanText.text = "노란창병 :" + YellowSpearman;
    //    GreenSpearmanText.text = "초록창병 :" + GreenSpearman;
    //    OrangeSpearmanText.text = "주황창병 :" + OrangeSpearman;
    //    VioletSpearmanText.text = "보라창병 :" + VioletSpearman;
    //}

    //public void UpdateMageText(int RedMage, int BlueMage, int YellowMage, int GreenMage, int OrangeMage, int VioletMage)
    //{
    //    RedMageText.text = "빨간마법사 :" + RedMage;
    //    BlueMageText.text = "파란마법사 :" + BlueMage;
    //    YellowMageText.text = "노란마법사 :" + YellowMage;
    //    GreenMageText.text = "초록마법사 :" + GreenMage;
    //    OrangeMageText.text = "주황마법사 :" + OrangeMage;
    //    VioletMageText.text = "보라마법사 :" + VioletMage;
    //}

    //public void UpdateSwordmanCount()
    //{
    //    SoldiersTag.RedSwordmanTag();
    //    SoldiersTag.BlueSwordmanTag();
    //    SoldiersTag.YellowSwordmanTag();
    //    SoldiersTag.GreenSwordmanTag();
    //    SoldiersTag.OrangeSwordmanTag();
    //    SoldiersTag.VioletSwordmanTag();
    //    int RedSwordmanCount = SoldiersTag.RedSwordman.Length;
    //    int BlueSwordmanCount = SoldiersTag.BlueSwordman.Length;
    //    int YellowSwordmanCount = SoldiersTag.YellowSwordman.Length;
    //    int GreenSwordmanCount = SoldiersTag.GreenSwordman.Length;
    //    int OrangeSwordmanCount = SoldiersTag.OrangeSwordman.Length;
    //    int VioletSwordmanCount = SoldiersTag.VioletSwordman.Length;

    //    UpdateSwordmanText(RedSwordmanCount, BlueSwordmanCount, YellowSwordmanCount, GreenSwordmanCount, OrangeSwordmanCount, VioletSwordmanCount);
    //}

    //public void UpdateArcherCount()
    //{
    //    SoldiersTag.RedArcherTag();
    //    SoldiersTag.BlueArcherTag();
    //    SoldiersTag.YellowArcherTag();
    //    SoldiersTag.GreenArcherTag();
    //    SoldiersTag.OrangeArcherTag();
    //    SoldiersTag.VioletArcherTag();
    //    int RedArcherCount = SoldiersTag.RedArcher.Length;
    //    int BlueArcherCount = SoldiersTag.BlueArcher.Length;
    //    int YellowArcherCount = SoldiersTag.YellowArcher.Length;
    //    int GreenArcherCount = SoldiersTag.GreenArcher.Length;
    //    int OrangeArcherCount = SoldiersTag.OrangeArcher.Length;
    //    int VioletArcherCount = SoldiersTag.VioletArcher.Length;

    //    UpdateArcherText(RedArcherCount, BlueArcherCount, YellowArcherCount, GreenArcherCount, OrangeArcherCount, VioletArcherCount);
    //}

    //public void UpdateSpearmanCount()
    //{
    //    SoldiersTag.RedSpearmanTag();
    //    SoldiersTag.BlueSpearmanTag();
    //    SoldiersTag.YellowSpearmanTag();
    //    SoldiersTag.GreenSpearmanTag();
    //    SoldiersTag.OrangeSpearmanTag();
    //    SoldiersTag.VioletSpearmanTag();
    //    int RedSpearmanCount = SoldiersTag.RedSpearman.Length;
    //    int BlueSpearmanCount = SoldiersTag.BlueSpearman.Length;
    //    int YellowSpearmanCount = SoldiersTag.YellowSpearman.Length;
    //    int GreenSpearmanCount = SoldiersTag.GreenSpearman.Length;
    //    int OrangeSpearmanCount = SoldiersTag.OrangeSpearman.Length;
    //    int VioletSpearmanCount = SoldiersTag.VioletSpearman.Length;

    //    UpdateSpearmanText(RedSpearmanCount, BlueSpearmanCount, YellowSpearmanCount, GreenSpearmanCount, OrangeSpearmanCount, VioletSpearmanCount);
    //}

    //public void UpdateMageCount()
    //{
    //    SoldiersTag.RedMageTag();
    //    SoldiersTag.BlueMageTag();
    //    SoldiersTag.YellowMageTag();
    //    SoldiersTag.GreenMageTag();
    //    SoldiersTag.OrangeMageTag();
    //    SoldiersTag.VioletMageTag();
    //    int RedMageCount = SoldiersTag.RedMage.Length;
    //    int BlueMageCount = SoldiersTag.BlueMage.Length;
    //    int YellowMageCount = SoldiersTag.YellowMage.Length;
    //    int GreenMageCount = SoldiersTag.GreenMage.Length;
    //    int OrangeMageCount = SoldiersTag.OrangeMage.Length;
    //    int VioletMageCount = SoldiersTag.VioletMage.Length;

    //    UpdateMageText(RedMageCount, BlueMageCount, YellowMageCount, GreenMageCount, OrangeMageCount, VioletMageCount);
    //}


}

