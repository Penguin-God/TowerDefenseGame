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
        isLast = true;
        TutoriasDEF();

        createDefenser.CreateSoldier(6, 1);
        GameManager.instance.Gold -= 10;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        blackTowerEvent.BlackUiAudio.Play();
        blackTowerEvent.Hide_BuyBackGround();
    }

    // 그냥 스크립트하나 새로 만들어서 OnEnabled에 넣기
    public void SetButton(GameObject buttonObject) // 튜토리얼 전 오브젝트에 함수 넣고 인수는 후에 튜토리얼 진행할 오브젝트
    {
        Button button = buttonObject.GetComponent<Button>();
        button.enabled = true;
        button.onClick.AddListener(() => button.enabled = false);
        button.onClick.AddListener(() => TutoriasDEF());
    }

    public void ActiveButton(GameObject buttonObject)
    {
        Button button = buttonObject.GetComponent<Button>();
        button.enabled = true;
        button.onClick.AddListener(() => TutoriasDEF());
        button.onClick.AddListener(() => button.onClick.RemoveListener( () => TutoriasDEF()));
    }

    bool isLast = false;
    public void LastButton(GameObject backButton)
    {
        if (isLast) backButton.SetActive(true);
    }
}
