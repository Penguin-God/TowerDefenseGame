using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class TutorialManager : MonoBehaviour
{
    private TutorialFuntions tutorialFuntions = null;
    void Awake()
    {
        tutorialFuntions = GetComponent<TutorialFuntions>();
        // gameObject.AddComponent<Tutorial_Basic>();
        gameObject.AddComponent<Tutorial_OtherPlayer>();
        gameObject.AddComponent<Tutorial_Tower>();
        gameObject.AddComponent<Tutorial_Boss>();
    }
}
