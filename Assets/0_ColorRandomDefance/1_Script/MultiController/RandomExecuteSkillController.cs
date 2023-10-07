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

    public void RandomAttack(int skillRate)
    {
        bool isSkill = skillRate > UnityEngine.Random.Range(0, 101);
        photonView.RPC(nameof(Attack), RpcTarget.All, isSkill);
    }

    [PunRPC]
    void Attack(bool isSkill)
    {
        if (isSkill) _normalAct.Invoke();
        else _skillAct.Invoke();
    }
}
