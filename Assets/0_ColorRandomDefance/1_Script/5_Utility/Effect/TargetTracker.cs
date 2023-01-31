using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTracker : MonoBehaviour
{
    Transform _target;
    Vector3 _offset;
    public void SetInfo(Transform target, Vector3 offset)
    {
        _target = target;
        _offset = offset;
    }

    void Update()
    {
        if (_target == null) return;
        transform.position = _target.position + _offset;
    }
}
