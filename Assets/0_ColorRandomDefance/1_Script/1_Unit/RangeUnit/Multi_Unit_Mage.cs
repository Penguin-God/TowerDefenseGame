using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Mage : Multi_TeamSoldier
{
    [Header("메이지 변수")]
    [SerializeField] MageUnitStat mageStat;
    protected IReadOnlyList<float> skillStats;
    [SerializeField] protected ProjectileData skillData;

    [SerializeField] GameObject magicLight;
    [SerializeField] Transform energyBallShotPoint;
    protected ProjectileThrowingUnit _energyBallThower;

    protected ManaSystem manaSystem;
    protected override void OnAwake()
    {
        _chaseSystem = gameObject.AddComponent<RangeChaser>();
        LoadMageStat();
        SetMageAwake();

        var pathBuilder = new ResourcesPathBuilder();
        skillData = new ProjectileData(pathBuilder.BuildMageSkillEffectPath(UnitFlags.UnitColor), transform, skillData.SpawnTransform);
        _energyBallThower = gameObject.AddComponent<ProjectileThrowingUnit>();
        _energyBallThower.SetInfo(pathBuilder.BuildUnitWeaponPath(UnitFlags), energyBallShotPoint);
        normalAttackSound = EffectSoundType.MageAttack;
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

    // 법사 고유의 Awake 대체 가상 함수
    public virtual void SetMageAwake() { }
    public void InjectSkillController(UnitSkillController unitSkillController) => _unitSkillController = unitSkillController;
    bool Skillable => manaSystem != null && manaSystem.IsManaFull;

    [PunRPC]
    protected override void Attack()
    {
        if (Skillable) SpecialAttack();
        else StartCoroutine(nameof(MageAttack));
    }

    protected IEnumerator MageAttack()
    {
        base.StartAttack();

        nav.isStopped = true;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.7f);
        magicLight.SetActive(true);

        // TODO : 딱 공격하려는 순간에 적이 죽어버리면 공격을 안함. 이건 판정 문제인데 그냥 target위치를 기억해서 거기다가 던지는게 나은듯
        if (PhotonNetwork.IsMasterClient && target != null && Chaseable)
        {
            _energyBallThower.FlatThrow(target, UnitAttacker.NormalAttack);
            manaSystem?.AddMana_RPC();
        }

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        nav.isStopped = false;

        base.EndAttack();
    }

    [SerializeField] float mageSkillCoolDownTime;
    protected UnitSkillController _unitSkillController = null;
    public override void SpecialAttack()
    {
        base.SpecialAttack();
        manaSystem?.ClearMana_RPC();
        if (_unitSkillController != null && PhotonNetwork.IsMasterClient)
            photonView.RPC(nameof(DoSkill), RpcTarget.All, target.GetComponent<PhotonView>().ViewID);
            // _unitSkillController.DoSkill(this);
        else if (PhotonNetwork.IsMasterClient)
            MageSkile();

        PlaySkillSound();
        StartCoroutine(Co_EndSkillAttack(mageSkillCoolDownTime)); // 임시방편
    }
    
    IEnumerator Co_EndSkillAttack(float skillTime)
    {
        yield return new WaitForSeconds(skillTime);
        base.EndSkillAttack(0);
    }

    [PunRPC] 
    void DoSkill(int targetId)
    {
        _targetManager.ChangedTarget(Managers.Multi.GetPhotonViewComponent<Multi_Enemy>(targetId));
        _unitSkillController.DoSkill(this);
    }

    protected GameObject SkillSpawn(Vector3 spawnPos) => WeaponSpawner.Spawn(skillData.WeaponPath, spawnPos);
    protected int CalculateSkillDamage(float rate) => Mathf.RoundToInt(Mathf.Max(Damage, BossDamage) * rate);
    protected virtual void MageSkile() { }
    protected virtual void PlaySkillSound() { }
}