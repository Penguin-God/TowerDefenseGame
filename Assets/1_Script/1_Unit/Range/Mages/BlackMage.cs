using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMage : Unit_Mage
{
    [SerializeField] Transform skileShotPositions = null;
    [SerializeField] Transform ultimate_SkileShotPositions = null;

    // 검마는 사전작업 필요 없는데 놔두면 부모 SetMageAwake를 실행해 버려서 일부러 비워둠
    public override void SetMageAwake() {}

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
            //instantEnergyBall.GetComponent<CollisionWeapon>().UnitOnDamage += (Enemy enemy) => delegate_OnSkile(enemy);
            instantEnergyBall.GetComponent<Rigidbody>().velocity = directions.GetChild(i).rotation.normalized * Vector3.forward * 50;
        }
    }
}
