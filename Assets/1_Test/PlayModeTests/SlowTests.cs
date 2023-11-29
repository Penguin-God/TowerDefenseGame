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
        _sut = new GameObject().AddComponent<SlowController>();
        _speedManager = new(Speed);
        _sut.DependencyInject(_speedManager);
    }

    [TearDown]
    public void End()
    {
        Object.Destroy(_sut.gameObject);
        _speedManager = null;
    }

    Slow CreateDurationSlow(float intensity) => Slow.CreateDurationSlow(intensity, 0.001f);

    [UnityTest]
    public IEnumerator ����_���ο��_Ư��_�ð�_�Ŀ�_Ǯ����_��()
    {
        _sut.ApplyNewSlow(CreateDurationSlow(30));

        // Assert
        Assert.AreEqual(7f, _speedManager.CurrentSpeed);
        Assert.IsTrue(_speedManager.IsSlow);
        yield return new WaitForSeconds(0.0011f);
        Assert.AreEqual(10f, _speedManager.CurrentSpeed);
        Assert.IsFalse(_speedManager.IsSlow);
    }

    [UnityTest]
    public IEnumerator ����_���ο��_����_Ǯ���_��()
    {
        _sut.ApplyNewSlow(Slow.CreateInfinitySlow(30));

        Assert.AreEqual(7f, _speedManager.CurrentSpeed);
        Assert.IsTrue(_speedManager.IsSlow);
        yield return null;

        _sut.ExitSlow();
        Assert.AreEqual(10f, _speedManager.CurrentSpeed);
        Assert.IsTrue(_speedManager.IsSlow);
    }

    [UnityTest]
    public IEnumerator ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��()
    {
        yield return ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��(CreateDurationSlow(30), CreateDurationSlow(50), 5);
        End();
        yield return ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��(CreateDurationSlow(30), Slow.CreateInfinitySlow(60), 4);
        End();
        yield return ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��(Slow.CreateInfinitySlow(20), Slow.CreateInfinitySlow(70), 3);
    }

    IEnumerator ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��(Slow slow1, Slow slow2, float expected)
    {
        SetUp();
        _sut.ApplyNewSlow(slow1);
        _sut.ApplyNewSlow(slow2);
        
        Assert.AreEqual(expected, _speedManager.CurrentSpeed);
        yield return null;
    }

    [UnityTest]
    public IEnumerator ����_���ο찡_������_����_���ο찡_�����ִٸ�_�װ�_����Ǿ��_��()
    {
        _sut.ApplyNewSlow(CreateDurationSlow(30));
        _sut.ApplyNewSlow(Slow.CreateInfinitySlow(10));

        // Assert
        Assert.AreEqual(7f, _speedManager.CurrentSpeed);
        Assert.IsTrue(_speedManager.IsSlow);
        yield return new WaitForSeconds(0.0011f);
        Assert.AreEqual(9f, _speedManager.CurrentSpeed);
        Assert.IsTrue(_speedManager.IsSlow);
    }
}
