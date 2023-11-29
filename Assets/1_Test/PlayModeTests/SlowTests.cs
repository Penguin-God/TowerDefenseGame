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
    public IEnumerator 지속_슬로우는_특정_시간_후에_풀려야_함()
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
    public IEnumerator 무한_슬로우는_직접_풀어야_함()
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
    public IEnumerator 슬로우_중첩_시_더_강한_슬로우가_적용되어야_함()
    {
        yield return 슬로우_중첩_시_더_강한_슬로우가_적용되어야_함(CreateDurationSlow(30), CreateDurationSlow(60), 4);
        yield return 슬로우_중첩_시_더_강한_슬로우가_적용되어야_함(CreateDurationSlow(30), Slow.CreateInfinitySlow(60), 4);
        yield return 슬로우_중첩_시_더_강한_슬로우가_적용되어야_함(Slow.CreateInfinitySlow(20), Slow.CreateInfinitySlow(60), 4);
    }

    IEnumerator 슬로우_중첩_시_더_강한_슬로우가_적용되어야_함(Slow slow1, Slow slow2, float expected)
    {
        SetUp();
        _sut.ApplyNewSlow(slow1);
        _sut.ApplyNewSlow(slow2);
        
        yield return null;
        Assert.AreEqual(expected, _speedManager.CurrentSpeed);
    }

    [UnityTest]
    public IEnumerator 지속_슬로우가_끝나고_범위_슬로우가_남아있다면_그게_적용되어야_함()
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
