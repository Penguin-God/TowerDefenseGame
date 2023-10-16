using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MageAttackerController : UnitAttackControllerTemplate
{
    protected override string AnimationName => "isAttack";
    NavMeshAgent _nav;
    Action<Vector3> _shotEnergyball;
    [SerializeField] GameObject _magicLight;
    [SerializeField] Transform _shotPoint;
    protected override void Awake()
    {
        base.Awake();
        _nav = GetComponent<NavMeshAgent>();
    }

    protected ManaSystem _manaSystem;
    public void Inject(ManaSystem manaSystem, Action<Vector3> shotEnergyball)
    {
        _manaSystem = manaSystem;
        _shotEnergyball = shotEnergyball;
    }

    protected override IEnumerator Co_Attack()
    {
        _nav.isStopped = true;
        yield return WaitSecond(0.7f);
        _magicLight.SetActive(true);

        _shotEnergyball?.Invoke(_shotPoint.position);
        if (PhotonNetwork.IsMasterClient)
            _manaSystem.AddMana_RPC();

        yield return WaitSecond(0.5f);
        _magicLight.SetActive(false);
        _nav.isStopped = false;
    }
}
