using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class SpearmanSkillAttackController : UnitAttackControllerTemplate
{
    protected override string AnimationName => "isSpecialAttack";

    NavMeshAgent _nav;
    [SerializeField] GameObject _spear;
    [SerializeField] Transform _shotPoint;
    protected override void Awake()
    {
        base.Awake();
        _nav = GetComponent<NavMeshAgent>();
    }

    ThrowSpearData _throwSpearData;
    Action<Multi_Enemy> _attack;
    public void ChangeSpearData(ThrowSpearData throwSpearData, Action<Multi_Enemy> attack)
    {
        _throwSpearData = throwSpearData;
        _attack = attack;
    }

    protected override IEnumerator Co_Attack()
    {
        yield return StartCoroutine(Co_ShotSpear());

        _spear.SetActive(false);
        _nav.isStopped = true;
        // PlaySound(EffectSoundType.SpearmanSkill);

        yield return WaitSecond(0.5f);
        _nav.isStopped = false;
        _spear.SetActive(true);
    }

    IEnumerator Co_ShotSpear()
    {
        yield return WaitSecond(_throwSpearData.WaitForVisibility);
        var shotSpear = CreateSpear();
        SetTrail(shotSpear, false); // 트레일 늘어지는거 방지
        yield return WaitSecond(CalculateDelayTime(1) - _throwSpearData.WaitForVisibility);
        SetTrail(shotSpear, true);
        ThrowSpear(shotSpear);
    }

    Multi_Projectile CreateSpear()
    {
        var shotSpear = Managers.Resources.Instantiate(_throwSpearData.WeaponPath, _shotPoint.position).GetComponent<Multi_Projectile>();
        shotSpear.GetComponent<Collider>().enabled = false;
        shotSpear.transform.rotation = Quaternion.LookRotation(transform.forward);
        foreach (var particle in shotSpear.GetComponentsInChildren<ParticleSystem>())
        {
            var main = particle.main;
            main.simulationSpeed = _unit.Stats.AttackSpeed;
        }
        return shotSpear;
    }

    void SetTrail(Component spear, bool isActive)
    {
        var trail = spear.GetComponentInChildren<TrailRenderer>();
        if (trail != null)
            trail.enabled = isActive;
    }

    void ThrowSpear(Multi_Projectile shotSpear)
    {
        shotSpear.transform.position = _shotPoint.position;
        shotSpear.GetComponent<Collider>().enabled = true;
        shotSpear.AttackShot(transform.forward, _attack);
        shotSpear.transform.Rotate(_throwSpearData.RotateVector);
    }
}
