using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MonsterDomainTests
{
    public class SpeedTests
    {
        const float OriginSpeed = 100;
        SpeedManager CreateSpeedManager() => new SpeedManager(OriginSpeed);
        float GetSpeed(SpeedManager speedManager) => speedManager.CurrentSpeed;

        [Test]
        public void ����_��_������_����_�ӵ���_���ƾ�_��()
        {
            var sut = CreateSpeedManager();

            Assert.AreEqual(100, sut.OriginSpeed);
            Assert.AreEqual(100, GetSpeed(sut));
        }

        [Test]
        public void �ӵ�_����_��_����_�ӵ���_���ƿ;�_��()
        {
            var sut = CreateSpeedManager();

            sut.ChangeSpeed(30);
            sut.RestoreSpeed();

            Assert.AreEqual(OriginSpeed, GetSpeed(sut));
        }
    }
}
