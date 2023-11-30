using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class AreaSlowApplier : MonoBehaviourPun
{
    [SerializeField] float _slowPercent;
    List<Multi_NormalEnemy> _targets = new List<Multi_NormalEnemy>();

    public void Inject(float slowPer, float raduis)
    {
        _slowPercent = slowPer;
        GetComponentInChildren<SphereCollider>().radius = raduis;
    }

    [PunRPC]
    void SetInfo(float slowPer, float raduis)
    {
        _slowPercent = slowPer;
        GetComponentInChildren<SphereCollider>().radius = raduis;
    }

    void ApplySlow(Multi_NormalEnemy enemy)
    {
        if (_targets.Contains(enemy) == false)
        {
            _targets.Add(enemy);
            enemy.OnDead += _ => ExitSlow(enemy);
            enemy.OnSlow(_slowPercent);
        }
    }

    void ExitSlow(Multi_NormalEnemy enemy)
    {
        if (_targets.Contains(enemy))
        {
            _targets.Remove(enemy);
            enemy.SlowController.ExitInfinitySlow();
        }
    }

    void OnEnable() => StartCoroutine(Co_KeepSlowInRage());

    void OnDisable()
    {
        foreach (Multi_NormalEnemy target in _targets)
            target.SlowController.ExitInfinitySlow();
        _targets.Clear();
        StopCoroutine(Co_KeepSlowInRage());
    }

    IEnumerator Co_KeepSlowInRage()
    {
        while (true)
        {
            foreach (var monster in _targets.Where(x => x.IsSlow == false))
                monster.OnSlow(_slowPercent);
            yield return null;
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
            ExitSlow(enemy);
    }
}
