using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_MageEnemy : Multi_NormalEnemy
{
    [SerializeField] Material en;
    [PunRPC]
    protected override void RPC_OnDamage(int damage, bool isSkill)
    {
        if (isSkill)
        {
            //photonView.RPC(nameof(DecreasedEffect), RpcTarget.All);
            damage -= Mathf.CeilToInt(damage * 80 * 0.01f);
        }
        base.RPC_OnDamage(damage, isSkill);
    }

    [PunRPC]
    void DecreasedEffect()
    {
        //Multi_Managers.Effect.ChangeAllMaterial(transform);
        StartCoroutine(Co_ChangedMat());
    }

    IEnumerator Co_ChangedMat()
    {
        yield return new WaitForSeconds(2f);
        ChangeMat(originMat);
    }
}
