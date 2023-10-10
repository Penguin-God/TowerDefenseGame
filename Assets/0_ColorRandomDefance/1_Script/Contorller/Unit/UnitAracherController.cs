using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAracherController : MonoBehaviour
{
    object attack;
    object skillattack;

    __UnitAttackController attaker;
    void Awake()
    {
        attaker = GetComponent<__UnitAttackController>();
    }

    object GetAttackCommend() => null;
}
