using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDirectionAttack : MonoBehaviour
{
    public void MultiDirectionShot(Transform directions, GameObject shotObject)
    {
        for (int i = 0; i < directions.childCount; i++)
        {
            Transform instantTransform = directions.GetChild(i);

            GameObject instantEnergyBall = Instantiate(shotObject, instantTransform.position, instantTransform.rotation);
            instantEnergyBall.transform.rotation = directions.GetChild(i).rotation;
            instantEnergyBall.GetComponent<Rigidbody>().velocity = directions.GetChild(i).rotation.normalized * Vector3.forward * 50;
        }
    }
}
