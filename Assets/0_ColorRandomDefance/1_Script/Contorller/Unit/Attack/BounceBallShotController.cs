using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBallShotController : MonoBehaviour
{
    MageAttackerController _energyShotController;
    ManaSystem _manaSystem;
    Animator _animator;
    void Awake()
    {
        _energyShotController = GetComponent<MageAttackerController>();
        _manaSystem = GetComponent<ManaSystem>();
        _animator = GetComponentInChildren<Animator>();
    }

    float _manaLockTime;
    Action<Vector3> _shotEnergyball;
    public void DependencyInject(float manaLockTime, Action<Vector3> shotEnergyball)
    {
        _manaLockTime = manaLockTime;
        _shotEnergyball = shotEnergyball;
    }

    public void DoSkill() => StartCoroutine(Co_Spell());
    IEnumerator Co_Spell()
    {
        _animator.SetTrigger("isAttack");
        _manaSystem.LockManaForDuration(_manaLockTime);
        yield return StartCoroutine(_energyShotController.Co_ShotBounceBall(_shotEnergyball));
    }
}
