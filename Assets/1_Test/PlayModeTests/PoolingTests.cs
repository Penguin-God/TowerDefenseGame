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
        var poolManager = new PoolManager("@PoolManager");
        var sut = new ResourcesManager(poolManager);

        sut.Instantiate("Weapon/Arrows/YellowArrowTrail 1");
        yield return null;

        Assert.IsTrue(poolManager.ContainsPool("YellowArrowTrail 1"));
    }
}
