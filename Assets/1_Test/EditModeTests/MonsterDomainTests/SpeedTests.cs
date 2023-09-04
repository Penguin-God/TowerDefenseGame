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
        public void 슬로우_시_비율만큼_이속이_감소해야_함()
        {
            var sut = CreateSpeedManager();
            
            sut.OnSlow(30);

            Assert.AreEqual(70, GetSpeed(sut));
        }

        [Test]
        public void 속도_복구_시_원래_속도로_돌아와야_함()
        {
            var sut = CreateSpeedManager();

            sut.OnSlow(30);
            sut.RestoreSpeed();

            Assert.AreEqual(OriginSpeed, GetSpeed(sut));
        }

        [Test]
        public void 슬로우_적용_상태에_따라_관련_값이_바뀌어야_함()
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
        public void 더_약한_슬로우는_적용되면_안됨()
        {
            var sut = CreateSpeedManager();

            Assert.IsTrue(sut.OnSlow(20));
            Assert.IsFalse(sut.OnSlow(15));
        }
    }
}
