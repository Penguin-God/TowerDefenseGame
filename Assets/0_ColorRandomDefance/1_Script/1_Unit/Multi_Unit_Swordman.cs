using Photon.Pun;

public class Multi_Unit_Swordman : Multi_TeamSoldier
{
    UnitAttackControllerTemplate _unitAttackControllerTemplate;
    protected override void OnAwake()
    {
        _chaseSystem = gameObject.AddComponent<MeeleChaser>();
        _unitAttackControllerTemplate = new UnitAttackControllerGenerator().GenerateSwordmanAttacker(this);
    }

    protected override void AttackToAll() => photonView.RPC(nameof(Attack), RpcTarget.All);

    [PunRPC] protected void Attack() => _unitAttackControllerTemplate.DoAttack(AttackDelayTime);
}
