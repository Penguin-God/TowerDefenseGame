using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitStateTests
{
    [Test]
    public void 공격_시_공격_중인_상태를_반환해야_함()
    {
        var sut = new UnitAttackState();

        var result = sut.DoAttack();

        Assert.IsFalse(result.IsAttackable);
        Assert.IsTrue(result.IsAttack);
    }

    [Test]
    public void 공격_준비_시_공격_가능한_상태를_반환해야_함()
    {
        var sut = new UnitAttackState();

        var result = sut.ReadyAttack();

        Assert.IsTrue(result.IsAttackable);
        Assert.IsFalse(result.IsAttack);
    }

    [Test]
    public void 공격이_끝나면_쿨다운_대기_중인_상태를_반환해야_함()
    {
        var sut = new UnitAttackState();

        var result = sut.AttackDone();

        Assert.IsFalse(result.IsAttackable);
        Assert.IsFalse(result.IsAttack);
    }
}
