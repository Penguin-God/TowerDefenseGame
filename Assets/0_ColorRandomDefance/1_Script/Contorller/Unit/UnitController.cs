using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitController : MonoBehaviour
{
    UnitAttackController _attackController;
    void Awake()
    {
        _attackController = GetComponent<UnitAttackController>();
    }

    public void Attack()
    {

    }

    // protected abstract I
}
