using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class RandomExcuteSkillController : MonoBehaviourPun
{
    Action _normalAct;
    Action _skillAct;

    public void DependencyInject(Action normalAct, Action skillAct)
    {
        _normalAct = normalAct;
        _skillAct = skillAct;
    }

    public void RandomAttack(int skillRate) => photonView.RPC(nameof(Attack), RpcTarget.All, MathUtil.GetRandomBoolByRate(skillRate));

    [PunRPC]
    void Attack(bool isSkill)
    {
        if (isSkill) _skillAct.Invoke();
        else _normalAct.Invoke();
    }
}
