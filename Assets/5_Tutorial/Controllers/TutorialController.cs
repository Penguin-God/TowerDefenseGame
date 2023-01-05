using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialController : MonoBehaviour
{
    protected TutorialFuntions tutorFuntions;
    void Start()
    {
        tutorFuntions = FindObjectOfType<TutorialFuntions>();
        StartCoroutine(Co_DoTutorial());
    }
    protected virtual bool TutorialStartCondition() => true;
    protected abstract IEnumerable<ITutorial> DoTutorial();
    IEnumerator Co_DoTutorial()
    {
        yield return new WaitUntil(() => TutorialStartCondition());
        DoTutorial();
    }
}
