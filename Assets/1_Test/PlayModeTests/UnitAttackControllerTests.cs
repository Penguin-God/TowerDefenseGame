using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitAttackControllerTests
{
    UnitAttackState CreateAttackState(bool isAttackable, bool isAttack) => new UnitAttackState(isAttackable, isAttack);

    TestAttacker CreateAttacker(UnitStateManager unitStateManager, float attSpeed)
    {
        var result = new GameObject("Attacker").AddComponent<TestAttacker>();
        result.DependencyInject(unitStateManager, new Unit(new UnitFlags(), new UnitStats(new UnitDamageInfo(), 0, attackSpeed: attSpeed, 0, 0)));
        return result;
    }

    [UnityTest]
    public IEnumerator 유닛_공격은_정해진_시간에_따라_상태가_바뀌어야_함()
    {
        yield return 유닛_공격은_공속에_따라_상태가_바뀌어야_함(1, 0.02f, 0.021f, 0.021f);
        yield return 유닛_공격은_공속에_따라_상태가_바뀌어야_함(2, 0.02f, 0.011f, 0.011f);
    }

    public IEnumerator 유닛_공격은_공속에_따라_상태가_바뀌어야_함(float _attackSpeed, float coolDown, float firstDelay, float secondDelay)
    {
        var stateManager = new UnitStateManager(new ObjectSpot());
        var sut = CreateAttacker(stateManager, _attackSpeed);
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
