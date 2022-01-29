using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class UnitPassive : MonoBehaviour
{
    public abstract void SetPassive(TeamSoldier _team);

    public abstract void ApplyData(float p1, float p2 = 0, float p3 = 0);
}
