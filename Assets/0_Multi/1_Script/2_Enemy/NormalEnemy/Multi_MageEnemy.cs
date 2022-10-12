using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Multi_MageEnemy : Multi_NormalEnemy
{
    [PunRPC]
    protected override void RPC_OnDamage(int damage, bool isSkill)
    {
        if (isSkill) damage -= Mathf.CeilToInt(damage * 80 * 0.01f);
        base.RPC_OnDamage(damage, isSkill);
    }
}
