using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitChaseController : MonoBehaviour
{
    Transform target;
    NavMeshAgent _nav;
    IEnumerator NavCoroutine()
    {
        while (true)
        {
            yield return null;
            if (target == null)
            {
                UpdateTarget();
                continue;
            }
        }
    }

    void UpdateTarget()
    {

    }

    public void Stop() => _nav.isStopped = true;
    public void ReleaseFix() => _nav.isStopped = false;
}
