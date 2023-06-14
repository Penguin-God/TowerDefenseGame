using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitStateTests
{
    [Test]
    public void ����_��_����_����_���¸�_��ȯ�ؾ�_��()
    {
        var sut = new UnitAttackState();

        var result = sut.DoAttack();

        Assert.IsFalse(result.IsAttackable);
        Assert.IsTrue(result.IsAttack);
    }

    [Test]
    public void ����_�غ�_��_����_������_���¸�_��ȯ�ؾ�_��()
    {
        var sut = new UnitAttackState();

        var result = sut.ReadyAttack();

        Assert.IsTrue(result.IsAttackable);
        Assert.IsFalse(result.IsAttack);
    }

    [Test]
    public void ������_������_��ٿ�_���_����_���¸�_��ȯ�ؾ�_��()
    {
        var sut = new UnitAttackState();

        var result = sut.AttackDone();

        Assert.IsFalse(result.IsAttackable);
        Assert.IsFalse(result.IsAttack);
    }
}
