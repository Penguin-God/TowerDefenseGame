using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    void Awake() => Init();

    protected abstract void Init();

    public virtual void Clear()
    {

    }
}
