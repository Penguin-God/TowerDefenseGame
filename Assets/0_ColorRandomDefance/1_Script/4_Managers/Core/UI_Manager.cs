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

public class UI_Manager
{
    int _order = 10; // 기본 UI랑 팝업 UI 오더 다르게 하기 위해 초기값 10으로 세팅

    Stack<UI_Popup> _currentPopupStack = new Stack<UI_Popup>();
    public int PopupCount => _currentPopupStack.Count;

    Dictionary<string, GameObject> _popupCashByPath = new Dictionary<string, GameObject>();
    Dictionary<PopupGroupType, UI_Popup> _groupTypeByCurrentPopup = new Dictionary<PopupGroupType, UI_Popup>();

    public readonly float UIScreenWidth = 800;
    public readonly float UIScreenHeight = 480;

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
        canvasScaler.referenceResolution = new Vector2(UIScreenWidth, UIScreenHeight);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

        if(sort)
            SetSotingOrder(canvas);
    }

    void SetSotingOrder(Canvas canvas)
    {
        canvas.sortingOrder = _order;
        _order++;
    }

    public T MakeSubItem<T>(Transform parent, string name = null) where T : UI_Base
    {
        T ui = ShowUI<T>("SubItem", name, null, parent);
        ui.transform.localScale = Vector3.one;
        ui.transform.localPosition = ui.transform.position;
        return ui;
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene => ShowUI<T>("Scene", name);

    public T ShowPopGroupUI<T>(PopupGroupType type, string name = null) where T : UI_Popup
    {
        if (_groupTypeByCurrentPopup[type] != null)
            ClosePopupUI();
        T popup = ShowPopupUI<T>(name);
        _groupTypeByCurrentPopup[type] = popup;
        return popup;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        T popup = ShowUI<T>("Popup", name, InstantPopupUI);
        _currentPopupStack.Push(popup);
        SetSotingOrder(popup.gameObject.GetOrAddComponent<Canvas>());
        return popup;
    }

    GameObject InstantPopupUI(string path)
    {
        if (_popupCashByPath.TryGetValue(path, out GameObject popupCash))
            return popupCash;
        else
        {
            var go = Managers.Resources.Instantiate(path);
            _popupCashByPath.Add(path, go);
            return go;
        }
    }

    public T ShowDefualtUI<T>(string name = null) where T : UI_Base => ShowUI<T>("Default", name);

    T ShowUI<T>(string uiType, string name = null, Func<string, GameObject> getObject = null, Transform parent = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;
        GameObject go = getObject == null ? Managers.Resources.Instantiate($"UI/{uiType}/{name}") : getObject($"UI/{uiType}/{name}");
        T ui = go.GetOrAddComponent<T>();
        go.transform.SetParent(parent ?? Root);
        go.gameObject.SetActive(true);
        return ui;
    }


    public void ClosePopupUI() => _currentPopupStack.Pop().gameObject.SetActive(false);

    public void ClosePopupUI(PopupGroupType groupType)
    {
        if (_groupTypeByCurrentPopup[groupType] == null) return;
        _groupTypeByCurrentPopup[groupType] = null;
        ClosePopupUI();
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

    public UI_Popup PeekPopupUI()
    {
        if (_currentPopupStack.Count == 0) return null;
        return _currentPopupStack.Peek();
    }

    public void Clear()
    {
        _root = null;
        _currentPopupStack.Clear();
        _popupCashByPath.Clear();
        _groupTypeByCurrentPopup.Clear();
    }
}
