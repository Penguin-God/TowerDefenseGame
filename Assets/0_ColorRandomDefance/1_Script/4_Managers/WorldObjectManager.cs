using System.Collections;
using System.Collections.Generic;

public class WorldObjectManager<T>
{
    readonly MultiData<ObjectManager<T>> _data = new MultiData<ObjectManager<T>>(() => new ObjectManager<T>());
    ObjectManager<T> GetManager(byte id) => _data.GetData(id);

    public IReadOnlyList<T> GetList(byte id) => GetManager(id).List;
    public int GetCount(byte id) => GetManager(id).Count;
    public void AddObject(T obj, byte id) => GetManager(id).AddObject(obj);
    public void RemoveObject(T obj, byte id) => GetManager(id).RemoveObject(obj);
}
