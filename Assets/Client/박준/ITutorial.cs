using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITutorial
{
    void TutorialAction();
    void TutorialEndAction();
    bool EndCurrentTutorialAction();
}
