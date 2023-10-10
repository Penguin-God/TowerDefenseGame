using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PoolingTests
{
    const string TestObjectPath = "Prefabs/Weapon/Arrows/YellowArrowTrail 1";
    [UnityTest]
    public IEnumerator Poolable을_소유한_객체의_풀이_없을_시_풀을_생성해야_함()
    {
        var poolManager = new PoolManager("@PoolManager");
        var sut = CreateResourcesManager(poolManager);

        Assert.NotNull(sut.Instantiate(TestObjectPath));
        yield return null;

        Assert.IsTrue(poolManager.ContainsPool("YellowArrowTrail 1"));
    }

    [UnityTest]
    public IEnumerator 풀에_객체가_없을_시_새_객체를_생성_후_반환해야_함()
    {
        var poolManager = new PoolManager("@PoolManager");
        poolManager.CreatePool(TestObjectPath, 0);
        var sut = CreateResourcesManager(poolManager);

        yield return null;
        
        Assert.NotNull(sut.Instantiate(TestObjectPath));
    }

    ResourcesManager CreateResourcesManager(PoolManager poolManager)
    {
        var result = new ResourcesManager();
        result.DependencyInject(poolManager);
        return result;
    }
}
