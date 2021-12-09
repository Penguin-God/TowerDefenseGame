using System;
using UnityEngine;

public class BluePassive : UnitPassive
{
    [SerializeField] float slowPercent;
    [SerializeField] float slowTime;

    // 법사가 쓰기 위한 변수들
    public float get_SlowPercent { get { return slowPercent; } }
    // 법사 패시브에서 slowTime은 무한이므로 콜라이더 범위 변수로 씀
    public float get_ColliderRange { get { return slowTime; } }
    public event Action OnBeefup;

    public override void SetPassive()
    {
        teamSoldier.delegate_OnPassive += (Enemy enemy) => enemy.EnemySlow(slowPercent, slowTime);
    }

    [Space] [Space] [Space]
    [SerializeField] float enhanced_SlowPercent;
    [SerializeField] float enhanced_SlowTime;
    public override void Beefup_Passive()
    {
        slowPercent = enhanced_SlowPercent;
        slowTime = enhanced_SlowTime;
        OnBeefup();
    }
}
