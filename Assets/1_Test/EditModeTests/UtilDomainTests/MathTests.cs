using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MathTests
{
    [Test]
    [TestCase(100, 30, 30)]
    [TestCase(100, 4.5f, 4.5f)]
    [TestCase(100, 0, 0)]
    [TestCase(0, 5, 0)]
    public void 적절한_퍼센트_소수_값을_반환해야_함(float total, float percent, float expected)
    {
        Assert.That(expected, Is.EqualTo(MathUtil.CalculatePercentage(total, percent)).Within(0.01f));
    }

    [Test]
    [TestCase(100, 30, 30)]
    [TestCase(100, 4.8f, 5)]
    [TestCase(100, 0, 0)]
    [TestCase(0, 5, 0)]
    public void 적절한_퍼센트_정수_값을_반환해야_함(int total, float percent, int expected)
    {
        Assert.AreEqual(expected, MathUtil.CalculatePercentage(total, percent));
    }
}
