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
            ProjectileData data = new ProjectileData(mageSkillObject.GetComponent<Multi_Projectile>(), transform, instantTransform);
            ProjectileShotDelegate.ShotProjectile(data, instantTransform.forward, OnSkileHit);
        }
    }


    //public override void MageSkile()
    //{
    //    // TODO : Event로 옮기기
    //    Transform useSkileTransform = (isUltimate) ? ultimate_SkileShotPositions : skileShotPositions;
    //    MultiDirectionShot(useSkileTransform);
    //}

    //void MultiDirectionShot(Transform directions)
    //{
    //    for (int i = 0; i < directions.childCount; i++)
    //    {
    //        Transform instantTransform = directions.GetChild(i);

    //        GameObject instantEnergyBall = UsedSkill(instantTransform.position);
    //        instantEnergyBall.transform.rotation = instantTransform.rotation;
    //        instantEnergyBall.GetComponent<Multi_Projectile>()
    //            .Shot(instantTransform.position, instantTransform.forward, 50, OnSkileHit);
    //    }
    //}
}
