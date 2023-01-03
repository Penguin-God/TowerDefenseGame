using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReadTutorialText : MonoBehaviour, ITutorial
{
    [SerializeField] string type = "";
    [SerializeField] TutorialFuntions tutorFuntions;

    public bool EndCurrentTutorialAction()
    {
        return Input.GetMouseButtonUp(0);
    }

    public void TutorialAction()
    {
        tutorFuntions.OffLigth();
        UnityEngine.Object ligthObj = null;
        if (type != "") ligthObj = FindObjectOfType(Type.GetType(type));

        if (ligthObj != null)
        {
            GameObject ligthGameObj = GameObject.Find(ligthObj.name);
            tutorFuntions.Set_SpotLight(ligthGameObj.transform.position);
        }
    }

    [SerializeField] bool filedExplanationEnd = false;
    private void OnDisable()
    {
        if (filedExplanationEnd) tutorFuntions.OnLigth();
    }
}
