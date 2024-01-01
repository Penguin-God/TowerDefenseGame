using Photon.Pun;
using System;

public class NetworkAttackController : MonoBehaviourPun
{
    Action _normalAct;
    Action _skillAct;
    AttackCounter _attackCounter;

    public void DependencyInject(Action normalAct, Action skillAct, AttackCounter attackCounter)
    {
        _normalAct = normalAct;
        _skillAct = skillAct;
        _attackCounter = attackCounter;
    }

    public void NetworkAttack(int skillRate)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        bool isSkill = _attackCounter.IsSkillable;
        photonView.RPC(nameof(Attack), RpcTarget.All, isSkill);

        if (isSkill) _attackCounter.UseSkill();
        else _attackCounter.AddAttackCount();
    }

    [PunRPC]
    void Attack(bool isSkill)
    {
        if (isSkill) _skillAct.Invoke();
        else _normalAct.Invoke();
    }
}

public class AttackCounter
{
    readonly int NeedCountForSkill;
    int _currentAttackCount;
    public bool IsSkillable => _currentAttackCount >= NeedCountForSkill;
    public AttackCounter(int needSkollCountForSkill) => NeedCountForSkill = needSkollCountForSkill;

    public void AddAttackCount() => _currentAttackCount++;
    public void UseSkill() => _currentAttackCount = 0;
}