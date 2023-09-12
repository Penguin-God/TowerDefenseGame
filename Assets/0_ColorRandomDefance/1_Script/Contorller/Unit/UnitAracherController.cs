using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAracherController : MonoBehaviour
{
    object attack;
    object skillattack;

    UnitAttackController attaker;
    void Awake()
    {
        attaker = GetComponent<UnitAttackController>();
    }

    object GetAttackCommend() => null;
}
