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
        public void ���ο�_��_������ŭ_�̼���_�����ؾ�_��()
        {
            var sut = CreateSpeedManager();
            
            sut.OnSlow(30);

            Assert.AreEqual(70, GetSpeed(sut));
        }

        [Test]
        public void �ӵ�_����_��_����_�ӵ���_���ƿ;�_��()
        {
            var sut = CreateSpeedManager();

            sut.OnSlow(30);
            sut.RestoreSpeed();

            Assert.AreEqual(OriginSpeed, GetSpeed(sut));
        }

        [Test]
        public void ���ο�_����_���¿�_����_����_����_�ٲ���_��()
        {
            var sut = CreateSpeedManager();

            sut.OnSlow(30f);
            Assert.AreEqual(30f, sut.ApplySlowRate);
            Assert.IsTrue(sut.IsSlow);

            sut.RestoreSpeed();
            Assert.AreEqual(0, sut.ApplySlowRate);
            Assert.IsFalse(sut.IsSlow);
        }

        [Test]
        public void ��_����_���ο��_����Ǹ�_�ȵ�()
        {
            var sut = CreateSpeedManager();

            Assert.IsTrue(sut.OnSlow(20));
            Assert.IsFalse(sut.OnSlow(15));
        }
    }
}
