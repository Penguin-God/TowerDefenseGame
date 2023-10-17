using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitAttackControllerTests
{
    UnitAttackState CreateAttackState(bool isAttackable, bool isAttack) => new UnitAttackState(isAttackable, isAttack);

    [UnityTest]
    public IEnumerator ����_������_������_�ð���_����_���°�_�ٲ���_��()
    {
        yield return ����_������_���ӿ�_����_���°�_�ٲ���_��(1, 0.02f, 0.021f, 0.021f);
        yield return ����_������_���ӿ�_����_���°�_�ٲ���_��(2, 0.02f, 0.011f, 0.011f);
    }

    public IEnumerator ����_������_���ӿ�_����_���°�_�ٲ���_��(float _attackSpeed, float coolDown, float firstDelay, float secondDelay)
    {
        var sut = new GameObject("Attacker").AddComponent<TestAttacker>();
        var stateManager = new UnitStateManager(new ObjectSpot());
        sut.DependencyInject(stateManager, new Unit(new UnitFlags(), new UnitStats(new UnitDamageInfo(), 0, attackSpeed: _attackSpeed, 0, 0)));
        sut.DoAttack(coolDown);
        Assert.AreEqual(stateManager.UnitAttackState, CreateAttackState(false, true));
        yield return new WaitForSeconds(firstDelay);
        Assert.AreEqual(stateManager.UnitAttackState, CreateAttackState(false, false));
        yield return new WaitForSeconds(secondDelay);
        Assert.AreEqual(stateManager.UnitAttackState, CreateAttackState(true, false));
    }
}

public class TestAttacker : UnitAttackControllerTemplate
{
    protected override IEnumerator Co_Attack()
    {
        yield return WaitSecond(0.01f);
    }
}
