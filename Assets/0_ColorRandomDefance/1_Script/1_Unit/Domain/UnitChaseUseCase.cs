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
    Kiss,
}

public class UnitChaseUseCase
{
    float _range;
    const float ContactDistance = 4f;
    public UnitChaseUseCase(float range) => _range = range;

    public ChaseState CalculateChaseState(Vector3 chaserPos, Vector3 targetPos, Vector3 chaserDir, Vector3 targetDir)
    {
        float distance = Vector3.Distance(targetPos, chaserPos);
        if (IsFacingTarget(chaserDir, targetDir) && distance < _range) return ChaseState.FaceToFace;
        else return CalculateChaseState(chaserPos, targetPos);
    }

    ChaseState CalculateChaseState(Vector3 chaserPos, Vector3 targetPos)
    {
        float distance = Vector3.Distance(targetPos, chaserPos);
        if (ContactDistance >= distance) return ChaseState.Contact;
        else if (_range * 0.8f >= distance) return ChaseState.Close;
        else return ChaseState.Far;
    }

    public Vector3 GetDestinationPos(ChaseState state, Vector3 targetPostion, Vector3 targetForward)
    {
        switch (state)
        {
            case ChaseState.Chase: return targetPostion - (targetForward * 1);
            case ChaseState.InRange: return targetPostion - (targetForward * 2);
            case ChaseState.FaceToFace: return targetPostion - (targetForward * -5f);
            case ChaseState.Lock: return targetPostion - (targetForward * -1f);
            default : return Vector3.zero;
        }
    }

    bool IsFacingTarget(Vector3 chaserDir, Vector3 targetDir) => Vector3.Dot(targetDir, chaserDir) < -0.8f;
}
