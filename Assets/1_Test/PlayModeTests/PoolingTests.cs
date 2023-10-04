using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PoolingTests
{
    [UnityTest]
    public IEnumerator Poolable을_소유한_객체의_풀이_없을_시_풀을_생성해야_함()
    {
        var sut = new ResourcesManager();
        var poolManager = new PoolManager();
        poolManager.Init("@PoolManager");

        sut.Instantiate("Weapon/Arrows/YellowArrowTrail 1");

        Assert.IsTrue(poolManager.ContainsPool("Weapon/Arrows/YellowArrowTrail 1"));
        yield return null;
    }
}
