using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PoolingTests
{
    [UnityTest]
    public IEnumerator Poolable��_������_��ü��_Ǯ��_����_��_Ǯ��_�����ؾ�_��()
    {
        var sut = new ResourcesManager();
        var poolManager = new PoolManager();
        poolManager.Init("@PoolManager");

        sut.Instantiate("Weapon/Arrows/YellowArrowTrail 1");

        Assert.IsTrue(poolManager.ContainsPool("Weapon/Arrows/YellowArrowTrail 1"));
        yield return null;
    }
}
