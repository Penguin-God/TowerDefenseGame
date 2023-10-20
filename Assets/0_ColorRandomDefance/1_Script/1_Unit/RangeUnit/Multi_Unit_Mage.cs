using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Mage : Multi_TeamSoldier
{
    [Header("메이지 변수")]
    [SerializeField] GameObject magicLight;
    [SerializeField] Transform energyBallShotPoint;

    ManaSystem _manaSystem;
    MageAttackerController _normalAttacker;
    MageSpellController _skillController;

    protected override void OnAwake()
    {
        _chaseSystem = gameObject.AddComponent<RangeChaser>();
        LoadMageStat();
        _normalAttacker = new UnitAttackControllerGenerator().GenerateMageAattacker(this, _manaSystem, ShotEnergyBall);
        _skillController = UnitAttackControllerGenerator.GenerateMageSkillController(this, _manaSystem, _unitSkillController, mageSkillCoolDownTime);

        canvasRectTransform = transform.GetComponentInChildren<RectTransform>();
        StartCoroutine(Co_SetCanvas());
    }

    void LoadMageStat()
    {
        if (Managers.Data.MageStatByFlag.TryGetValue(UnitFlags, out MageUnitStat stat))
        {
            _manaSystem = GetComponent<ManaSystem>();
            _manaSystem.SetInfo(stat.MaxMana, stat.AddMana);
        }
    }

    public void InjectSkillController(UnitSkillController unitSkillController) => _unitSkillController = unitSkillController;
    bool Skillable => _manaSystem != null && _manaSystem.IsManaFull;

    [PunRPC]
    protected override void Attack()
    {
        if (Skillable) _skillController.DoAttack(0);
        else _normalAttacker.DoAttack(AttackDelayTime);
    }

    void ShotEnergyBall(Vector3 pos) => Managers.Resources.Instantiate(GetWeaponPath(), pos).GetComponent<Multi_Projectile>().AttackShot(GetDir(), UnitAttacker.NormalAttack);
    string GetWeaponPath() => new ResourcesPathBuilder().BuildUnitWeaponPath(UnitFlags);
    Vector3 GetDir() => new ThorwPathCalculator().CalculateThorwPath_To_Monster(TargetEnemy, transform);

    [SerializeField] float mageSkillCoolDownTime;
    UnitSkillController _unitSkillController = null;
    
    protected override void ResetValue()
    {
        base.ResetValue();
        _manaSystem.ClearMana();
    }

    RectTransform canvasRectTransform;
    Vector3 sliderDir = new Vector3(90, 0, 0);
    IEnumerator Co_SetCanvas()
    {
        while (true)
        {
            canvasRectTransform.rotation = Quaternion.Euler(sliderDir);
            yield return null;
        }
    }
}