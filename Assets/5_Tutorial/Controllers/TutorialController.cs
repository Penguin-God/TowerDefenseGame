using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TutorialUseCases;

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

    List<ITutorial> tutorials = new List<ITutorial>();
    IEnumerator Co_DoTutorial()
    {
        tutorialFuntions.OffLigth();
        yield return new WaitForSecondsRealtime(0.1f);

        foreach (var tutorial in tutorials)
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
    protected Highlight_UI CreateUI_HighLightCommend(string uiName) => new Highlight_UI(uiName);
    protected ButtonClickCommend CreateClickCommend(string uiName) => new ButtonClickCommend(uiName);

    protected void AddCommend(ITutorial tutorial) => tutorials.Add(tutorial);
    protected void AddReadCommend(string text) => tutorials.Add(CreateReadCommend(text));

    protected void AddSpotLightCommend(string text, Vector3 pos)
    {
        var spotLightCommend = CreateComposite();
        spotLightCommend.AddCommend(CreateReadCommend(text));
        spotLightCommend.AddCommend(CreateSpotLightCommend(pos));
        tutorials.Add(spotLightCommend);
    }
   
    protected void AddUI_HighLightCommend(string text, string uiName)
    {
        var highLight_UICommend = CreateComposite();
        highLight_UICommend.AddCommend(CreateReadCommend(text));
        highLight_UICommend.AddCommend(CreateUI_HighLightCommend(uiName));
        tutorials.Add(highLight_UICommend);
    }

    protected void AddClickCommend(string text, string uiName)
    {
        var clickCommend = CreateComposite();
        clickCommend.AddCommend(CreateReadCommend(text));
        clickCommend.AddCommend(CreateUI_HighLightCommend(uiName));
        clickCommend.AddCommend(CreateClickCommend(uiName));
        tutorials.Add(clickCommend);
    }
}
