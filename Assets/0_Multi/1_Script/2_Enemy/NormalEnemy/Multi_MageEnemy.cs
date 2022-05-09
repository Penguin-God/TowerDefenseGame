using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Multi_MageEnemy : Multi_NormalEnemy
{
    // 상태이상 무효화
    [PunRPC] protected override void OnSlow(float slowPercent, float slowTime) { }
    [PunRPC] protected override void ExitSlow() { }
    [PunRPC] protected override void OnStun(int stunPercent, float stunTime) { }
    [PunRPC] protected override void OnPoison(int poisonPercent, int poisonCount, float poisonDelay, int maxDamage) { }
}
