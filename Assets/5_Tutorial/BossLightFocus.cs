using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLightFocus : MonoBehaviour, ITutorial
{
    [SerializeField] string tagName = "";
    [SerializeField] TutorialFuntions tutorFuntions;

    public bool EndCurrentTutorialAction()
    {
        return Input.GetMouseButtonUp(0);
    }

    public void TutorialAction()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(tagName);
        tutorFuntions.OffLigth();
        if (obj != null) tutorFuntions.Set_SpotLight(obj.transform.position);
    }
}
