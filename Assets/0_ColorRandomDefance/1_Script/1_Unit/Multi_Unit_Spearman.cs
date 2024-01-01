using UnityEngine;

public class Multi_Unit_Spearman : Multi_TeamSoldier
{
    [Header("창병 변수")]
    [SerializeField] float _skillReboundTime;
    SpearmanAttackController _normalAttackController;
    SpearmanSkillAttackController _skillAttackContrller;
    NetworkAttackController _attackExcuter;
    protected override void OnAwake()
    {
        _chaseSystem = gameObject.AddComponent<MeeleChaser>();
        _normalAttackController = new UnitAttackControllerGenerator().GenerateSpearmanAttcker(this);
        
        _attackExcuter = gameObject.AddComponent<NetworkAttackController>();
        _attackExcuter.DependencyInject(Normal, SpecialAttack, new AttackCounter(3));
    }

    ThrowSpearData _throwSpearData;
    public void SetSpearData(ThrowSpearData throwSpearData)
    {
        _throwSpearData = throwSpearData;
        if(_skillAttackContrller == null)
            _skillAttackContrller = UnitAttackControllerGenerator.GenerateTemplate<SpearmanSkillAttackController>(this);
        _skillAttackContrller.ChangeSpearData(_throwSpearData, SkillAttack);
    }

    void Normal() => _normalAttackController.DoAttack(AttackDelayTime);
    void SpecialAttack() => _skillAttackContrller.DoAttack(_skillReboundTime);

    protected override void AttackToAll() => _attackExcuter.NetworkAttack();

    void SkillAttack(Multi_Enemy target) => UnitAttacker.SkillAttack(target, CalculateSpearDamage(target.enemyType));
    int CalculateSpearDamage(EnemyType enemyType) => Mathf.RoundToInt(UnitAttacker.CalculateDamage(enemyType) * _throwSpearData.AttackRate);
}
