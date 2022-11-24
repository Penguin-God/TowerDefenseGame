using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_MageEnemy : Multi_NormalEnemy
{
    [PunRPC]
    protected override void RPC_OnDamage(int damage, bool isSkill)
    {
        if (isSkill)
        {
            photonView.RPC(nameof(DecreasedEffect), RpcTarget.All);
            damage -= Mathf.CeilToInt(damage * 80 * 0.01f);
        }
        base.RPC_OnDamage(damage, isSkill);
    }

    Coroutine _coMat;
    [PunRPC]
    void DecreasedEffect()
    {
        Multi_Managers.Effect.ChangeAllMaterial("Gray", transform);
        Multi_Managers.Sound.PlayEffect(EffectSoundType.DebuffSkill);
        if (_coMat != null) StopCoroutine(_coMat);
        _coMat = StartCoroutine(Co_ChangedMat());
    }

    IEnumerator Co_ChangedMat()
    {
        yield return new WaitForSeconds(2f);
        ChangeMat(originMat);
        _coMat = null;
    }
}
