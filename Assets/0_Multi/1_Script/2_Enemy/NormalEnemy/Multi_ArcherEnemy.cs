using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_ArcherEnemy : Multi_NormalEnemy
{
    protected override void Passive()
    {
        ChangeMaxSpeed(maxSpeed * 1.5f);
    }

    [PunRPC]
    protected override void OnSlow(float slowPercent, float slowTime)
    {
        slowPercent /= 2;
        base.OnSlow(slowPercent, slowTime);
    }
}
