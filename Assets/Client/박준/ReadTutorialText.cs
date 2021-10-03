using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadTutorialText : MonoBehaviour, ITutorial
{
    [SerializeField] TutorialManager tutorManager;

    public bool EndCurrentTutorialAction()
    {
        return Input.GetMouseButtonDown(0);
    }

    public void TutorialAction()
    {
        
    }

    public void TutorialEndAction()
    {

    }
}
