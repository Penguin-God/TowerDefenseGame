using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChaseState
{
    NoneTarget,
    Chase,
    Far,
    Close, // 적당히 가깝다
    InRange,
    Contact,
    Lock,
    FaceToFace,
}

public class UnitChaseUseCase
{
    float _range;
    const float ContactDistance = 4f;
    public UnitChaseUseCase(float range) => _range = range;

    public ChaseState CalculateChaseState(Vector3 chaserPos, Vector3 targetPos)
    {
        float distance = Vector3.Distance(targetPos, chaserPos);
        if (ContactDistance >= distance) return ChaseState.Contact;
        else if (_range * 0.8f >= distance) return ChaseState.Close;
        else return ChaseState.Far;
    }
}
