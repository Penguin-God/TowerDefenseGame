using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace TutorialUseCases
{
    public class TutorialComposite : ITutorial
    {
        List<ITutorial> _commends = new List<ITutorial>();
        public void AddCommend(ITutorial tutorial) => _commends.Add(tutorial);
        public void TutorialAction() => _commends.ForEach(x => x.TutorialAction());
        public bool EndCondition() => _commends.All(x => x.EndCondition());
        public void EndAction() => _commends.ForEach(x => x.EndAction());
    }

    public class ReadTextCommend : ITutorial
    {
        string _text;
        public ReadTextCommend(string text) => _text = text;

        public void TutorialAction() => Managers.UI.ShowPopupUI<TutorialText>().Setup(_text);
        public void EndAction() => Managers.UI.ClosePopupUI();
        public bool EndCondition() => Input.GetMouseButtonUp(0);
    }

    public class SpotLightCommend : ITutorial
    {
        protected Vector3 spotPos;
        Light _light;
        public SpotLightCommend(Vector3 lightPos) => spotPos = lightPos;

        public virtual void TutorialAction()
        {
            _light = UnityEngine.Object.Instantiate(Resources.Load<Light>("Tutorial/SpotLight"));
            _light.gameObject.SetActive(true);
            _light.transform.position = spotPos;
        }
        public void EndAction() => UnityEngine.Object.Destroy(_light.gameObject);
        public bool EndCondition() => Input.GetMouseButtonUp(0);
    }

    public class SpotLightActionCommend : SpotLightCommend
    {
        Func<Vector3> _getPos;
        public SpotLightActionCommend(Func<Vector3> getPos) : base(Vector3.zero) => _getPos = getPos;
        public override void TutorialAction()
        {
            spotPos = _getPos();
            base.TutorialAction();
        }
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

        public void EndAction() { }
    }

    public class ButtonClickCommend : ITutorial
    {
        string _uiName;
        public ButtonClickCommend(string uiName) => _uiName = uiName;

        Button button;
        bool _isDone = false;
        void End() => _isDone = true;

        public void TutorialAction()
        {
            button = GameObject.Find(_uiName).GetComponent<Button>();
            button.onClick.AddListener(End);
        }
        public bool EndCondition() => _isDone;
        public void EndAction() => button.onClick.RemoveListener(End);
    }

    public class ActionCommend : ITutorial
    {
        Action _tutorialAction;
        Func<bool> _endCondtion;
        Action _endActoin;
        public ActionCommend(Action tutorialAction, Func<bool> endCondtion = null, Action endActoin = null)
        {
            _tutorialAction = tutorialAction;
            _endCondtion = endCondtion;
            _endActoin = endActoin;
        }

        public void TutorialAction() => _tutorialAction?.Invoke();
        public bool EndCondition()
        {
            if (_endCondtion == null) return true;
            else return _endCondtion();
        }
        public void EndAction() => _endActoin?.Invoke();
    }
}