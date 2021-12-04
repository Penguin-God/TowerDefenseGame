using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMage : Unit_Mage
{
    [SerializeField] Transform skileShotPositions = null;
    [SerializeField] Transform ultimate_SkileShotPositions = null;

    public override void MageSkile()
    {
        base.MageSkile();

        Transform useSkileTransform = (isUltimate) ? ultimate_SkileShotPositions : skileShotPositions;
        MultiDirectionShot(useSkileTransform, mageEffectObject);
    }

    void MultiDirectionShot(Transform directions, GameObject shotObject)
    {
        for (int i = 0; i < directions.childCount; i++)
        {
            Transform instantTransform = directions.GetChild(i);

            GameObject instantEnergyBall = Instantiate(shotObject, instantTransform.position, instantTransform.rotation);
            instantEnergyBall.GetComponent<CollisionWeapon>().UnitOnDamage += (Enemy enemy) => delegate_OnSkile(enemy);
            instantEnergyBall.GetComponent<Rigidbody>().velocity = directions.GetChild(i).rotation.normalized * Vector3.forward * 50;
        }
    }
}
