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

    Slow CreateDurationSlow(float intensity, float duration = 0.001f) => Slow.CreateDurationSlow(intensity, duration);
    void ApplySlow(Slow slow) => _sut.ApplyNewSlow(slow);
    void AssertSpeedState(float speed, bool isSlow)
    {
        Assert.AreEqual(speed, _speedManager.CurrentSpeed);
        Assert.AreEqual(isSlow, _speedManager.IsSlow);
    }


    [UnityTest]
    public IEnumerator ����_���ο��_Ư��_�ð�_�Ŀ�_Ǯ����_��()
    {
        ApplySlow(CreateDurationSlow(30));

        // Assert
        AssertSpeedState(7, true);
        yield return new WaitForSeconds(0.0011f);
        AssertSpeedState(10, false);
    }

    [UnityTest]
    public IEnumerator ����_���ο��_����_Ǯ���_��()
    {
        ApplySlow(Slow.CreateInfinitySlow(30));

        AssertSpeedState(7, true);
        yield return null;

        _sut.ExitInfinitySlow();
        AssertSpeedState(10, false);
    }

    [UnityTest]
    public IEnumerator ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��()
    {
        yield return ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��(CreateDurationSlow(30), CreateDurationSlow(50), 5);
        End();
        yield return ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��(CreateDurationSlow(50), CreateDurationSlow(30), 5);
        End();
        yield return ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��(CreateDurationSlow(30), Slow.CreateInfinitySlow(60), 4);
        End();
        yield return ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��(Slow.CreateInfinitySlow(20), Slow.CreateInfinitySlow(70), 3);
    }

    IEnumerator ���ο�_��ø_��_��_����_���ο찡_����Ǿ��_��(Slow slow1, Slow slow2, float expected)
    {
        SetUp();
        ApplySlow(slow1);
        ApplySlow(slow2);
        
        Assert.AreEqual(expected, _speedManager.CurrentSpeed);
        yield return null;
    }

    [UnityTest]
    public IEnumerator ����_���ο찡_������_����_���ο찡_�����ִٸ�_�װ�_����Ǿ��_��()
    {
        ApplySlow(CreateDurationSlow(30));
        ApplySlow(Slow.CreateInfinitySlow(10));

        // Assert
        AssertSpeedState(7, true);
        yield return new WaitForSeconds(0.0011f);
        AssertSpeedState(9, true);
    }

    [UnityTest]
    public IEnumerator ����_���ο찡_������_����_���ο�_�ð���_�����ִٸ�_����_���ο�_��_����_����_��_����Ǿ��_��()
    {
        ApplySlow(CreateDurationSlow(20));
        ApplySlow(Slow.CreateInfinitySlow(30));
        ApplySlow(CreateDurationSlow(10));

        AssertSpeedState(7, true);
        _sut.ExitInfinitySlow();
        AssertSpeedState(8, true);
        yield return null;
    }


    [UnityTest]
    public IEnumerator ����_���ο찡_����_�ð���_���ŵǾ��_��()
    {
        ApplySlow(CreateDurationSlow(10, 0.02f));
        yield return new WaitForSeconds(0.01f);
        ApplySlow(CreateDurationSlow(10, 0.02f));
        yield return new WaitForSeconds(0.015f);

        AssertSpeedState(9, true);
        yield return null;
    }
}
