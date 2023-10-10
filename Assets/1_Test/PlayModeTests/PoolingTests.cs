using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PoolingTests
{
    const string TestObjectPath = "Prefabs/Weapon/Arrows/YellowArrowTrail 1";
    [UnityTest]
    public IEnumerator Poolable��_������_��ü��_Ǯ��_����_��_Ǯ��_�����ؾ�_��()
    {
        var poolManager = new PoolManager("@PoolManager");
        var sut = CreateResourcesManager(poolManager);

        Assert.NotNull(sut.Instantiate(TestObjectPath));
        yield return null;

        Assert.IsTrue(poolManager.ContainsPool("YellowArrowTrail 1"));
    }

    [UnityTest]
    public IEnumerator Ǯ��_��ü��_����_��_��_��ü��_����_��_��ȯ�ؾ�_��()
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
