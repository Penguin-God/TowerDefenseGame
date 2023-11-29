using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SlowTests
{
    const float Speed = 10f;
    SlowController _sut;
    SpeedManager _speedManager;

    [SetUp]
    public void SetUp()
    {
        var controller = new GameObject().AddComponent<SlowController>();
        _speedManager = new(Speed);
        controller.DependencyInject(_speedManager);
    }

    Slow CreateDurationSlow(float intensity) => Slow.CreateDurationSlow(intensity, 0.001f);

    [UnityTest]
    public IEnumerator ����_���ο��_Ư��_�ð�_�Ŀ�_Ǯ����_��()
    {
        _sut.ApplyNewSlow(CreateDurationSlow(30));

        // Assert
        Assert.AreEqual(_speedManager.CurrentSpeed, 7f);
        Assert.IsTrue(_speedManager.IsSlow);
        yield return new WaitForSeconds(0.0011f);
        Assert.AreEqual(_speedManager.CurrentSpeed, 10f);
        Assert.IsFalse(_speedManager.IsSlow);
    }

    [UnityTest]
    public IEnumerator ����_���ο��_����_Ǯ���_��()
    {
        _sut.ApplyNewSlow(Slow.CreateInfinitySlow(30));

        Assert.AreEqual(_speedManager.CurrentSpeed, 7f);
        Assert.IsTrue(_speedManager.IsSlow);
        yield return null;

        _sut.ExitSlow();
        Assert.AreEqual(_speedManager.CurrentSpeed, 10f);
        Assert.IsTrue(_speedManager.IsSlow);
    }

    [UnityTest]
    public IEnumerator ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��()
    {
        yield return ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��(CreateDurationSlow(30), CreateDurationSlow(60), 4);
        yield return ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��(CreateDurationSlow(30), Slow.CreateInfinitySlow(60), 4);
        yield return ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��(Slow.CreateInfinitySlow(20), Slow.CreateInfinitySlow(60), 4);
    }

    IEnumerator ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��(Slow slow1, Slow slow2, float expected)
    {
        SetUp();
        _sut.ApplyNewSlow(slow1);
        _sut.ApplyNewSlow(slow2);
        
        yield return null;
        Assert.AreEqual(expected, _speedManager.CurrentSpeed);
    }

    [UnityTest]
    public IEnumerator ����_���ο찡_������_����_���ο찡_�����ִٸ�_�װ�_����Ǿ��_��()
    {
        _sut.ApplyNewSlow(CreateDurationSlow(30));
        _sut.ApplyNewSlow(Slow.CreateInfinitySlow(10));

        // Assert
        Assert.AreEqual(_speedManager.CurrentSpeed, 7f);
        Assert.IsTrue(_speedManager.IsSlow);
        yield return new WaitForSeconds(0.0011f);
        Assert.AreEqual(_speedManager.CurrentSpeed, 9f);
        Assert.IsTrue(_speedManager.IsSlow);
    }
}
