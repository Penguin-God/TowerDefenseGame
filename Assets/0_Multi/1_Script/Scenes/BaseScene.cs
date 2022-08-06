using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {

    }

    public virtual void Clear()
    {

    }
}
