using UnityEngine;
using UnityEngine.UI;

public class ClickTutorialButton : MonoBehaviour, ITutorial
{
    [SerializeField] TutorialFuntions tutorFuntions = null;
    [SerializeField] RectTransform showUITransform = null;
    [SerializeField] Button tutorialButton = null;
    [SerializeField] bool UI_TutorialEnd = false;

    bool isTutorialStart = false;

    public void SetTutorialUI(RectTransform rect)
    {
        showUITransform = rect;

        if (rect.GetComponent<Button>() != null) tutorialButton = rect.GetComponent<Button>();
        else if (rect.GetComponentInChildren<Button>() != null) tutorialButton = rect.GetComponentInChildren<Button>();
    }

    public void TutorialStart()
    {
        isTutorialStart = true;
    }

    public bool EndCurrentTutorialAction()
    {
        return isTutorialStart;
    }

    public void TutorialAction()
    {
        Button[] allbutton = FindObjectsOfType<Button>();
        // 튜토리얼에 이용하는 버튼만 활성화
        for (int i = 0; i < allbutton.Length; i++)
        {
            if (allbutton[i] == tutorialButton) allbutton[i].enabled = true;
            else allbutton[i].enabled = false;
        }

        if(tutorialButton != null) tutorialButton.onClick.AddListener( () => TutorialStart());
        if (showUITransform != null) tutorFuntions.SetBlindUI(showUITransform);
    }

    [SerializeField] bool isGameProgress;

    private void OnDisable()
    {
        if (isGameProgress) tutorFuntions.GameProgress();
        else if (UI_TutorialEnd) tutorFuntions.Reset_FocusUI();
    }
}
