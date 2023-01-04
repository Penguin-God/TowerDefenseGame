using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상속받는 스크립트는 TutorialCondition()만 구현하면 됨
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