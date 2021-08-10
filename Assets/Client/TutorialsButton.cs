using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialsButton : MonoBehaviour
{
    
    public GameObject TutorialsText;
    int Count = 1;
    int SommonCount = 0;
    public TutorialArrows tutorialArrows;

    [SerializeField] GameObject obj_tutorialButton;
    public void ButtonTutoriasDEF()
    {
        TutorialsText.transform.GetChild(Count).gameObject.SetActive(false);
        Count += 1;
        TutorialsText.transform.GetChild(Count).gameObject.SetActive(true);
        tutorialArrows.ArrowStart(1);
        if (Count >= 3)
        {
            tutorialArrows.ArrowStop(1);
            obj_tutorialButton.SetActive(false);
        }
    }

    public void TutoriasDEF()
    {
        if (Count == 1)
        {
            GameManager.instance.Gold += 10;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
        }
        TutorialsText.transform.GetChild(Count).gameObject.SetActive(false);
        Count += 1;
        TutorialsText.transform.GetChild(Count).gameObject.SetActive(true);
        tutorialArrows.ArrowStart(1);
        //if (Count == 12 || Count == 13)
        //{
        //    tutorialArrows.ArrowStart(2);
        //}
        //else
        //{
        //    tutorialArrows.ArrowStart(1);
        //}

    }

    public CreateDefenser createDefenser;
    [SerializeField] GameObject paintButton;
    public void ClickTutorialSommonButton()
    {
        //tutorialArrows.ArrowStop(1);
        if (GameManager.instance.Gold >= 5 && Count == 3)
        {
            createDefenser.CreateSoldier(0, 0);
            GameManager.instance.Gold -= 5;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
            SommonCount += 1;
            if (SommonCount == 3)
            {
                TutoriasDEF();
                SetButton(paintButton);
            }
            
        }
        
    }

    public UnitManageButton unitManageButton;
    public void clickTutorialUnitManageButton()
    {
        if (Count == 4 || Count == 8)
        {
            TutoriasDEF();
            unitManageButton.FirstChilk();
        }
    }

    public void ClickSwordmanButton()
    {
        if (Count == 5)
        {
            TutoriasDEF();
            unitManageButton.ChlikSwordmanButton();
        }
    }

    public void ClickRedSwordmanButton()
    {
        if (Count == 6)
        {
            TutoriasDEF();
            unitManageButton.ChlikRedSwordmanButton();
        }
    }

    public CombineSoldier combineSoldier;
    public void ClickTutorialCombineRedArcherButton()
    {
        if (Count == 7)
        {
            TutoriasDEF();
            combineSoldier.CombineRedArcher();
        }
        
    }

    public void ClickArcherutton()
    {
        if (Count == 9)
        {
            TutoriasDEF();
            unitManageButton.ChlikArcherButton();
        }
    }

    public void ClickRedArcherButton()
    {
        if (Count == 10)
        {
            TutoriasDEF();
            unitManageButton.ChlikRedArcherButton();
        }
    }

    public ButtonDown buttonDown;
    
    public void ClickTutorialXButton()
    {
        if(Count == 11)
        {
            TutoriasDEF();

            buttonDown.AllButtonDown();
        }
    }

    public StoryMode storyMode;
    public GameObject ComeBackClientButton;
    public void ClickTutorialMove()
    {
        if (Count == 12 )
        {
            TutoriasDEF();

            storyMode.EnterStoryMode();
        }
        else if (Count == 15)
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
        TutoriasDEF();
        whiteTowerEvent.ClickWhiteSwordmanButton();
    }

    public BlackTowerEvent blackTowerEvent;

    public void ClickTutorialBlackArcherButton()
    {
         TutoriasDEF();
         blackTowerEvent.ClickBlackArcherButton();
    }

    public void SetButton(GameObject buttonObject)
    {
        buttonObject.GetComponent<Button>().enabled = true;
    }
}
