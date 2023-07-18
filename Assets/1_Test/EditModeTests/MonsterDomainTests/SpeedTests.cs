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
        public void 생성_시_원본과_현재_속도가_같아야_함()
        {
            // Arrange
            var sut = CreateSpeedManager();

            // Assert
            Assert.AreEqual(100, sut.OriginSpeed);
            Assert.AreEqual(100, sut.CurrentSpeed);
        }

        [Test]
        public void 슬로우_시_비율만큼_이속이_감소해야_함()
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
        public void 속도_복구_시_원래_속도로_돌아와야_함()
        {
            // Arrange
            var sut = CreateSpeedManager();
            sut.OnSlow(30); // Make it slower first

            // Act
            sut.RestoreSpeed();

            // Assert
            Assert.AreEqual(OriginSpeed, sut.CurrentSpeed);
        }

        [Test]
        public void 슬로우_상태에_맞게_참과_거짓을_반환해야_함()
        {
            // Arrange
            var sut = CreateSpeedManager();

            // Act
            sut.OnSlow(30); // Make it slower first

            // Assert
            Assert.IsTrue(sut.IsSlow);

            // Act
            sut.RestoreSpeed();

            // Assert
            Assert.IsFalse(sut.IsSlow);
        }
    }
}
