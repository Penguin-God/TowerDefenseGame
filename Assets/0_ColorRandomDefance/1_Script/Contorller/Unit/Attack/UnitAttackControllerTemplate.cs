using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttackControllerTemplate : MonoBehaviour
{
    protected Animator _animator;
    protected virtual string AnimationName { get; }
    WorldAudioPlayer _worldAudioPlayer;
    
    UnitStateManager _unitState;
    protected Unit _unit;

    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _worldAudioPlayer = GetComponent<WorldAudioPlayer>();
    }

    public void DependencyInject(UnitStateManager unitState, Unit unit)
    {
        _unitState = unitState;
        _unit = unit;
    }

    protected virtual void DoAnima() => _animator.SetTrigger(AnimationName);
    public void DoAttack(float coolDownTime)
    {
        if (_animator != null) DoAnima();
        StartCoroutine(Co_DoAttack(coolDownTime));
    }
    IEnumerator Co_DoAttack(float coolDownTime)
    {
        _unitState.StartAttack();
        yield return StartCoroutine(Co_Attack());
        EndAttack();
        yield return WaitSecond(coolDownTime);
        _unitState.ReadyAttack();
    }

    protected virtual void EndAttack() => _unitState.EndAttack();

    protected abstract IEnumerator Co_Attack();
    protected virtual WaitForSeconds WaitSecond(float second) => new WaitForSeconds(second);
    protected void PlaySound(EffectSoundType soundType) => _worldAudioPlayer.PlayObjectEffectSound(_unitState.Spot, soundType);
}

public abstract class UnitAttackController : UnitAttackControllerTemplate
{
    protected override void DoAnima()
    {
        _animator.speed = _unit.Stats.AttackSpeed;
        base.DoAnima();
    }

    protected override void EndAttack()
    {
        base.EndAttack();
        if(_animator != null)
            _animator.speed = 1;
    }

    protected override WaitForSeconds WaitSecond(float second) => new WaitForSeconds(CalculateDelayTime(second));
    float CalculateDelayTime(float delay) => delay / _unit.Stats.AttackSpeed;
}

public class UnitAttackControllerGenerator
{
    public static T GenerateTemplate<T>(Multi_TeamSoldier unit) where T : UnitAttackControllerTemplate
    {
        var result = unit.GetComponent<T>();
        result.DependencyInject(unit._state, unit.Unit);
        return result;
    }

    public SwordmanAttackController GenerateSwordmanAttacker(Multi_TeamSoldier unit)
    {
        var result = GenerateTemplate<SwordmanAttackController>(unit);
        result.RecevieInject(unit);
        return result;
    }

    public ArcherNormalAttackController GenerateArcherAttacker(Multi_TeamSoldier unit, Transform shotPoint)
    {
        var result = GenerateTemplate<ArcherNormalAttackController>(unit);
        result.Inject(shotPoint, unit, unit.UnitAttacker);
        return result;
    }

    public ArcherSpecialAttackController GenerateArcherSkillAttcker(Multi_TeamSoldier unit, ArcherArrowShoter archerArrowShoter)
    {
        var result = GenerateTemplate<ArcherSpecialAttackController>(unit);
        result.Inject(archerArrowShoter, unit, unit.UnitAttacker);
        return result;
    }

    public SpearmanAttackController GenerateSpearmanAttcker(Multi_TeamSoldier unit)
    {
        var result = GenerateTemplate<SpearmanAttackController>(unit);
        result.Inject(unit);
        return result;
    }

    public MageAttackerController GenerateMageAattacker(Multi_TeamSoldier unit, ManaSystem manaSystem, Action<Vector3> shotEnergyball)
    {
        var result = GenerateTemplate<MageAttackerController>(unit);
        result.Inject(manaSystem, shotEnergyball);
        return result;
    }

    public static MageSpellController GenerateMageSkillController(Multi_TeamSoldier unit, ManaSystem manaSystem, UnitSkillController unitSkillController, float skillCastingTime)
    {
        var result = GenerateTemplate<MageSpellController>(unit);
        result.DependencyInject(manaSystem, unitSkillController, skillCastingTime);
        return result;
    }
}