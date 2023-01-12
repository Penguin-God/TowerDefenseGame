using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TutorialUseCases;
using System;

public abstract class TutorialController : MonoBehaviour
{
    protected TutorialFuntions tutorialFuntions;
    void Start()
    {
        tutorialFuntions = FindObjectOfType<TutorialFuntions>();
        AddTutorials();
        StartCoroutine(Co_WaitCondition());
    }
    protected virtual bool TutorialStartCondition() => true;
    protected abstract void AddTutorials();
    IEnumerator Co_WaitCondition()
    {
        yield return new WaitUntil(() => TutorialStartCondition());
        StartCoroutine(Co_DoTutorial());
        
    }

    List<ITutorial> tutorialCommends = new List<ITutorial>();
    IEnumerator Co_DoTutorial()
    {
        tutorialFuntions.OffLigth();
        yield return new WaitForSecondsRealtime(0.1f);

        foreach (var tutorial in tutorialCommends)
        {
            tutorial.TutorialAction();
            yield return new WaitUntil(() => tutorial.EndCondition());
            tutorial.EndAction();
            yield return new WaitForSecondsRealtime(0.1f); // 2번 넘어가서 대기
        }
        // 모든 튜토리얼이 끝나면 게임 진행
        tutorialFuntions.GameProgress();
    }

    protected TutorialComposite CreateComposite() => new TutorialComposite();
    protected ReadTextCommend CreateReadCommend(string text) => new ReadTextCommend(text);
    protected SpotLightCommend CreateSpotLightCommend(Vector3 pos) => new SpotLightCommend(pos);
    protected SpotLightActionCommend CreateSpotLightActionCommend(Func<Vector3> getPos) => new SpotLightActionCommend(getPos);
    protected Highlight_UI CreateUI_HighLightCommend(string uiName) => new Highlight_UI(uiName);
    protected ButtonClickCommend CreateClickCommend(string uiName) => new ButtonClickCommend(uiName);
    protected ActionCommend CreateActionCommend(Action tutorialAction, Func<bool> endCondtion = null, Action endActoin = null)
        => new ActionCommend(tutorialAction, endCondtion, endActoin);

    protected void AddCommend(ITutorial tutorial) => tutorialCommends.Add(tutorial);
    protected void AddReadCommend(string text) => tutorialCommends.Add(CreateReadCommend(text));

    protected void AddSpotLightActionCommend(string text, Func<Vector3> getPos)
    {
        var spotLightCommend = CreateComposite();
        spotLightCommend.AddCommend(CreateReadCommend(text));
        spotLightCommend.AddCommend(CreateSpotLightActionCommend(getPos));
        tutorialCommends.Add(spotLightCommend);
    }
    protected void AddSpotLightCommend(string text, Vector3 pos)
    {
        var spotLightCommend = CreateComposite();
        spotLightCommend.AddCommend(CreateReadCommend(text));
        spotLightCommend.AddCommend(CreateSpotLightCommend(pos));
        tutorialCommends.Add(spotLightCommend);
    }
   
    protected void AddUI_HighLightCommend(string text, string uiName)
    {
        var highLight_UICommend = CreateComposite();
        highLight_UICommend.AddCommend(CreateReadCommend(text));
        highLight_UICommend.AddCommend(CreateUI_HighLightCommend(uiName));
        tutorialCommends.Add(highLight_UICommend);
    }

    protected void AddClickCommend(string text, string uiName)
    {
        var clickCommend = CreateComposite();
        clickCommend.AddCommend(CreateReadCommend(text));
        clickCommend.AddCommend(CreateUI_HighLightCommend(uiName));
        clickCommend.AddCommend(CreateClickCommend(uiName));
        tutorialCommends.Add(clickCommend);
    }

    protected void AddActionCommend(Action tutorialAction, Func<bool> endCondtion = null, Action endActoin = null)
        => tutorialCommends.Add(CreateActionCommend(tutorialAction, endCondtion, endActoin));
}
