using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MonsterSpeedTests
{
    MonsterSpeedManager CreateSpeedManager(float speed)
    {
        var result = new GameObject("speed").AddComponent<MonsterSpeedManager>();
        result.SetSpeed(speed);
        return result;
    }
    float GetSpeed(MonsterSpeedManager manager) => manager.SpeedManager.CurrentSpeed;
    
    [UnityTest]
    public IEnumerator �ð���_�ִ�_���ο��_�Ŀ�_�����Ǿ��_��()
    {
        var sut = CreateSpeedManager(5);
        sut.OnSlowWithTime(10, 0.0001f);

        Assert.AreEqual(4.5f, GetSpeed(sut));
        yield return new WaitForSeconds(0.001f);
        Assert.AreEqual(5f, GetSpeed(sut));
    }

    [UnityTest]
    public IEnumerator ��_����_���ο��_����Ǹ�_�ȵ�()
    {
        var sut = CreateSpeedManager(5);

        sut.OnSlow(20);
        Assert.AreEqual(4f, GetSpeed(sut));

        sut.OnSlow(15);
        Assert.AreEqual(4f, GetSpeed(sut));
        yield return null;
    }
}
