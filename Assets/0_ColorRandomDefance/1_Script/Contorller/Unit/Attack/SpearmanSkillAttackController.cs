using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Photon.Pun;

public class SpearmanSkillAttackController : UnitAttackControllerTemplate
{
    protected override string AnimationName => "isSpecialAttack";

    NavMeshAgent _nav;
    UnitProjectileSync _projectileSync;
    [SerializeField] GameObject _spear;
    [SerializeField] Transform _shotPoint;
    protected override void Awake()
    {
        base.Awake();
        _nav = GetComponent<NavMeshAgent>();
        _projectileSync = GetComponent<UnitProjectileSync>();
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
        yield return StartCoroutine(Co_ShotSpear()); //new WaitForSeconds(PreparationTime); 

        _spear.SetActive(false);
        _nav.isStopped = true;
        PlaySound(EffectSoundType.SpearmanSkill);

        yield return new WaitForSeconds(0.5f);
        _nav.isStopped = false;
        _spear.SetActive(true);
    }

    // 쏘는 부분 자체를 따로 빼야됨
    const float PreparationTime = 0.9f;

    IEnumerator Co_ShotSpear()
    {
        yield return new WaitForSeconds(_throwSpearData.WaitForVisibility);
        var shotSpear = CreateSpear();
        _projectileSync.RegisterSyncProjectile(shotSpear, _attack, _throwSpearData.RotateVector);
        SetTrail(shotSpear, false); // 트레일 늘어지는거 방지
        yield return new WaitForSeconds(PreparationTime - _throwSpearData.WaitForVisibility);
        yield return new WaitForSeconds(0.1f);
        SetTrail(shotSpear, true);
        if (PhotonNetwork.IsMasterClient)
            _projectileSync.SyncProjectileShot(transform.forward);
    }

    UnitProjectile CreateSpear()
    {
        var shotSpear = Managers.Resources.Instantiate(_throwSpearData.WeaponPath, _shotPoint.position).GetComponent<UnitProjectile>();
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
}
