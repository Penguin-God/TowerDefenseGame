using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace TutorialUseCases
{
    public class TutorialComposite : ITutorial
    {
        List<ITutorial> _commends = new List<ITutorial>();
        public void AddCommend(ITutorial tutorial) => _commends.Add(tutorial);
        public bool EndCondition() => _commends.All(x => x.EndCondition());

        public void TutorialAction() => _commends.ForEach(x => x.TutorialAction());
    }

    public class ReadTextCommend : ITutorial
    {
        string _text;
        public ReadTextCommend(string text) => _text = text;

        public void TutorialAction() => Managers.UI.ShowPopupUI<TutorialText>().Setup(_text);
        public void EndAction() => Managers.UI.ClosePopupUI();
        public bool EndCondition() => Input.GetMouseButtonUp(0);
    }

    public class SpotLightCommend
    {
        Vector3 spotPos;
        Light _light;
        public SpotLightCommend(Vector3 lightPos) => spotPos = lightPos;

        public void TutorialAction()
        {
            _light = Resources.Load<Light>("Tutorial/SpotLight");
            _light.gameObject.SetActive(true);
            _light.transform.position = spotPos;
        }
        public void EndAction() => Object.Destroy(_light.gameObject);
        public bool EndCondition() => Input.GetMouseButtonUp(0);
    }

    public class Highlight_UI : ITutorial
    {
        string _uiName;
        public Highlight_UI(string uiName) => _uiName = uiName;

        public void TutorialAction()
        {
            var showUITransform = GameObject.Find(_uiName).GetComponent<RectTransform>();
            //tutorFuntions.SetAllButton(false);
            //if (showUITransform != null) tutorFuntions.SetBlindUI(showUITransform);
        }
        // public void EndAction() => tutorFuntions.Reset_FocusUI();
        public bool EndCondition() => Input.GetMouseButtonUp(0);
    }

    public class ButtonClickCommend : ITutorial
    {
        string _uiName;
        public ButtonClickCommend(string uiName) => _uiName = uiName;
        bool _isDone = false;
        public void TutorialAction()
        {
            var targetButton = GameObject.Find(_uiName).GetComponent<Button>();
            targetButton.onClick.AddListener(() => _isDone = true);
        }
        public bool EndCondition() => _isDone;
    }
}