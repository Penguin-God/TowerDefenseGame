using System;
using UnityEngine;

public class BluePassive : UnitPassive
{
    [SerializeField] float apply_SlowPercet;
    [SerializeField] float apply_SlowTime;


    //[Space] [Space]
    //[SerializeField] float slowPercent;
    //[SerializeField] float slowTime;

    // 법사가 쓰기 위한 변수들
    public float Get_SlowPercent => apply_SlowPercet;
    // 법사 패시브에서 slowTime은 무한이므로 콜라이더 범위 변수로 씀
    public float Get_ColliderRange => apply_SlowTime;
    public event Action OnBeefup;

    public override void SetPassive() => teamSoldier.delegate_OnPassive += (Enemy enemy) => enemy.EnemySlow(apply_SlowPercet, apply_SlowTime);


    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_SlowPercet = p1;
        apply_SlowTime = p2;
    }

    //public override void ApplyData(float p1, float en_p1, float p2 = 0, float en_p2 = 0, float p3 = 0, float en_p3 = 0)
    //{
    //    slowPercent = p1;
    //    enhanced_SlowPercent = en_p1;
    //    slowTime = p2;
    //    enhanced_SlowTime = en_p2;

    //    apply_SlowPercet = slowPercent;
    //    apply_SlowTime = slowTime;
    //}

    //[Space] [Space] [Space]
    //[SerializeField] float enhanced_SlowPercent;
    //[SerializeField] float enhanced_SlowTime;
    //public override void Beefup_Passive()
    //{
    //    apply_SlowPercet = enhanced_SlowPercent;
    //    apply_SlowTime = enhanced_SlowTime;
    //    if(OnBeefup != null) OnBeefup();
    //}
}
