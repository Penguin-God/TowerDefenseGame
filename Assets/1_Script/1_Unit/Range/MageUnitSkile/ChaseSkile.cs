using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChaseSkile : MonoBehaviour
{
    public event Action OnChase;

    private void OnEnable()
    {
        if (OnChase != null) OnChase();
    }
}
