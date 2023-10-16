using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Mage : Multi_TeamSoldier
{
    [Header("메이지 변수")]
    [SerializeField] MageUnitStat mageStat;
    protected IReadOnlyList<float> skillStats;
    
    [SerializeField] GameObject magicLight;
    [SerializeField] Transform energyBallShotPoint;

    protected ManaSystem manaSystem;
    MageAttackerController _normalAttacker;

    protected override void OnAwake()
    {
        _chaseSystem = gameObject.AddComponent<RangeChaser>();
        LoadMageStat();

        normalAttackSound = EffectSoundType.MageAttack;
        _normalAttacker = new UnitAttackControllerGenerator().GenerateMageAattacker(this, manaSystem, ShotEnergyBall);
    }

    void LoadMageStat()
    {
        if (Managers.Data.MageStatByFlag.TryGetValue(UnitFlags, out MageUnitStat stat))
        {
            mageStat = stat;
            skillStats = mageStat.SkillStats;
            manaSystem = GetComponent<ManaSystem>();
            manaSystem?.SetInfo(stat.MaxMana, stat.AddMana);
        }
    }

    public void InjectSkillController(UnitSkillController unitSkillController) => _unitSkillController = unitSkillController;
    bool Skillable => manaSystem != null && manaSystem.IsManaFull;

    [PunRPC]
    protected override void Attack()
    {
        if (Skillable) MageSkile();
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
        photonView.RPC(nameof(DoSkill), RpcTarget.All, target.GetComponent<PhotonView>().ViewID);
        StartCoroutine(Co_EndSkillAttack(mageSkillCoolDownTime)); // 임시방편
    }
    
    IEnumerator Co_EndSkillAttack(float skillTime)
    {
        yield return new WaitForSeconds(skillTime);
        base.EndAttack(0);
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        manaSystem.ClearMana_RPC();
    }

    [PunRPC] 
    void DoSkill(int targetId)
    {
        _targetManager.ChangedTarget(Managers.Multi.GetPhotonViewComponent<Multi_Enemy>(targetId));
        _unitSkillController.DoSkill(this);
    }
}