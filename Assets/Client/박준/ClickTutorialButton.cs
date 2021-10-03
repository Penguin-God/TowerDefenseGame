using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickTutorialButton : MonoBehaviour, ITutorial
{
    [SerializeField] GameObject BlindUI;
    [SerializeField] Button tutorialButton = null;

    public bool EndCurrentTutorialAction()
    {
        return EventSystem.current.currentSelectedGameObject == tutorialButton.gameObject && Input.GetMouseButtonUp(0);
    }

    public void TutorialAction()
    {
        Button[] allbutton = FindObjectsOfType<Button>();
        for(int i = 0; i < allbutton.Length; i++)
        {
            if (allbutton[i] == tutorialButton) tutorialButton.enabled = true;
        }
    }

    public void TutorialEndAction()
    {
        BlindUI.SetActive(false);
    }
}
