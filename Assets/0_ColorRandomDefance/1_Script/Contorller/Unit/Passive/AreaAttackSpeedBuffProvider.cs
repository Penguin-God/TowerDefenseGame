using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttackSpeedBuffProvider : MonoBehaviour
{
    float _buffAmount;
    List<Multi_TeamSoldier> _passiveTargets = new List<Multi_TeamSoldier>();

    public void DependecyInject(float radius, float buffAmount)
    {
        GetComponent<SphereCollider>().radius = radius;
        _buffAmount = buffAmount;
    }

    void OnTriggerEnter(Collider other)
    {
        var unit = other.GetComponentInParent<Multi_TeamSoldier>();
        if (unit != null && _passiveTargets.Contains(unit) == false)
        {
            unit.Unit.Stats.AttackSpeed += _buffAmount;
            _passiveTargets.Add(unit);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var unit = other.GetComponentInParent<Multi_TeamSoldier>();
        if (unit != null && _passiveTargets.Contains(unit))
        {
            unit.Unit.Stats.AttackSpeed -= _buffAmount;
            _passiveTargets.Remove(unit);
        }
    }

    void OnDisable()
    {
        foreach (var target in _passiveTargets)
            target.Unit.Stats.AttackSpeed -= _buffAmount;
        _passiveTargets.Clear();
    }
}
