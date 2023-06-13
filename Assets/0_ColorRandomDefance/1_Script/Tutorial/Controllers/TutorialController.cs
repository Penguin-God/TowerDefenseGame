using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TutorialCommends;
using System;
using System.Linq;

public abstract class TutorialController : MonoBehaviour
{
    protected TutorialFuntions tutorialFuntions;

    void Start()
    {
        tutorialFuntions = FindObjectOfType<TutorialFuntions>();
        AddTutorials();
        StartCoroutine(Co_WaitCondition());
        Init();
    }
    protected abstract bool TutorialStartCondition();
    protected abstract void AddTutorials();
    protected virtual void Init() { }
    IEnumerator Co_WaitCondition()
    {
        yield return new WaitUntil(() => TutorialStartCondition());
        tutorialFuntions.OffLigth();
        StartCoroutine(Co_DoTutorials());
    }

    List<ITutorial> tutorialCommends = new List<ITutorial>();
    IEnumerator Co_DoTutorials()
    {
        yield return StartCoroutine(Co_DoTutorial(tutorialCommends.First(), 1f));
     
        foreach (var tutorial in tutorialCommends.Skip(1))
            yield return StartCoroutine(Co_DoTutorial(tutorial));
        // 모든 튜토리얼이 끝나면 게임 진행
        tutorialFuntions.GameProgress();
    }

    IEnumerator Co_DoTutorial(ITutorial tutorial, float delayTime = 0.1f)
    {
        var buttons = GameObject.FindObjectsOfType<Button>();
        SetEnabledAllButton(buttons, false);
        tutorial.TutorialAction();
        yield return new WaitForSecondsRealtime(delayTime);
        yield return new WaitUntil(() => tutorial.EndCondition());
        tutorial.EndAction();
        SetEnabledAllButton(buttons, true);

        yield return 1;
    }

    void SetEnabledAllButton(Button[] buttons, bool isActive)
    {
        foreach (var button in buttons.Where(x => x != null))
            button.enabled = isActive;
    }

    // 샌드박스 함수들
    protected void AddCommend(ITutorial tutorial) => tutorialCommends.Add(tutorial);
    void AddCompositeCommend(string text, ITutorial commend)
    {
        var compositeCommend = new TutorialComposite();
        compositeCommend.AddCommend(new ReadTextCommend(text));
        compositeCommend.AddCommend(commend);
        tutorialCommends.Add(compositeCommend);
    }
    protected void AddReadCommend(string text) => tutorialCommends.Add(new ReadTextCommend(text));

    protected void AddUnitHighLightCommend(string text, UnitClass unitClass)
        => AddUnitHighLightCommend(text, () => Managers.Unit.FindUnit((unit) => unit.UnitClass == unitClass).transform.position + new Vector3(0, 5, 0));

    protected void AddUnitHighLightCommend(string text, UnitFlags unitFlag, Func<bool> endCondition = null)
        => AddCompositeCommend(text, new SpotLightActionCommend(() => Managers.Unit.FindUnit(unitFlag).transform.position + new Vector3(0, 5, 0), 10f, endCondition));

    protected void AddUnitHighLightCommend(string text, Func<Vector3> getPos) => AddCompositeCommend(text, new SpotLightActionCommend(getPos));

    protected void AddObjectHighLightCommend(string text, Vector3 pos, float range = 10f) => AddCompositeCommend(text, new SpotLightCommend(pos, range));

    protected void AddUI_HighLightCommend(string text, string uiName) => AddCompositeCommend(text, new Highlight_UICommend(uiName));

    protected void AddClickCommend(string text, string uiName)
    {
        var clickCommend = new TutorialComposite();
        clickCommend.AddCommend(new Highlight_UICommend(uiName));
        clickCommend.AddCommend(new ButtonClickCommend(uiName));
        AddCompositeCommend(text, clickCommend);
    }

    protected void AddActionCommend(Action tutorialAction, Func<bool> endCondtion = null, Action endActoin = null)
        => tutorialCommends.Add(new ActionCommend(tutorialAction, endCondtion, endActoin));

    protected void AddTargetUINameToIndexNameActionCommend<T>(Func<T, bool> condition, string indexName) where T : UnityEngine.Object
        => AddActionCommend
        (() => FindObjectsOfType<T>() 
        .Where(condition)
        .First()
        .name = indexName);
}
