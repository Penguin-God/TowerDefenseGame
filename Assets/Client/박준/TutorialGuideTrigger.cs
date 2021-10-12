using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGuideTrigger : MonoBehaviour
{
    [SerializeField] TutorialManager tutorialManager;

    private void OnEnable()
    {
        StartCoroutine(Co_TutorialStart());
    }

    IEnumerator Co_TutorialStart()
    {
        yield return new WaitUntil(() => TutorialCondition());
        tutorialManager.TutorialStart(transform);
    }

    public virtual bool TutorialCondition()
    {
        return true;
    }
}
