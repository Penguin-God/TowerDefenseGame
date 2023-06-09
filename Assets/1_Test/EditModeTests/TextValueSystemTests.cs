using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TextValueSystemTests
{
    UnitFlags RedArcher = new UnitFlags(0, 1);
    UnitFlags VioaltSpearman = new UnitFlags(5, 2);

    [Test]
    public void ����_�÷��׿�_����_������_Ű��_�����ؾ�_��()
    {
        var sut = new UnitKeyBuilder();

        Assert.AreEqual("At01", sut.BuildAttackKey(RedArcher));
        Assert.AreEqual("BAt01", sut.BuildBossAttackKey(RedArcher));
    }

    [Test]
    public void �нú��_������ŭ��_Ű��_�����ؾ�_��()
    {
        var sut = new UnitKeyBuilder();
        const int PassiveCount = 4;

        var result = sut.BuildPassiveKeys(VioaltSpearman, PassiveCount);

        Assert.AreEqual(result.Count, PassiveCount);
        for (int i = 0; i < result.Count; i++)
            Assert.AreEqual($"Pa52{i}", result[i]);
    }
}
