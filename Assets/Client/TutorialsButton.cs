using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialsButton : MonoBehaviour
{
    
    public GameObject TutorialsText;
    int Count = 1;
    public void TutoriasDEF()
    {
        TutorialsText.transform.GetChild(Count).gameObject.SetActive(false);
        Count += 1;
        TutorialsText.transform.GetChild(Count).gameObject.SetActive(true);
    }

    public CreateDefenser createDefenser;
    public void ClickTutorialSommonButton()
    {
        if (GameManager.instance.Gold >= 5 && Count == 1)
        {
            createDefenser.CreateSoldier(0, 0);
            GameManager.instance.Gold -= 5;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
            TutoriasDEF();
            
        }
        
    }

    public UnitManageButton unitManageButton;
    public void clickTutorialUnitManageButton()
    {
        if (Count == 2 || Count == 6)
        {
            TutoriasDEF();
            unitManageButton.FirstChilk();
        }
    }

    public void ClickSwordmanButton()
    {
        if (Count == 3)
        {
            TutoriasDEF();
            unitManageButton.ChlikSwordmanButton();
        }
    }

    public void ClickRedSwordmanButton()
    {
        if (Count == 4)
        {
            TutoriasDEF();
            unitManageButton.ChlikRedSwordmanButton();
        }
    }

    public CombineSoldier combineSoldier;
    public void ClickTutorialCombineRedArcherButton()
    {
        if (Count == 5)
        {
            TutoriasDEF();
            combineSoldier.CombineRedArcher();
        }
        
    }

    public void ClickArcherutton()
    {
        if (Count == 7)
        {
            TutoriasDEF();
            unitManageButton.ChlikArcherButton();
        }
    }

    public void ClickRedArcherButton()
    {
        if (Count == 8)
        {
            TutoriasDEF();
            unitManageButton.ChlikRedArcherButton();
        }
    }

    public ButtonDown buttonDown;
    
    public void ClickTutorialXButton()
    {
        if(Count == 9)
        {
            TutoriasDEF();

            buttonDown.AllButtonDown();
        }
    }

    public StoryMode storyMode;
    public GameObject ComeBackClientButton;
    public void ClickTutorialMove()
    {
        if (Count == 10 )
        {
            TutoriasDEF();

            storyMode.EnterStoryMode();
        }
        else if (Count == 13)
        {
            //Loding.LoadScene("클라이언트");
            TutoriasDEF();

            storyMode.EnterStoryMode();
            ComeBackClientButton.SetActive(true);
        }
        
    }

    public void ClickComeBackClientButton()
    {
        Loding.LoadScene("클라이언트");
    }

    public WhiteTowerEvent whiteTowerEvent;

    public void ClickTutorialWhiteSwordmanButton()
    {
        if (Count == 11)
        {
            TutoriasDEF();

            whiteTowerEvent.ClickWhiteSwordmanButton();
        }
    }

    public BlackTowerEvent blackTowerEvent;

    public void ClickTutorialBlackArcherButton()
    {
        if(Count == 12)
        {
            TutoriasDEF();

            blackTowerEvent.ClickBlackArcherButton();
        }
    }


}
