using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttackSpeedBuffProvider : MonoBehaviour
{
    [SerializeField] float _attackDelayRate;
    [SerializeField] List<Multi_TeamSoldier> _passiveTargets = new List<Multi_TeamSoldier>();

    public void DependecyInject(float radius, float delayRate)
    {
        GetComponentInChildren<SphereCollider>().radius = radius;
        _attackDelayRate = delayRate;
    }

    void OnTriggerEnter(Collider other)
    {
        var unit = other.GetComponentInParent<Multi_TeamSoldier>();
        if (unit != null && _passiveTargets.Contains(unit) == false)
        {
            Change_Unit_AttackCollDown(unit, _attackDelayRate);
            _passiveTargets.Add(unit);
        }
    }

    void OnTriggerExit(Collider other) // redPassive.get_DownDelayWeigh 의 역수 곱해서 공속 되돌림
    {
        var unit = other.GetComponentInParent<Multi_TeamSoldier>();
        if (unit != null && _passiveTargets.Contains(unit))
        {
            Change_Unit_AttackCollDown(unit, (1 / _attackDelayRate));
            _passiveTargets.Remove(unit);
        }
    }

    void Change_Unit_AttackCollDown(Multi_TeamSoldier _unit, float rate) => _unit.AttackDelayTime *= rate;
}
