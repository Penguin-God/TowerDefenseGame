using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Object = UnityEngine.Object;

public abstract class Multi_UI_Base : MonoBehaviour
{
    protected Dictionary<Type, Object[]> _objectsByType = new Dictionary<Type, Object[]>();

    protected bool _initDone = false;
    protected virtual void Init() { }

    void Start()
    {
        if (_initDone) return;
        Init();
        _initDone = true;
    }

    protected void Bind<T>(Type type) where T : Object
    {
        string[] names = Enum.GetNames(type); // enum의 이름들을 가져옴. C# 리플렉션의 놀라운 은총, 근데 C++은 없음 ㅋㅋ
        Object[] objects = new Object[names.Length];
        _objectsByType.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, true, names[i]);
            else
                objects[i] = Util.FindChild<T>(gameObject, true, names[i]);

            if (objects[i] == null) print($"실패!!!   실패한 이름 {names[i]}");
        }
    }

    protected T Get<T>(int index) where T : Object
    {
        if (_objectsByType.TryGetValue(typeof(T), out Object[] objects) == false) return null;

        return objects[index] as T;
    }

    protected GameObject GetObject(int index) => Get<GameObject>(index);
    protected Text GetText(int index) => Get<Text>(index);
    protected Button GetButton(int index) => Get<Button>(index);
    protected Image GetImage(int index) => Get<Image>(index);

    public static void BindEvnet(GameObject go, Action<PointerEventData> action, UI_Event type = UI_Event.Click)
    {
        UI_EventHandler handler = go.GetOrAddComponent<UI_EventHandler>();
        BindEvnet(handler, action, type);
    }

    public static void BindEvnets(GameObject[] gos, Action<PointerEventData> action, UI_Event type = UI_Event.Click)
    {
        for (int i = 0; i < gos.Length; i++)
        {
            UI_EventHandler handler = gos[i].GetOrAddComponent<UI_EventHandler>();
            BindEvnet(handler, action, type);
        }
    }

    public static void BindEvnet(UI_EventHandler handler, Action<PointerEventData> action, UI_Event eventType = UI_Event.Click)
    {
        switch (eventType)
        {
            case UI_Event.Click: handler.OnClickHandler += action; break;
        }
    }
}
