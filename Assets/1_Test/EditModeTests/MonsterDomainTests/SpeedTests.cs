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
        public void 생성_시_원본과_현재_속도가_같아야_함()
        {
            var sut = CreateSpeedManager();

            Assert.AreEqual(100, sut.OriginSpeed);
            Assert.AreEqual(100, GetSpeed(sut));
        }

        [Test]
        public void 속도_복구_시_원래_속도로_돌아와야_함()
        {
            var sut = CreateSpeedManager();

            sut.ChangeSpeed(30);
            sut.RestoreSpeed();

            Assert.AreEqual(OriginSpeed, GetSpeed(sut));
        }
    }
}
