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
    public IEnumerator 지속_슬로우는_특정_시간_후에_풀려야_함()
    {
        ApplySlow(CreateDurationSlow(30));

        // Assert
        AssertSpeedState(7, true);
        yield return new WaitForSeconds(0.0011f);
        AssertSpeedState(10, false);
    }

    [UnityTest]
    public IEnumerator 무한_슬로우는_직접_풀어야_함()
    {
        ApplySlow(Slow.CreateInfinitySlow(30));

        AssertSpeedState(7, true);
        yield return null;

        _sut.ExitInfinitySlow();
        AssertSpeedState(10, false);
    }

    [UnityTest]
    public IEnumerator 슬로우_중첩_시_더_강한_슬로우가_적용되어야_함()
    {
        yield return 슬로우_중첩_시_더_강한_슬로우가_적용되어야_함(CreateDurationSlow(30), CreateDurationSlow(50), 5);
        End();
        yield return 슬로우_중첩_시_더_강한_슬로우가_적용되어야_함(CreateDurationSlow(50), CreateDurationSlow(30), 5);
        End();
        yield return 슬로우_중첩_시_더_강한_슬로우가_적용되어야_함(CreateDurationSlow(30), Slow.CreateInfinitySlow(60), 4);
        End();
        yield return 슬로우_중첩_시_더_강한_슬로우가_적용되어야_함(Slow.CreateInfinitySlow(20), Slow.CreateInfinitySlow(70), 3);
    }

    IEnumerator 슬로우_중첩_시_더_강한_슬로우가_적용되어야_함(Slow slow1, Slow slow2, float expected)
    {
        SetUp();
        ApplySlow(slow1);
        ApplySlow(slow2);
        
        Assert.AreEqual(expected, _speedManager.CurrentSpeed);
        yield return null;
    }

    [UnityTest]
    public IEnumerator 지속_슬로우가_끝나고_범위_슬로우가_남아있다면_그게_적용되어야_함()
    {
        ApplySlow(CreateDurationSlow(30));
        ApplySlow(Slow.CreateInfinitySlow(10));

        // Assert
        AssertSpeedState(7, true);
        yield return new WaitForSeconds(0.0011f);
        AssertSpeedState(9, true);
    }

    [UnityTest]
    public IEnumerator 범위_슬로우가_끝나고_지속_슬로우_시간이_남아있다면_전에_슬로우_중_가장_강한_게_적용되어야_함()
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
    public IEnumerator 같은_슬로우가_오면_시간만_갱신되어야_함()
    {
        ApplySlow(CreateDurationSlow(10, 0.02f));
        yield return new WaitForSeconds(0.01f);
        ApplySlow(CreateDurationSlow(10, 0.02f));
        yield return new WaitForSeconds(0.015f);

        AssertSpeedState(9, true);
        yield return null;
    }
}
