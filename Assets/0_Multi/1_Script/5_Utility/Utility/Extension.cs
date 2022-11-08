using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        => Util.GetOrAddComponent<T>(go);

    public static T GetRandom<T>(this List<T> list) => list[Random.Range(0, list.Count)];
}
