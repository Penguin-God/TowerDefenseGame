using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSlowApplier : MonoBehaviour
{
    private float _slowPercent;
    private List<Multi_NormalEnemy> _targets = new List<Multi_NormalEnemy>();

    public void SetInfo(float slowPer, float raduis)
    {
        _slowPercent = slowPer;
        GetComponentInChildren<SphereCollider>().radius = raduis;
    }

    void ApplySlow(Multi_NormalEnemy enemy)
    {
        if (!_targets.Contains(enemy) && !enemy.IsSlow)
        {
            _targets.Add(enemy);
            enemy.OnSlow(_slowPercent, -1);
        }
    }

    void CancelSlow(Multi_NormalEnemy enemy)
    {
        if (_targets.Contains(enemy))
        {
            _targets.Remove(enemy);
            enemy.ExitSlow(RpcTarget.All); // TODO : 나중에 동기화 마스터한테 옮기고 이게 맞는지 확인해보기
        }
    }

    void FixedUpdate()
    {
        foreach (var monster in _targets)
        {
            if (monster.IsSlow == false)
                monster.OnSlow(_slowPercent, -1);
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
