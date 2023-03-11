using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiServiceMidiator
{
       
}


public class MultiService<T>
{
    T[] ts = new T[2];
    T GetServiece() => ts[PlayerIdManager.Id];
    T GetServiece(byte id) => ts[id];
}
