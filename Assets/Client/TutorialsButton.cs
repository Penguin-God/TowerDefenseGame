using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialsButton : MonoBehaviour
{
    public int Count;
    public CombineSoldier combine;
    public UnitManageButton unitManage;
    public CreateDefenser createDefenser;
    public StoryMode storyMode;
    public WhiteTowerEvent whiteTowerEvent;
    public BlackTowerEvent blackTowerEvent;

    void Start()
    {
        Count = 1;
        this.transform.position = SommonSoldiersButton.transform.position;
        TutorialProgressTrue(0);

    }

    public GameObject SommonSoldiersButton;
    public GameObject EnterStoryModeButton;
    public GameObject UnitManageButton;
    public GameObject SwordmansManageButton;
    public GameObject RedSwordmanManageButton;
    public GameObject CombineRedArcherButton;
    public GameObject WhiteSwordmanButton;
    public GameObject BlackArcherButton;
    public GameObject ComeBackClientButton;
    public GameObject TutorialTexts;
    public GameObject TutorialImages;

    public void TutorialProgressTrue(int number)
    {
        TutorialTexts.transform.GetChild(number + 1).gameObject.SetActive(true);
        TutorialImages.transform.GetChild(number).gameObject.SetActive(true);
    }

    public void TutorialProgressFalse(int number)
    {
        TutorialTexts.transform.GetChild(number + 1).gameObject.SetActive(false);
        TutorialImages.transform.GetChild(number).gameObject.SetActive(false);
    }

    public void ClickTutorialsButton()
    {
        switch (Count)
        {
            case 1: 
                createDefenser.CreateSoldier(0, 0);
                GameManager.instance.Gold -= 5;
                UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
                Count += 1;
                break;
            case 2:
                createDefenser.CreateSoldier(0, 0);
                GameManager.instance.Gold -= 5;
                UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
                TutorialProgressFalse(0);
                TutorialProgressTrue(1);
                this.transform.position = UnitManageButton.transform.position;
                Count += 1;
                break;
            case 3:
                unitManage.FirstChilk();
                this.transform.position = SwordmansManageButton.transform.position;
                TutorialProgressFalse(1);
                TutorialProgressTrue(2);
                Count += 1;
                break;
            case 4:
                unitManage.ChlikSwordmanButton();
                this.transform.position = RedSwordmanManageButton.transform.position;
                TutorialProgressFalse(2);
                TutorialProgressTrue(3);
                Count += 1;
                break;
            case 5:
                unitManage.ChlikRedSwordmanButton();
                this.transform.position = CombineRedArcherButton.transform.position;
                TutorialProgressFalse(3);
                TutorialProgressTrue(4);
                Count += 1;
                break;
            case 6:
                combine.CombineRedArcher();
                this.transform.position = EnterStoryModeButton.transform.position;
                TutorialProgressFalse(4);
                TutorialProgressTrue(5);
                Count += 1;
                break;
            case 7:
                storyMode.EnterStoryMode();
                this.transform.position = WhiteSwordmanButton.transform.position;
                TutorialProgressFalse(5);
                TutorialProgressTrue(6);
                Count += 1;
                break;
            case 8:
                whiteTowerEvent.ClickWhiteSwordmanButton();
                TutorialProgressFalse(6);
                TutorialProgressTrue(7);
                Count += 1;
                this.transform.position = BlackArcherButton.transform.position;
                break;
            case 9:
                TutorialProgressFalse(7);
                TutorialProgressTrue(8);
                blackTowerEvent.ClickBlackArcherButton();
                this.transform.position = EnterStoryModeButton.transform.position;
                Count += 1;
                break;
            case 10:
                storyMode.EnterStoryMode();
                this.transform.position = ComeBackClientButton.transform.position;
                ComeBackClientButton.gameObject.SetActive(true);
                TutorialProgressFalse(8);
                TutorialProgressTrue(9);
                Count += 1;
                break;
            case 11:
                SceneManager.LoadScene("클라이언트");
                Count += 1;
                break;



        }
    }

    
}
