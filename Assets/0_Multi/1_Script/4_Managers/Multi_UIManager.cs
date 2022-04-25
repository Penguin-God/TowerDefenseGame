using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Multi_UIManager : MonoBehaviour
{
    private static Multi_UIManager m_instance;
    public static Multi_UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<Multi_UIManager>();
            }
            return m_instance;
        }
    }

    private void Start()
    {
        Multi_GameManager.instance.OnStart += Set_GameUI;
        Multi_GameManager.instance.OnStart += () => UpdateStageText(1);

        Multi_EnemySpawner.instance.OnStartNewStage += UpdateStageText;
    }

    [SerializeField] GameObject title_UI;
    [SerializeField] GameObject game_UI;
    public void Set_GameUI()
    {
        title_UI.SetActive(false);
        game_UI.SetActive(true);
    }

    [SerializeField] Text StageText;
    [SerializeField] Text GoldText;
    [SerializeField] Text FoodText;
    [SerializeField] Text EnemyCount;
    [SerializeField] Text CurrnetUnitText;
    [SerializeField] Text GameOverText;

    [SerializeField] Text gameResultText;
    // public Button SoldierCombine;

    public GameObject BlackTowerButton;
    public GameObject WhiteTowerButton;

    public Text CombineSuccessText;
    public Text CombineFailText;

    public void UpdateStageText(int Stage) => StageText.text = "현재 스테이지 : " + Stage;

    public void UpdateGoldText(int Gold) => GoldText.text = "" + Gold;
    public void UpdateFoodText(int Food) => FoodText.text = "" + Food;

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


    public void SetActiveGameOverUI()
    {
        GameOverText.gameObject.SetActive(true);
    }

    public GameObject clearObject;
    public void SetActiveClearUI()
    {
        clearObject.SetActive(true);
        //ClearText.gameObject.SetActive(true);
        //RestartText.gameObject.SetActive(true);
    }
}
