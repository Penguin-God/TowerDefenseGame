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
    public void 유닛_플래그에_따른_데미지_키를_생성해야_함()
    {
        var sut = new UnitKeyBuilder();

        Assert.AreEqual("At01", sut.BuildAttackKey(RedArcher));
        Assert.AreEqual("BAt01", sut.BuildBossAttackKey(RedArcher));
    }

    [Test]
    public void 패시브는_개수만큼의_키를_생성해야_함()
    {
        var sut = new UnitKeyBuilder();
        const int PassiveCount = 4;

        var result = sut.BuildPassiveKeys(VioaltSpearman, PassiveCount);

        Assert.AreEqual(result.Count, PassiveCount);
        for (int i = 0; i < result.Count; i++)
            Assert.AreEqual($"Pa52{i}", result[i]);
    }
}
