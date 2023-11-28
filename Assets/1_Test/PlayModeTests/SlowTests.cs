using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SlowTests
{
    [UnityTest]
    public IEnumerator 지속_슬로우는_특정_시간_후에_풀려야_함()
    {

        yield return null;
    }

    [UnityTest]
    public IEnumerator 더_강한_슬로우가_적용되어야_함()
    {

        yield return null;
    }

    [UnityTest]
    public IEnumerator 지속_슬로우가_끝나고_범위_슬로우가_남아있다면_그게_적용되어야_함()
    {

        yield return null;
    }
}
