using System;
using UnityEngine;

public class BluePassive : UnitPassive
{
    [SerializeField] float apply_SlowPercet;
    [SerializeField] float apply_SlowTime;

    // 법사가 쓰기 위한 변수들
    public float Get_SlowPercent => apply_SlowPercet;
    // 법사 패시브에서 slowTime은 무한이므로 콜라이더 범위 변수로 씀
    public float Get_ColliderRange => apply_SlowTime;
    public event Action OnBeefup;

    public override void SetPassive(TeamSoldier _team)
    {
        _team.delegate_OnPassive += (Enemy enemy) => enemy.EnemySlow(apply_SlowPercet, apply_SlowTime);
    }

    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_SlowPercet = p1;
        apply_SlowTime = p2;
    }
}
