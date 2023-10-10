using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitController : MonoBehaviour
{
    __UnitAttackController _attackController;
    void Awake()
    {
        _attackController = GetComponent<__UnitAttackController>();
    }

    public void Attack()
    {

    }

    // protected abstract I
}
