using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Mage : Multi_TeamSoldier
{
    [Header("메이지 변수")]
    //[SerializeField] MageUnitStat mageStat;
    //protected IReadOnlyList<float> skillStats;
    
    [SerializeField] GameObject magicLight;
    [SerializeField] Transform energyBallShotPoint;

    protected ManaSystem manaSystem;
    MageAttackerController _normalAttacker;
    MageSkillAttackController _skillController;

    protected override void OnAwake()
    {
        _chaseSystem = gameObject.AddComponent<RangeChaser>();
        LoadMageStat();
        _normalAttacker = new UnitAttackControllerGenerator().GenerateMageAattacker(this, manaSystem, ShotEnergyBall);
        _skillController = UnitAttackControllerGenerator.GenerateMageSkillController(this, manaSystem, _unitSkillController, mageSkillCoolDownTime);
    }

    void LoadMageStat()
    {
        if (Managers.Data.MageStatByFlag.TryGetValue(UnitFlags, out MageUnitStat stat))
        {
            // mageStat = stat;
            // skillStats = mageStat.SkillStats;
            manaSystem = GetComponent<ManaSystem>();
            manaSystem?.SetInfo(stat.MaxMana, stat.AddMana);
        }
    }

    public void InjectSkillController(UnitSkillController unitSkillController) => _unitSkillController = unitSkillController;
    bool Skillable => manaSystem != null && manaSystem.IsManaFull;

    [PunRPC]
    protected override void Attack()
    {
        // if (Skillable) MageSkile();
        if (Skillable) _skillController.DoAttack(0);
        else _normalAttacker.DoAttack(AttackDelayTime);
    }

    void ShotEnergyBall(Vector3 pos) => Managers.Resources.Instantiate(GetWeaponPath(), pos).GetComponent<Multi_Projectile>().AttackShot(GetDir(), UnitAttacker.NormalAttack);
    string GetWeaponPath() => new ResourcesPathBuilder().BuildUnitWeaponPath(UnitFlags);
    Vector3 GetDir() => new ThorwPathCalculator().CalculateThorwPath_To_Monster(TargetEnemy, transform);

    [SerializeField] float mageSkillCoolDownTime;
    protected UnitSkillController _unitSkillController = null;
    void MageSkile()
    {
        DoAttack();
        manaSystem?.ClearMana_RPC();
        _unitSkillController.DoSkill(this);
        StartCoroutine(Co_EndSkillAttack(mageSkillCoolDownTime)); // 임시방편
    }
    
    IEnumerator Co_EndSkillAttack(float skillTime)
    {
        yield return new WaitForSeconds(skillTime);
        base.EndAttack();
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        manaSystem.ClearMana_RPC();
    }
}