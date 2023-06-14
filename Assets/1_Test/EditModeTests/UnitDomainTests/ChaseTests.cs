using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ChaseTests
{
    const float Range = 20f;
    UnitChaseUseCase CreateSut() => new UnitChaseUseCase(Range);

    [Test]
    [TestCase(19, ChaseState.Far)]
    [TestCase(14, ChaseState.Close)]
    [TestCase(3, ChaseState.Contact)]
    public void 거리에_따라_적절한_상태를_반환해야_함(float z, ChaseState expected)
    {
        var sut = CreateSut();

        var result = sut.CalculateChaseState(Vector3.zero, new Vector3(0, 0, z));

        Assert.AreEqual(expected, result);
    }
}
