using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSlowApplier : MonoBehaviour
{
    float _slowPercent;
    List<Multi_NormalEnemy> _targets = new List<Multi_NormalEnemy>();

    public void SetInfo(float slowPer, float raduis)
    {
        _slowPercent = slowPer;
        GetComponentInChildren<SphereCollider>().radius = raduis;
    }

    void ApplySlow(Multi_NormalEnemy enemy)
    {
        if (_targets.Contains(enemy) == false && enemy.IsSlow == false)
        {
            _targets.Add(enemy);
            enemy.OnDead += _ => CancelSlow(enemy);
            enemy.OnSlow(_slowPercent, Mathf.Infinity);
        }
    }

    void CancelSlow(Multi_NormalEnemy enemy)
    {
        if (_targets.Contains(enemy))
        {
            _targets.Remove(enemy);
            enemy.RestoreSpeedToAll();
        }
    }

    void FixedUpdate()
    {
        foreach (var monster in _targets)
        {
            if (monster.IsSlow == false)
                monster.OnSlow(_slowPercent, Mathf.Infinity);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponentInParent<Multi_NormalEnemy>();
        if (enemy != null)
            ApplySlow(enemy);
    }

    void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponentInParent<Multi_NormalEnemy>();
        if (enemy != null)
            CancelSlow(enemy);
    }
}
