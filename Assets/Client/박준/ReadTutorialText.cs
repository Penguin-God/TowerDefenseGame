﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReadTutorialText : MonoBehaviour, ITutorial
{
    [SerializeField] string type = "";
    [SerializeField] TutorialFuntions tutorFuntions;

    public bool EndCurrentTutorialAction()
    {
        return Input.GetMouseButtonDown(0);
    }

    public void TutorialAction()
    {
        UnityEngine.Object ligthObj = null;
        if (type != "") ligthObj = FindObjectOfType(Type.GetType(type));

        if (ligthObj != null)
        {
            tutorFuntions.OffLigth();
            GameObject ligthGameObj = GameObject.Find(ligthObj.name);
            tutorFuntions.Set_SpotLight(ligthGameObj.transform.position);
        }
    }

    [SerializeField] bool filedExplanationEnd = false;
    [SerializeField] bool isGameProgress = false;
    private void OnDisable()
    {
        if (filedExplanationEnd) tutorFuntions.OnLigth();
        if (isGameProgress) tutorFuntions.GameProgress();
    }
}
