using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum PopupGroupType
{
    Single,
    UnitWindow,
}

class PopupGroup
{
    Multi_UI_Popup currentPopup = null;

    public void ShowPopup(Multi_UI_Popup popup)
    {
        OffCurrentPopup();
        currentPopup = popup;
        popup.gameObject.SetActive(true);
    }

    public void ClosePopupUI<T>()
    {
        OffCurrentPopup();
        currentPopup = null;
    }

    public void OffCurrentPopup()
    {
        if (currentPopup != null)
            currentPopup.gameObject.SetActive(false);
    }
}

public class Multi_UI_Manager
{
    Dictionary<string, Multi_UI_Popup> _popupByType = new Dictionary<string, Multi_UI_Popup>();

    Dictionary<PopupGroupType, PopupGroup> popupGroupByGroupType = new Dictionary<PopupGroupType, PopupGroup>();
    int _order = 10; // 기본 UI랑 팝업 UI 오더 다르게 하기 위해 초기값 10으로 세팅

    // Stack<Multi_UI_Popup> _popupStack = new Stack<Multi_UI_Popup>();
    Multi_UI_Base _sceneUI = null;

    public void Init()
    {
        foreach (PopupGroupType type in Enum.GetValues(typeof(PopupGroupType)))
            popupGroupByGroupType.Add(type, new PopupGroup());
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

    public T ShowPopupUI<T>(string name = null, PopupGroupType type = PopupGroupType.Single) where T : Multi_UI_Popup
    {
        if (_popupByType.TryGetValue(name, out Multi_UI_Popup dictPopup))
        {
            ShowPopupUI(dictPopup, type);
            return dictPopup.gameObject.GetComponent<T>();
        }

        T popup = Multi_Managers.Resources.Instantiate($"UI/Popup/{name}").GetOrAddComponent<T>();
        _popupByType.Add(name, popup);
        popup.transform.SetParent(Root);
        ShowPopupUI(popup, type);
        return popup;
    }

    void ShowPopupUI(Multi_UI_Popup popup, PopupGroupType type)
    {
        if (type == PopupGroupType.Single)
            popup.gameObject.SetActive(true);
        else
            popupGroupByGroupType[type].ShowPopup(popup);
    }

    public void CloseAllPopupUI()
    {
        foreach (PopupGroup group in popupGroupByGroupType.Values)
            group.OffCurrentPopup();
    }

    public void Clear()
    {
        _sceneUI = null;
    }
}
