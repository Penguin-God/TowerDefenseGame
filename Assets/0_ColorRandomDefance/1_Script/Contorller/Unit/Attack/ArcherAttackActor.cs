using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherAttackActor : MonoBehaviour, IUnitAttackActor
{
    [SerializeField] TrailRenderer _trail;
    [SerializeField] NavMeshAgent _nav;

    public float AttackCoolTime => 0.5f;
    public IEnumerator Do(Multi_Enemy target)
    {
        _nav.isStopped = true;
        _trail.gameObject.SetActive(false);
        // _thrower.FlatThrow(target, base.NormalAttack);
        yield return new WaitForSeconds(1f);
        _trail.gameObject.SetActive(true);
        _nav.isStopped = false;
    }
}
