using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttackControllerTemplate : MonoBehaviour
{
    Animator _animator;
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

    void DoAnima()
    {
        if(_animator == null ) return;
        _animator.speed = _unit.Stats.AttackSpeed;
        _animator.SetTrigger(AnimationName);
    }
    public void DoAttack(float coolDownTime)
    {
        DoAnima();
        StartCoroutine(Co_DoAttack(coolDownTime));
    }
    IEnumerator Co_DoAttack(float coolDownTime)
    {
        _unitState.StartAttack();
        yield return StartCoroutine(Co_Attack());
        _unitState.EndAttack();
        yield return WaitSecond(coolDownTime);
        _unitState.ReadyAttack();
    }

    protected abstract IEnumerator Co_Attack();
    protected WaitForSeconds WaitSecond(float second) => new WaitForSeconds(CalculateDelayTime(second));
    protected float CalculateDelayTime(float delay) => delay / _unit.Stats.AttackSpeed;
    protected void PlaySound(EffectSoundType soundType) => _worldAudioPlayer.PlayObjectEffectSound(_unitState.Spot, soundType);
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