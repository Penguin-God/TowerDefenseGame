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
        var poolManager = new PoolManager("@PoolManager");
        var sut = new ResourcesManager(poolManager);

        sut.Instantiate("Weapon/Arrows/YellowArrowTrail 1");
        yield return null;

        Assert.IsTrue(poolManager.ContainsPool("YellowArrowTrail 1"));
    }
}
