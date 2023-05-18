using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitController
{
    public bool TryCombine(UnitFlags targetFlag);
}
