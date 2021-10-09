using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCombine : MonoBehaviour, ITutorailActionTrigger
{
    [SerializeField] TutorialManager tutorialManager;
    private void OnEnable()
    {
        StartCoroutine(Co_Action());
    }

    IEnumerator Co_Action()
    {
        yield return new WaitUntil(() => TutorialAction_Condition());

        transform.GetChild(0).gameObject.SetActive(true);
        tutorialManager.TutorialStart(transform);
    }

    public bool TutorialAction_Condition()
    {
        GameObject[] reds = GameObject.FindGameObjectsWithTag("RedSwordman");
        GameObject[] blues = GameObject.FindGameObjectsWithTag("BlueSwordman");
        GameObject[] yellows = GameObject.FindGameObjectsWithTag("YellowSwordman");
        bool condition = (reds.Length >= 3 || blues.Length >= 3 || yellows.Length >= 3);
        return condition;
    }
}