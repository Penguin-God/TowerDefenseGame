using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Multi_UI_Manager
{
    Dictionary<string, Multi_UI_Popup> _popupByName = new Dictionary<string, Multi_UI_Popup>();

    int _order = 10; // 기본 UI랑 팝업 UI 오더 다르게 하기 위해 초기값 10으로 세팅

    Stack<Multi_UI_Popup> _popupStack = new Stack<Multi_UI_Popup>();
    Multi_UI_Base _sceneUI = null;

    Transform _root;
    public Transform Root
    {
        get
        {
            if (_root == null) _root = new GameObject("@UI_Root").transform;
            return _root;
        }
    }

    public void SetCanvas(GameObject go, bool sort)
    {
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        GraphicRaycaster raycaster = go.GetOrAddComponent<GraphicRaycaster>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; // canvas안의 canvas가 부모 관계없이 독립적인 sort값을 가지게 하는 옵션

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : Multi_UI_Base
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject go = Multi_Managers.Resources.Instantiate($"UI/SubItem/{name}");
        if (parent != null) go.transform.SetParent(parent);
        return go.GetOrAddComponent<T>();
    }

    public T ShowSceneUI<T>(string name = null) where T : Multi_UI_Base
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject go = Multi_Managers.Resources.Instantiate($"UI/Scene/{name}");
        T sceneUI = go.GetOrAddComponent<T>();
        _sceneUI = sceneUI;
        go.transform.SetParent(Root);
        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : Multi_UI_Popup
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        if (_popupByName.TryGetValue(name, out Multi_UI_Popup dictPopup))
        {
            dictPopup.gameObject.SetActive(true);
            return dictPopup.gameObject.GetComponent<T>();
        }

        GameObject go = Multi_Managers.Resources.Instantiate($"UI/Popup/{name}");
        T popup = go.GetOrAddComponent<T>();
        _popupStack.Push(popup);
        _popupByName.Add(name, go.GetComponent<Multi_UI_Popup>());
        go.transform.SetParent(Root);
        return popup;
    }

    public void ClosePopupUI(Multi_UI_Popup popup)
    {
        if (_popupStack.Count == 0) return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("팝업창 닫는거 실패!!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0) return;

        Multi_UI_Popup popup = _popupStack.Pop();
        // TODO 나중에 구현하기
        //_popupByName.Remove(popup);
        Multi_Managers.Resources.Destroy(popup.gameObject);
        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    public void Clear()
    {
        _popupStack.Clear();
        _sceneUI = null;
    }
}
