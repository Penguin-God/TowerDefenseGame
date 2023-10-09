using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestoryAfterSecond : MonoBehaviour
{
    [SerializeField] float aliveTime;
    void OnEnable()
    {
        StartCoroutine(Co_Destory(aliveTime));
    }

    public void ReturnObjet()
    {
        StopAllCoroutines();
        Managers.Resources.Destroy(gameObject);
    }

    IEnumerator Co_Destory(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        ReturnObjet();
    }
}
