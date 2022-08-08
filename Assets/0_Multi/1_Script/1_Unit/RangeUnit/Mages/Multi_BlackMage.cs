using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_BlackMage : Multi_Unit_Mage
{
    [SerializeField] Transform skileShotPositions = null;
    [SerializeField] Transform ultimate_SkileShotPositions = null;

    protected override void MageSkile()
    {
        MultiDirectionShot(skileShotPositions);
    }

    void MultiDirectionShot(Transform directions)
    {
        for (int i = 0; i < directions.childCount; i++)
        {
            Transform instantTransform = directions.GetChild(i);
            ProjectileShotDelegate.ShotProjectile(skillData, instantTransform.position, instantTransform.forward, OnSkileHit);
        }
    }
}
