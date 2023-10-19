using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class UI_Manager
{
    int _order = 10; // 기본 UI랑 팝업 UI 오더 다르게 하기 위해 초기값 10으로 세팅

    Stack<UI_Popup> _currentPopupStack = new Stack<UI_Popup>();
    public int PopupCount => _currentPopupStack.Count;

    HashSet<UI_Scene> _sceneUIs = new HashSet<UI_Scene>();
    public IReadOnlyList<UI_Scene> SceneUIs => _sceneUIs.ToArray();

    Dictionary<string, GameObject> _uiCashByPath = new Dictionary<string, GameObject>();
    public readonly float UIScreenWidth = 800;
    public readonly float UIScreenHeight = 480;

    Transform _root;
    public Transform Root
    {
        get
        {
            if (_root == null) _root = new GameObject("@UI_Root").transform;
            return _root;
        }
    }

    public void SetCanvas(GameObject go)
    {
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; // canvas안의 canvas가 부모 관계없이 독립적인 sort값을 가지게 하는 옵션
        go.GetOrAddComponent<GraphicRaycaster>();
        SetCanvasSacle(go);
    }

    void SetCanvasSacle(GameObject go)
    {
        var canvasScaler = go.GetOrAddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(UIScreenWidth, UIScreenHeight);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
    }

    public void SetSotingOrder(Canvas canvas)
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

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        var cash = GetSceneUI<T>();
        if (cash != null)
        {
            cash.gameObject.SetActive(true);
            return cash;
        }
        else
        {
            var result = ShowUI<T>("Scene", name);
            _sceneUIs.Add(result);
            return result;
        }
    }

    public T GetSceneUI<T>() where T : UI_Scene => _sceneUIs.OfType<T>().FirstOrDefault();

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        T popup = ShowUI<T>("Popup", name, InstantPopupUI);
        _currentPopupStack.Push(popup);
        SetSotingOrder(popup.gameObject.GetOrAddComponent<Canvas>());
        return popup;
    }

    GameObject InstantPopupUI(string path)
    {
        if (_uiCashByPath.TryGetValue(path, out GameObject popupCash))
            return popupCash;
        else
        {
            var go = Managers.Resources.Instantiate(path);
            _uiCashByPath.Add(path, go);
            return go;
        }
    }

    public T ShowDefualtUI<T>(string name = null) where T : UI_Base
    {
        T result = ShowUI<T>("Default", name);
        SetCanvasSacle(result.gameObject);
        SetSotingOrder(result.GetComponent<Canvas>());
        return result;
    }
    T ShowUI<T>(string uiType, string name = null, Func<string, GameObject> getObject = null, Transform parent = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;
        string path = $"UI/{uiType}/{name}";
        GameObject go = getObject == null ? Managers.Resources.Instantiate(path) : getObject(path);
        go.transform.SetParent(parent ?? Root); // ??는 parent == null ? null : Root 랑 같음
        go.gameObject.SetActive(true);
        return go.GetOrAddComponent<T>();
    }

    public void ClosePopupUI()
    {
        if(PopupCount > 0)
            _currentPopupStack.Pop().gameObject.SetActive(false);
    }

    public void CloseAllPopupUI()
    {
        _currentPopupStack.ToList().ForEach(x => x.gameObject.SetActive(false));
        _currentPopupStack.Clear();
    }

    public T FindPopupUI<T>() where T : UI_Popup
    {
        if (0 >= PopupCount) return null;
        return _currentPopupStack.Where(x => x is T).FirstOrDefault() as T;
    }

    public UI_Popup PeekPopupUI()
    {
        if (PopupCount == 0) return null;
        return _currentPopupStack.Peek();
    }

    public void Clear()
    {
        _root = null;
        _currentPopupStack.Clear();
        _uiCashByPath.Clear();
        _sceneUIs.Clear();
    }
}
