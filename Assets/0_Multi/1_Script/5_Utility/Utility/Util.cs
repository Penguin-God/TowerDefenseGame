using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null) component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject parent, bool recursive = false, string name = "")
    {
        Transform tf = FindChild<Transform>(parent, recursive, name);
        if (tf != null) return tf.gameObject;
        else return null;
    }

    public static T FindChild<T>(GameObject parent, bool recursive = false, string findChildName = null) where T : UnityEngine.Object
    {
        if (parent == null) return null;

        if (recursive == false)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Transform tf = parent.transform.GetChild(i);
                if (string.IsNullOrEmpty(findChildName) || tf.name == findChildName)
                {
                    T component = tf.GetComponent<T>();
                    if (component != null) return component;
                }
            }
        }
        else
        {
            foreach (T component in parent.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(findChildName) || component.name == findChildName)
                    return component;
            }
        }

        return null;
    }

    public static List<int> GetRangeList(int start, int end)
    {
        var result = new List<int>();
        for (int i = start; i < end; i++)
            result.Add(i);
        return result;
    }
}
