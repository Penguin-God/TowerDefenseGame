using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Multi_MageEnemy : Multi_NormalEnemy
{
    // 패시브 무효화
    protected override void OnSlow(float slowPercent, float slowTime) { }
    protected override void ExitSlow() { }
    protected override void OnStun(int stunPercent, float stunTime) { }
    protected override void OnPoison(int poisonPercent, int poisonCount, float poisonDelay, int maxDamage) { }
}
