using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReadTutorialText : MonoBehaviour, ITutorial
{
    [SerializeField] string type = "";
    TutorialFuntions tutorFuntions;
    public bool EndCurrentTutorialAction()
    {
        return Input.GetMouseButtonUp(0);
    }

    public void TutorialAction()
    {
        tutorFuntions = FindObjectOfType<TutorialFuntions>();
        tutorFuntions.OffLigth();
        UnityEngine.Object ligthObj = null;
        if (type != "") ligthObj = FindObjectOfType(Type.GetType(type)); //  as GameObject 되는지 확인해보기

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
