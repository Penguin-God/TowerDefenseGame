﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_MageEnemy : Multi_NormalEnemy
{
    readonly float DAMAGE_REDUCTION_RATE = 50;
    [PunRPC]
    protected override void RPC_OnDamage(int damage, bool isSkill)
    {
        if (isSkill)
        {
            // TODO : 이팩트 더 방어가 되는 느낌으로 바꾸기
            // photonView.RPC(nameof(DecreasedEffect), RpcTarget.All);
            damage -= Mathf.CeilToInt(damage * DAMAGE_REDUCTION_RATE * 0.01f);
        }
        base.RPC_OnDamage(damage, isSkill);
    }

    Coroutine _coMat;
    [PunRPC]
    void DecreasedEffect()
    {
        Managers.Effect.ChangeAllMaterial("Gray", transform);
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
