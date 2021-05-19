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
                this.transform.position = UnitManageButton.transform.position;
                Count += 1;
                break;
            case 3:
                unitManage.FirstChilk();
                this.transform.position = SwordmansManageButton.transform.position;
                Count += 1;
                break;
            case 4:
                unitManage.ChlikSwordmanButton();
                this.transform.position = RedSwordmanManageButton.transform.position;
                Count += 1;
                break;
            case 5:
                unitManage.ChlikRedSwordmanButton();
                this.transform.position = CombineRedArcherButton.transform.position;
                Count += 1;
                break;
            case 6:
                combine.CombineRedArcher();
                this.transform.position = EnterStoryModeButton.transform.position;
                Count += 1;
                break;
            case 7:
                storyMode.EnterStoryMode();
                this.transform.position = WhiteSwordmanButton.transform.position;
                Count += 1;
                break;
            case 8:
                whiteTowerEvent.ClickWhiteSwordmanButton();
                Count += 1;
                this.transform.position = BlackArcherButton.transform.position;
                break;
            case 9:
                blackTowerEvent.ClickBlackArcherButton();
                this.transform.position = EnterStoryModeButton.transform.position;
                Count += 1;
                break;
            case 10:
                storyMode.EnterStoryMode();
                this.transform.position = ComeBackClientButton.transform.position;
                ComeBackClientButton.gameObject.SetActive(true);
                Count += 1;
                break;
            case 11:
                SceneManager.LoadScene("클라이언트");
                Count += 1;
                break;



        }
    }

    
}
