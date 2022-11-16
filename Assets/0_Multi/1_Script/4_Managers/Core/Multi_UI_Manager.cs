using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public enum PopupGroupType
{
    None,
    UnitWindow,
}

public class Multi_UI_Manager
{
    int _order = 10; // 기본 UI랑 팝업 UI 오더 다르게 하기 위해 초기값 10으로 세팅

    Stack<Multi_UI_Popup> _currentPopupStack = new Stack<Multi_UI_Popup>();
    Multi_UI_Base _sceneUI = null;

    Dictionary<string, Multi_UI_Popup> _nameByPopupCash = new Dictionary<string, Multi_UI_Popup>();
    Dictionary<PopupGroupType, Multi_UI_Popup> _groupTypeByCurrentPopup = new Dictionary<PopupGroupType, Multi_UI_Popup>();

    public void Init()
    {
        foreach (PopupGroupType type in Enum.GetValues(typeof(PopupGroupType)))
        {
            if (type == PopupGroupType.None) continue;
            _groupTypeByCurrentPopup.Add(type, null);
        }
    }

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
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; // canvas안의 canvas가 부모 관계없이 독립적인 sort값을 가지게 하는 옵션
        go.GetOrAddComponent<GraphicRaycaster>();

        CanvasScaler canvasScaler = go.GetOrAddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(800, 480);
        //canvasScaler.matchWidthOrHeight = 1;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

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
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = go.transform.position;
        return go.GetOrAddComponent<T>();
    }

    public T ShowSceneUI<T>(string name = null) where T : Multi_UI_Scene
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject go = Multi_Managers.Resources.Instantiate($"UI/Scene/{name}");
        T sceneUI = go.GetOrAddComponent<T>();
        _sceneUI = sceneUI;
        go.transform.SetParent(Root);
        return sceneUI;
    }

    public T ShowPopGroupUI<T>(PopupGroupType type, string name = null) where T : Multi_UI_Popup
    {
        if (_groupTypeByCurrentPopup[type] != null)
            ClosePopupUI();
        T popup = ShowPopupUI<T>(name);
        _groupTypeByCurrentPopup[type] = popup;
        return popup;
    }

    public T ShowPopupUI<T>(string name = null) where T : Multi_UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        string path = $"UI/Popup/{name}";
        if (_nameByPopupCash.TryGetValue(path, out Multi_UI_Popup popupCash))
        {
            ActivePopupUI(popupCash);
            return popupCash.gameObject.GetComponent<T>();
        }
        // 캐쉬가 없으면
        T popup = Multi_Managers.Resources.Instantiate(path).GetOrAddComponent<T>();
        _nameByPopupCash.Add(path, popup);
        ActivePopupUI(popup);

        return popup;
    }

    void ActivePopupUI(Multi_UI_Popup popup)
    {
        popup.gameObject.GetOrAddComponent<Canvas>().sortingOrder = _order;
        popup.transform.SetParent(Root);
        _order++;

        popup.gameObject.SetActive(true);
        _currentPopupStack.Push(popup);
    }

    public void ClosePopupUI() => _currentPopupStack.Pop().gameObject.SetActive(false);

    public void ClosePopupUI(PopupGroupType groupType)
    {
        ClosePopupUI();
        _groupTypeByCurrentPopup[groupType] = null;
    }

    public void CloseAllPopupUI()
    {
        foreach (PopupGroupType type in Enum.GetValues(typeof(PopupGroupType)))
        {
            if (type == PopupGroupType.None) continue;
            _groupTypeByCurrentPopup[type] = null;
        }
        _currentPopupStack.ToList().ForEach(x => x.gameObject.SetActive(false));
        _currentPopupStack.Clear();
    }

    public void Clear()
    {
        _sceneUI = null;
        _currentPopupStack.Clear();
        _nameByPopupCash.Clear();
        if (_root != null)
        {
            _root = null;
        }
    }

    public void ShowWaringText(string msg) => Multi_Managers.UI.ShowPopupUI<WarningText>().Show(msg);
    public void ShowClickRockWaringText(string msg) => Multi_Managers.UI.ShowPopupUI<WarningText>().ShowClickLockWaringText(msg);
}
