using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMage : Unit_Mage
{
    [SerializeField] Transform skileShotPositions = null;
    [SerializeField] Transform ultimate_SkileShotPositions = null;

    public override void SetMageAwake() 
    {
        SettingSkilePool(mageSkillObject, 50);
    }

    public override void MageSkile()
    {
        base.MageSkile();

        Transform useSkileTransform = (isUltimate) ? ultimate_SkileShotPositions : skileShotPositions;
        MultiDirectionShot(useSkileTransform);
    }

    void MultiDirectionShot(Transform directions)
    {
        for (int i = 0; i < directions.childCount; i++)
        {
            Transform instantTransform = directions.GetChild(i);

            GameObject instantEnergyBall = UsedSkill(instantTransform.position);
            instantEnergyBall.transform.rotation = instantTransform.rotation;
            instantEnergyBall.GetComponent<CollisionWeapon>().Shoot(instantTransform.forward, 50, (Enemy enemy) => delegate_OnHit(enemy));
        }
    }
}
