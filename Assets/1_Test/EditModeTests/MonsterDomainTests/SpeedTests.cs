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

        [Test]
        public void ���ο�_��_������ŭ_�̼���_�����ؾ�_��()
        {
            // Arrange
            var sut = CreateSpeedManager();
            float slowRate = 30; // 30% slower

            // Act
            sut.OnSlow(slowRate);

            // Assert
            Assert.AreEqual(70, sut.CurrentSpeed);
        }

        [Test]
        public void �ӵ�_����_��_����_�ӵ���_���ƿ;�_��()
        {
            // Arrange
            var sut = CreateSpeedManager();
            sut.OnSlow(30); // Make it slower first

            // Act
            sut.RestoreSpeed();

            // Assert
            Assert.AreEqual(OriginSpeed, sut.CurrentSpeed);
        }
    }
}
