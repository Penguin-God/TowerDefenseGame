using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Multi_SpearmanEnemy : Multi_NormalEnemy
{
    [PunRPC]
    protected override void RPC_OnDamage(int damage, bool isSkill)
    {
        if (isSkill == false)
            damage /= 2;
        base.RPC_OnDamage(damage, isSkill);
    }
}
