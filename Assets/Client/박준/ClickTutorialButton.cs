﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickTutorialButton : MonoBehaviour, ITutorial
{
    [SerializeField] TutorialFuntions tutorFuntions = null;
    [SerializeField] RectTransform rectTransform = null;
    [SerializeField] GameObject BlindUI = null;
    [SerializeField] Button tutorialButton = null;

    bool isTutorialStart = false;

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
        BlindUI.SetActive(true);
        Button[] allbutton = FindObjectsOfType<Button>();
        for (int i = 0; i < allbutton.Length; i++)
        {
            if (allbutton[i] == tutorialButton) allbutton[i].enabled = true;
            else allbutton[i].enabled = false;
        }
        tutorialButton.onClick.AddListener( () => TutorialStart());

        if (rectTransform != null) tutorFuntions.SetBlindUI(rectTransform);
    }

    [SerializeField] bool isGameProgress;

    private void OnDisable()
    {
        BlindUI.SetActive(false);

        if (isGameProgress) tutorFuntions.GameProgress();
    }
}
