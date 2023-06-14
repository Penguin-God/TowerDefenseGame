using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChaseState
{
    NoneTarget,
    Chase,
    OutRange,
    InRange,
    Lock,
    FaceToFace,
}

public class UnitChaseUseCase
{
    float _range;
    public UnitChaseUseCase(float range) => _range = range;

    public ChaseState CalculateChaseState(Vector3 chaserPos, Vector3 targetPos)
    {
        float distance = Vector3.Distance(targetPos, chaserPos);
        if (distance > _range) return ChaseState.OutRange;
        else return ChaseState.InRange;
    }
}
