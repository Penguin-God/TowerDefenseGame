using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailPlugin : MonoBehaviour
{
    TrailRenderer trailRenderer;
    void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer == null)
            trailRenderer = GetComponentInChildren<TrailRenderer>();
    }
    void OnEnable() => trailRenderer.Clear();
}
