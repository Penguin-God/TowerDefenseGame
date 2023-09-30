using System.Collections;
using System.Collections.Generic;

public class ObjectManager<T>
{
    readonly List<T> _list = new List<T>();
    public IReadOnlyList<T> List => _list;
    public int Count => _list.Count;
    public void AddObject(T obj) => _list.Add(obj);
    public void RemoveObject(T obj) => _list.Remove(obj);
}
